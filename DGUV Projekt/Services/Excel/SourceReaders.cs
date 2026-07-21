using System;
using System.Collections.Generic;
using System.IO;
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
    /// Liest die Loopliste. Spalten (1-basiert): B=(=), C=(+Ort), D=(-BMK),
    /// E=Loop-Nr., J=Bauform, K=Nennstrom, L=Charakteristik, M=Kommentar.
    /// Es werden alle Zeilen genommen, deren Spalte B mit '=' beginnt
    /// (Kopfzeilen werden dadurch automatisch uebersprungen).
    /// </summary>
    public static class LooplistReader
    {
        public static IList<LooplistRow> Read(string path)
        {
            var result = new List<LooplistRow>();
            IWorkbook wb = CellHelper.Open(path);
            ISheet sheet = wb.GetSheetAt(0);

            for (int r = 0; r <= sheet.LastRowNum; r++)
            {
                IRow row = sheet.GetRow(r);
                string fg = CellHelper.Str(row, 1); // B
                if (!fg.StartsWith("=")) continue;

                result.Add(new LooplistRow
                {
                    Funktionsgruppe = fg,
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
    /// Liest die Erdungsverbindungen. Spalten (1-basiert): A=(=), B=(-BMK),
    /// C=von(=), D=von(+), H=nach(=), I=nach(+), L=Kommentar.
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
                    NachFunktion = CellHelper.Str(row, 7), // H
                    NachOrt = CellHelper.Str(row, 8),      // I
                    Kommentar = CellHelper.Str(row, 11)    // L
                });
            }
            return result;
        }
    }
}
