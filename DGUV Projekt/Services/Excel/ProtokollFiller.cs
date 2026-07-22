using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        // Aktuell werden bewusst nur Spalte B (Betriebsmittel) und der
        // Kommentar geschrieben; die uebrigen Spalten bleiben leer.
        private const int ZColFunktion = 1;   // B  Zeile oben: (=)Funktion; Zeile unten: "-BMK +Ort"
        private const int ZColKommentar = 26; // AA Kommentar

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

        /// <summary>
        /// Befuellt das ZLPE-Blatt aus der (gefilterten) Betriebsmittelliste.
        /// Die Betriebsmittel werden nach Ortskennzeichen (+) sortiert und je
        /// Ort mit einer fett-zentrierten Bannerzeile (ueber B..AA) abgetrennt.
        /// Je Betriebsmittel werden nur Spalte B und der Kommentar geschrieben:
        ///   Zeile oben:  (=)Funktion            | Kommentar (Spalte AA)
        ///   Zeile unten: "-BMK +Ort"
        /// </summary>
        public int FillZlpe(IList<BetriebsmittelRow> rows)
        {
            ISheet sheet = RequireSheet(SheetZlpe);
            int firstRow = ZlpeFirstDataRow - 1;

            var template = BlockTemplate.Capture(sheet, firstRow, ZlpeBlockHeight, ZlpeColCount);
            ICellStyle bannerStyle = CreateBannerStyle(sheet, firstRow);
            RemoveMergedRegionsFrom(sheet, firstRow);

            // Nach Ortskennzeichen sortieren (leere Orte ans Ende); Reihenfolge
            // innerhalb eines Ortes bleibt stabil erhalten.
            var sorted = rows
                .OrderBy(r => string.IsNullOrEmpty(r.Ort) ? "￿" : r.Ort, StringComparer.OrdinalIgnoreCase)
                .ToList();

            int cursor = firstRow;
            string currentOrt = null;

            foreach (BetriebsmittelRow src in sorted)
            {
                string ort = string.IsNullOrEmpty(src.Ort) ? "(ohne Ortskennzeichen)" : src.Ort;

                // Neuer Ort -> Bannerzeile einschieben.
                if (!string.Equals(ort, currentOrt, StringComparison.OrdinalIgnoreCase))
                {
                    WriteBanner(sheet, cursor, ort, bannerStyle);
                    cursor += ZlpeBlockHeight;
                    currentOrt = ort;
                }

                template.ApplyTo(sheet, cursor);
                IRow rowA = sheet.GetRow(cursor);
                IRow rowB = sheet.GetRow(cursor + 1);

                Set(rowA, ZColFunktion, src.Funktion);     // B oben: =Funktion
                Set(rowA, ZColKommentar, src.Kommentar);   // AA: Kommentar
                Set(rowB, ZColFunktion, Join(src.Bmk, src.Ort)); // B unten: "-BMK +Ort"

                cursor += ZlpeBlockHeight;
            }

            return sorted.Count;
        }

        // Erzeugt eine fett-zentrierte Bannerzeile ueber B..AA (2 Zeilen hoch),
        // wie die Ort-Trenner im Originalprotokoll.
        private void WriteBanner(ISheet sheet, int top, string text, ICellStyle style)
        {
            const int colB = 1;
            const int colAA = 26;
            for (int b = 0; b < ZlpeBlockHeight; b++)
            {
                IRow row = sheet.GetRow(top + b) ?? sheet.CreateRow(top + b);
                for (int c = colB; c <= colAA; c++)
                {
                    ICell cell = row.GetCell(c) ?? row.CreateCell(c);
                    cell.CellStyle = style;
                }
            }
            sheet.AddMergedRegion(new CellRangeAddress(top, top + ZlpeBlockHeight - 1, colB, colAA));
            sheet.GetRow(top).GetCell(colB).SetCellValue(text);
        }

        private ICellStyle CreateBannerStyle(ISheet sheet, int protoRow)
        {
            ICellStyle style = _wb.CreateCellStyle();
            IRow proto = sheet.GetRow(protoRow);
            ICell baseCell = proto != null ? proto.GetCell(1) : null;
            if (baseCell != null)
            {
                style.CloneStyleFrom(baseCell.CellStyle); // Rahmen/Schriftfamilie uebernehmen
            }
            IFont bold = _wb.CreateFont();
            bold.IsBold = true;
            style.SetFont(bold);
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            // Sauberer Rahmen ringsum (Basiszelle hat nur Teil-Rahmen).
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            return style;
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

        public void Dispose()
        {
            _wb?.Close();
        }
    }
}
