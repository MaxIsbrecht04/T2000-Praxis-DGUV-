using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;

namespace DGUV_Projekt.Services.Excel
{
    /// <summary>
    /// Kleine Helfer zum robusten Auslesen von NPOI-Zellen (egal ob Text/Zahl).
    /// </summary>
    internal static class CellHelper
    {
        private static readonly DataFormatter Formatter = new DataFormatter();

        public static string Str(IRow row, int col)
        {
            if (row == null) return string.Empty;
            ICell cell = row.GetCell(col);
            if (cell == null) return string.Empty;
            return (Formatter.FormatCellValue(cell) ?? string.Empty).Trim();
        }

        public static IWorkbook Open(string path)
        {
            // WorkbookFactory erkennt .xls (HSSF) und .xlsx/.xlsm (XSSF) automatisch.
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return WorkbookFactory.Create(fs);
            }
        }
    }

    /// <summary>
    /// Liest die Betriebsmittelliste (z.B. SIE_MB_DataList) und filtert die zu
    /// pruefenden Betriebsmittel heraus. Spalten (1-basiert): B=(=)Funktion,
    /// C=(+)Ort, D=(-)BMK, G=Artikelbezeichnung.
    ///
    /// Uebernommen werden Schutzorgane (BMK -QA/-QB/-FC) und Motoren/Antriebe
    /// (-MA/-TA); alles andere (Klemmen, Kabel, PE, Sensoren, ...) wird
    /// verworfen. Mehrfach vorkommende Geraete (=/+/-BMK) werden zusammengefasst;
    /// als Kommentar wird die aussagekraeftigste Artikelbezeichnung gewaehlt.
    /// </summary>
    public static class BetriebsmittelReader
    {
        private static readonly HashSet<string> KeepClasses =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "QA", "QB", "FC", "MA", "TA" };

        private static readonly Regex ClassRegex = new Regex(@"^-([A-Za-z]+)", RegexOptions.Compiled);

        // Bevorzugte, aussagekraeftige Artikelbezeichnungen fuer den Kommentar.
        private static readonly Regex KeywordRegex = new Regex(
            @"schalter|motorschutz|leistungsschalter|lasttrenn|sicherung|überwachung|umrichter|antrieb|protec|motor",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly HashSet<string> IgnoreText =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "CAD-Systemeintrag", "None" };

        public static IList<BetriebsmittelRow> Read(string path)
        {
            IWorkbook wb = CellHelper.Open(path);
            ISheet sheet = wb.GetSheet("SIE_MB_DataList") ?? wb.GetSheetAt(0);

            var map = new Dictionary<string, BetriebsmittelRow>();
            var order = new List<string>();

            for (int r = 0; r <= sheet.LastRowNum; r++)
            {
                IRow row = sheet.GetRow(r);
                string fg = CellHelper.Str(row, 1);  // B (=)
                string bmk = CellHelper.Str(row, 3); // D (-)
                if (!fg.StartsWith("=") || bmk.Length == 0) continue;
                if (!KeepClasses.Contains(ClassOf(bmk))) continue;

                string ort = CellHelper.Str(row, 2); // C (+)
                string bez = CellHelper.Str(row, 6); // G Artikelbezeichnung
                string key = fg + "|" + ort + "|" + bmk;

                BetriebsmittelRow dev;
                if (!map.TryGetValue(key, out dev))
                {
                    dev = new BetriebsmittelRow { Funktion = fg, Ort = ort, Bmk = bmk, Kommentar = string.Empty };
                    map[key] = dev;
                    order.Add(key);
                }

                if (IsMeaningful(bez) &&
                    (dev.Kommentar.Length == 0 || (KeywordRegex.IsMatch(bez) && !KeywordRegex.IsMatch(dev.Kommentar))))
                {
                    dev.Kommentar = bez;
                }
            }

            var result = new List<BetriebsmittelRow>();
            foreach (string k in order) result.Add(map[k]);
            return result;
        }

        private static string ClassOf(string bmk)
        {
            Match m = ClassRegex.Match(bmk.Trim());
            return m.Success ? m.Groups[1].Value : string.Empty;
        }

        private static bool IsMeaningful(string bez)
        {
            return bez.Length > 0 && !IgnoreText.Contains(bez);
        }
    }

    /// <summary>
    /// Liest die Erdungsverbindungen. Spalten (1-basiert): A=(=), B=(-BMK),
    /// C=von(=), D=von(+), F=Erdungsklasse (POT/NET/MESH-BN), H=nach(=),
    /// I=nach(+), L=Kommentar.
    /// Es werden alle Zeilen genommen, deren Spalte A mit '=' beginnt.
    /// </summary>
    public static class GroundConnectionsReader
    {
        public static IList<GroundRow> Read(string path)
        {
            var result = new List<GroundRow>();
            IWorkbook wb = CellHelper.Open(path);
            ISheet sheet = wb.GetSheetAt(0);

            for (int r = 0; r <= sheet.LastRowNum; r++)
            {
                IRow row = sheet.GetRow(r);
                string fg = CellHelper.Str(row, 0); // A
                if (!fg.StartsWith("=")) continue;

                result.Add(new GroundRow
                {
                    Funktionsgruppe = fg,
                    Bmk = CellHelper.Str(row, 1),          // B
                    VonFunktion = CellHelper.Str(row, 2),  // C
                    VonOrt = CellHelper.Str(row, 3),       // D
                    Erdungsklasse = MapErdungsklasse(CellHelper.Str(row, 5)), // F
                    NachFunktion = CellHelper.Str(row, 7), // H
                    NachOrt = CellHelper.Str(row, 8),      // I
                    Kommentar = CellHelper.Str(row, 11)    // L
                });
            }
            return result;
        }

        // Uebersetzt die Erdungsklasse aus dem EPLAN-Export in die Schreibweise
        // des DGUV-Protokolls. EPLAN exportiert "POT" fuer den vermaschten
        // Potentialausgleich; im Protokoll steht dort "MESH-BN". Andere Werte
        // (z.B. "NET") bleiben unveraendert. Bei Bedarf pro Anlage erweiterbar.
        private static readonly Dictionary<string, string> ErdungsklasseMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "POT", "MESH-BN" }
            };

        private static string MapErdungsklasse(string value)
        {
            string mapped;
            return ErdungsklasseMap.TryGetValue(value, out mapped) ? mapped : value;
        }
    }
}
