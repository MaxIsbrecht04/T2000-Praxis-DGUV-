using System.Collections.Generic;

namespace DGUV_Projekt.Services.Excel
{
    /// <summary>
    /// Eine Zeile aus der Loopliste (Quelle fuer das Blatt
    /// "Messdatenblatt ZLPE IK RISO").
    /// </summary>
    public class LooplistRow
    {
        public string Funktionsgruppe { get; set; } // (=) z.B. =001EK_122
        public string Ort { get; set; }             // (+) z.B. +H011
        public string Bmk { get; set; }             // (-) z.B. -QB1  (Ueberstromschutzorgan)
        public string Loop { get; set; }            // Zusatzinfo / Loop-Nr. z.B. 501
        public string Bauform { get; set; }         // Technische Kenngroesse Bauform z.B. 63A
        public string Nennstrom { get; set; }       // Technische Kenngroesse Nennstrom z.B. 50A
        public string Charakteristik { get; set; }  // Betriebsklasse/Charakteristik z.B. gG
        public string Kommentar { get; set; }
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
