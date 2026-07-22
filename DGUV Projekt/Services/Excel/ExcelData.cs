using System.Collections.Generic;

namespace DGUV_Projekt.Services.Excel
{
    /// <summary>
    /// Ein zu pruefendes Betriebsmittel aus der Betriebsmittelliste (Quelle
    /// fuer das Blatt "Messdatenblatt ZLPE IK RISO"). Gefiltert auf
    /// Schutzorgane (-QA/-QB/-FC) und Motoren/Antriebe (-MA/-TA).
    /// </summary>
    public class BetriebsmittelRow
    {
        public string Funktion { get; set; }   // (=) z.B. =004VW_026   -> Spalte B oben
        public string Ort { get; set; }        // (+) z.B. +H011 / +X
        public string Bmk { get; set; }        // (-) z.B. -QA1 / -MA1  -> Spalte B unten "(-BMK +Ort)"
        public string Kommentar { get; set; }  // Artikelbezeichnung      -> Spalte AA
    }

    /// <summary>
    /// Eine Zeile aus den Erdungsverbindungen (Quelle fuer das Blatt
    /// "Messdatenblatt RPE").
    /// </summary>
    public class GroundRow
    {
        public string Funktionsgruppe { get; set; } // (=) BTM Kabel
        public string Bmk { get; set; }             // (-) BTMK z.B. -WE2
        public string VonFunktion { get; set; }     // von Betriebsmittel (=)
        public string VonOrt { get; set; }          // von Betriebsmittel (+)
        public string Erdungsklasse { get; set; }   // [POT] / [NET] / [MESH-BN]
        public string NachFunktion { get; set; }    // nach Betriebsmittel (=)
        public string NachOrt { get; set; }         // nach Betriebsmittel (+)
        public string Kommentar { get; set; }
    }

    /// <summary>
    /// Ergebnis eines Ausfuell-Vorgangs (fuer die Statusmeldung).
    /// </summary>
    public class FillResult
    {
        public int ZlpeRows { get; set; } = -1; // -1 = nicht ausgefuehrt
        public int RpeRows { get; set; } = -1;

        public List<string> Warnings { get; } = new List<string>();
    }
}
