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
    /// Liest die SAG-Kabelübersicht und filtert die Leistungskabel (-WD*) heraus.
    /// Spalten: A=volle Kabelbezeichnung, E=Typnummer,
    /// G=Quelle (volle Bezeichnung), H=Ziel (volle Bezeichnung).
    /// </summary>
    public static class KabelUebersichtReader
    {
        // "==133_32_K17=004VW_026-WD2.1" -> fg "=004VW_026", kabel "-WD2.1"
        private static readonly Regex KabelRegex =
            new Regex(@"^==[^=+\-]*(=[0-9A-Za-z._]+)(-WD[\w.]*)$", RegexOptions.Compiled);

        public static IList<KabelRow> Read(string path)
        {
            var result = new List<KabelRow>();
            IWorkbook wb = CellHelper.Open(path);
            ISheet sheet = wb.GetSheetAt(0);

            for (int r = 0; r <= sheet.LastRowNum; r++)
            {
                IRow row = sheet.GetRow(r);
                Match m = KabelRegex.Match(CellHelper.Str(row, 0)); // A
                if (!m.Success) continue;

                result.Add(new KabelRow
                {
                    Funktion = m.Groups[1].Value,
                    Kabel = m.Groups[2].Value,
                    Typ = CellHelper.Str(row, 4),   // E Typnummer
                    Quelle = CellHelper.Str(row, 6),// G
                    Ziel = CellHelper.Str(row, 7)   // H
                });
            }
            return result;
        }
    }

    /// <summary>
    /// Liest die Loopliste.
    /// Spalten: B=(=), C=(+Ort), D=(-BMK), E=Loop,
    /// J=Bauform, K=Nennstrom, L=Charakteristik, M=Bemerkung.
    /// </summary>
    public static class LooplistReader
    {
        public static IList<LoopRow> Read(string path)
        {
            var result = new List<LoopRow>();
            IWorkbook wb = CellHelper.Open(path);
            ISheet sheet = wb.GetSheetAt(0);

            for (int r = 0; r <= sheet.LastRowNum; r++)
            {
                IRow row = sheet.GetRow(r);
                string fg = CellHelper.Str(row, 1); // B
                if (!fg.StartsWith("=")) continue;

                result.Add(new LoopRow
                {
                    Funktion = fg,
                    Ort = CellHelper.Str(row, 2),            // C
                    Bmk = CellHelper.Str(row, 3),            // D
                    Loop = CellHelper.Str(row, 4),           // E
                    Bauform = CellHelper.Str(row, 9),        // J
                    Nennstrom = CellHelper.Str(row, 10),     // K
                    Charakteristik = CellHelper.Str(row, 11),// L
                    Kommentar = CellHelper.Str(row, 12)      // M
                });
            }
            return result;
        }
    }

    /// <summary>
    /// Liest die Erdungsverbindungen. 
    /// Spalten: A=(=), B=(-BMK),
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

        // übersetzt die Erdungsklasse aus dem EPLAN-Export in die Schreibweise
        // des DGUV-Protokolls. EPLAN exportiert "POT" fuer den vermaschten
        // Potentialausgleich; im Protokoll steht dort "MESH-BN". Andere Werte
        // (z.B. "NET") bleiben unverändert. Bei Bedarf pro Anlage erweiterbar.
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
