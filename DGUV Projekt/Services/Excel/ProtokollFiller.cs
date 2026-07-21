using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;

namespace DGUV_Projekt.Services.Excel
{
    /// <summary>
    /// Oeffnet die Pruefprotokoll-Vorlage und befuellt die beiden relevanten
    /// Untertabellen:
    ///   - "Messdatenblatt ZLPE IK RISO"  aus der Loopliste
    ///   - "Messdatenblatt RPE"           aus den Erdungsverbindungen
    ///
    /// Nur die Struktur-/Stammdaten-Spalten werden geschrieben. Die reinen
    /// Messwert-Spalten (Ik, Schleifenwiderstand, RISO, gemessener RPE) bleiben
    /// leer und werden vor Ort bzw. aus einem Pruefgeraet-Export ergaenzt.
    /// </summary>
    public class ProtokollFiller : IDisposable
    {
        // ---- Blattnamen in der Vorlage ------------------------------------
        private const string SheetZlpe = "Messdatenblatt ZLPE IK RISO";
        private const string SheetRpe = "Messdatenblatt RPE";

        // ---- Erste Datenzeile je Blatt (1-basiert, wie in Excel angezeigt) --
        // Bei abweichender Vorlage hier anpassen.
        private const int ZlpeFirstDataRow = 11; // ZLPE: 2 Zeilen je Eintrag
        private const int RpeFirstDataRow = 8;    // RPE : 1 Zeile je Eintrag

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
            int startIdx = ZlpeFirstDataRow - 1;

            for (int i = 0; i < rows.Count; i++)
            {
                LooplistRow src = rows[i];
                IRow rowA = GetOrCreate(sheet, startIdx + i * 2);
                IRow rowB = GetOrCreate(sheet, startIdx + i * 2 + 1);

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
            int startIdx = RpeFirstDataRow - 1;

            for (int i = 0; i < rows.Count; i++)
            {
                GroundRow src = rows[i];
                IRow row = GetOrCreate(sheet, startIdx + i);

                Set(row, RColFunktion, src.Funktionsgruppe);
                Set(row, RColBmk, src.Bmk);
                Set(row, RColVonFunk, src.VonFunktion);
                Set(row, RColVonOrt, src.VonOrt);
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

        private static IRow GetOrCreate(ISheet sheet, int rowIdx)
        {
            return sheet.GetRow(rowIdx) ?? sheet.CreateRow(rowIdx);
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
