using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace DGUV_Projekt.Services.Excel
{
    /// <summary>
    /// Oeffnet die Pruefprotokoll-Vorlage und befuellt die beiden relevanten
    /// Untertabellen:
    ///   - "Messdatenblatt ZLPE IK RISO"  aus der Loopliste (2 Zeilen je Eintrag)
    ///   - "Messdatenblatt RPE"           aus den Erdungsverbindungen (1 Zeile)
    ///
    /// Da die Vorlage nur wenige vorformatierte Zeilen enthaelt, wird pro
    /// Eintrag der Formatierungs-Block der ersten Datenzeile(n) geklont:
    /// Zellstile/Rahmen, Zeilenhoehe und die vertikal verbundenen Zellen.
    /// So bekommen auch alle zusaetzlich angelegten Zeilen die korrekte
    /// Formatierung.
    ///
    /// Nur Struktur-/Stammdaten werden geschrieben; die reinen Messwert-Spalten
    /// bleiben leer.
    /// </summary>
    public class ProtokollFiller : IDisposable
    {
        // ---- Blattnamen in der Vorlage ------------------------------------
        private const string SheetZlpe = "Messdatenblatt ZLPE IK RISO";
        private const string SheetRpe = "Messdatenblatt RPE";

        // ---- Erste Datenzeile je Blatt (1-basiert, wie in Excel) ----------
        private const int ZlpeFirstDataRow = 11; // ZLPE: 2 Zeilen je Eintrag
        private const int RpeFirstDataRow = 8;    // RPE : 1 Zeile je Eintrag

        private const int ZlpeBlockHeight = 2;
        private const int RpeBlockHeight = 1;

        private const int ZlpeColCount = 27; // A..AA
        private const int RpeColCount = 12;  // A..L

        // ---- Spaltenindizes (0-basiert) ZLPE ------------------------------
        private const int ZColFunktion = 1;   // B  (=)Funktionsgruppe  (Zeile A)
        private const int ZColZusatz = 2;     // C  Zusatzinfo / Loop-Nr.
        private const int ZColBauform = 7;    // H  Technische Kenngroesse Bauform
        private const int ZColNennstrom = 8;  // I  Nennstrom (Setting)
        private const int ZColCharakt = 9;    // J  Betriebsklasse/Charakteristik
        private const int ZColKommentar = 26; // AA Kommentar
        // Zeile B des Eintrags: Spalte B = "-BMK +Ort"

        // ---- Spaltenindizes (0-basiert) RPE -------------------------------
        private const int RColFunktion = 1;   // B  (=)Funktionsgruppe
        private const int RColBmk = 2;        // C  (-)BTMK
        private const int RColVonFunk = 6;    // G  von (=)
        private const int RColVonOrt = 7;     // H  von (+)
        private const int RColErdung = 8;     // I  Erdungsklasse
        private const int RColNachFunk = 9;   // J  nach (=)
        private const int RColNachOrt = 10;   // K  nach (+)
        private const int RColKommentar = 11; // L  Kommentar

        private readonly IWorkbook _wb;

        public ProtokollFiller(string templatePath)
        {
            using (FileStream fs = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                _wb = WorkbookFactory.Create(fs);
            }
        }

        /// <summary>Befuellt das ZLPE-Blatt aus der Loopliste.</summary>
        public int FillZlpe(IList<LooplistRow> rows)
        {
            ISheet sheet = RequireSheet(SheetZlpe);
            int firstRow = ZlpeFirstDataRow - 1;

            var template = BlockTemplate.Capture(sheet, firstRow, ZlpeBlockHeight, ZlpeColCount);
            RemoveMergedRegionsFrom(sheet, firstRow);

            for (int i = 0; i < rows.Count; i++)
            {
                int top = firstRow + i * ZlpeBlockHeight;
                template.ApplyTo(sheet, top);

                LooplistRow src = rows[i];
                IRow rowA = sheet.GetRow(top);
                IRow rowB = sheet.GetRow(top + 1);

                Set(rowA, ZColFunktion, src.Funktionsgruppe);
                Set(rowA, ZColZusatz, src.Loop);
                Set(rowA, ZColBauform, StripAmpere(src.Bauform));
                Set(rowA, ZColNennstrom, StripAmpere(src.Nennstrom));
                Set(rowA, ZColCharakt, src.Charakteristik);
                Set(rowA, ZColKommentar, src.Kommentar);

                // 2. Zeile: "-BMK +Ort" (z.B. "-QB1 +H011")
                Set(rowB, ZColFunktion, Join(src.Bmk, src.Ort));
            }
            return rows.Count;
        }

        /// <summary>Befuellt das RPE-Blatt aus den Erdungsverbindungen.</summary>
        public int FillRpe(IList<GroundRow> rows)
        {
            ISheet sheet = RequireSheet(SheetRpe);
            int firstRow = RpeFirstDataRow - 1;

            var template = BlockTemplate.Capture(sheet, firstRow, RpeBlockHeight, RpeColCount);
            RemoveMergedRegionsFrom(sheet, firstRow);

            for (int i = 0; i < rows.Count; i++)
            {
                int top = firstRow + i * RpeBlockHeight;
                template.ApplyTo(sheet, top);

                GroundRow src = rows[i];
                IRow row = sheet.GetRow(top);

                Set(row, RColFunktion, src.Funktionsgruppe);
                Set(row, RColBmk, src.Bmk);
                Set(row, RColVonFunk, src.VonFunktion);
                Set(row, RColVonOrt, src.VonOrt);
                Set(row, RColErdung, src.Erdungsklasse);
                Set(row, RColNachFunk, src.NachFunktion);
                Set(row, RColNachOrt, src.NachOrt);
                Set(row, RColKommentar, src.Kommentar);
            }
            return rows.Count;
        }

        public void Save(string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                _wb.Write(fs);
            }
        }

        // ---- Formatierungs-Vorlage eines Eintrags-Blocks ------------------

        /// <summary>
        /// Faengt Stile, Zeilenhoehen und verbundene Zellen des ersten
        /// Eintrags-Blocks ein und kann sie auf beliebige weitere Bloecke
        /// uebertragen.
        /// </summary>
        private class BlockTemplate
        {
            private readonly int _firstRow;
            private readonly int _blockHeight;
            private readonly int _colCount;
            private readonly ICellStyle[][] _styles; // [zeileImBlock][spalte]
            private readonly short[] _heights;
            private readonly List<int[]> _merges;     // {zeileOffset0, zeileOffset1, spalte0, spalte1}

            private BlockTemplate(int firstRow, int blockHeight, int colCount,
                ICellStyle[][] styles, short[] heights, List<int[]> merges)
            {
                _firstRow = firstRow;
                _blockHeight = blockHeight;
                _colCount = colCount;
                _styles = styles;
                _heights = heights;
                _merges = merges;
            }

            public static BlockTemplate Capture(ISheet sheet, int firstRow, int blockHeight, int colCount)
            {
                var styles = new ICellStyle[blockHeight][];
                var heights = new short[blockHeight];

                for (int b = 0; b < blockHeight; b++)
                {
                    styles[b] = new ICellStyle[colCount];
                    IRow row = sheet.GetRow(firstRow + b);
                    heights[b] = row != null ? row.Height : (short)-1;
                    if (row == null) continue;
                    for (int c = 0; c < colCount; c++)
                    {
                        ICell cell = row.GetCell(c);
                        if (cell != null) styles[b][c] = cell.CellStyle;
                    }
                }

                // Verbundene Zellen, die im ersten Block beginnen, als Muster merken.
                var merges = new List<int[]>();
                for (int i = 0; i < sheet.NumMergedRegions; i++)
                {
                    CellRangeAddress m = sheet.GetMergedRegion(i);
                    if (m.FirstRow == firstRow)
                    {
                        merges.Add(new[] { m.FirstRow - firstRow, m.LastRow - firstRow, m.FirstColumn, m.LastColumn });
                    }
                }

                return new BlockTemplate(firstRow, blockHeight, colCount, styles, heights, merges);
            }

            public void ApplyTo(ISheet sheet, int top)
            {
                for (int b = 0; b < _blockHeight; b++)
                {
                    IRow row = sheet.GetRow(top + b) ?? sheet.CreateRow(top + b);
                    if (_heights[b] >= 0) row.Height = _heights[b];

                    for (int c = 0; c < _colCount; c++)
                    {
                        if (_styles[b][c] == null) continue;
                        ICell cell = row.GetCell(c) ?? row.CreateCell(c);
                        cell.CellStyle = _styles[b][c];
                    }
                }

                foreach (int[] mo in _merges)
                {
                    sheet.AddMergedRegion(new CellRangeAddress(top + mo[0], top + mo[1], mo[2], mo[3]));
                }
            }
        }

        // ---- Helfer -------------------------------------------------------

        private ISheet RequireSheet(string name)
        {
            ISheet sheet = _wb.GetSheet(name);
            if (sheet == null)
            {
                throw new InvalidOperationException(
                    $"Blatt \"{name}\" wurde in der Vorlage nicht gefunden.");
            }
            return sheet;
        }

        // Entfernt alle verbundenen Zellen ab der ersten Datenzeile, damit die
        // pro Eintrag neu gesetzten Merges nicht mit alten kollidieren.
        private static void RemoveMergedRegionsFrom(ISheet sheet, int firstRow)
        {
            for (int i = sheet.NumMergedRegions - 1; i >= 0; i--)
            {
                if (sheet.GetMergedRegion(i).FirstRow >= firstRow)
                {
                    sheet.RemoveMergedRegion(i);
                }
            }
        }

        private static void Set(IRow row, int col, string value)
        {
            if (string.IsNullOrEmpty(value)) return; // leere Werte nicht schreiben
            ICell cell = row.GetCell(col) ?? row.CreateCell(col);
            cell.SetCellValue(value);
        }

        private static string Join(string bmk, string ort)
        {
            return $"{bmk} {ort}".Trim();
        }

        // "63A" -> "63", "1-10A" -> "1-10"; die Spalte traegt die Einheit [A] bereits.
        private static string StripAmpere(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            string v = value.Trim();
            if (v.EndsWith("A") || v.EndsWith("a"))
            {
                v = v.Substring(0, v.Length - 1).Trim();
            }
            return v;
        }

        public void Dispose()
        {
            _wb?.Close();
        }
    }
}
