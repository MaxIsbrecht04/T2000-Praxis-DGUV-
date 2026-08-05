using System.Collections.Generic;

namespace DGUV_Projekt.Services.Excel
{   
    // Daten aus der KabelListe
    public class KabelRow
    {
        public string Funktion { get; set; }   // (=) Stromkreis, z.B. =004VW_026
        public string Kabel { get; set; }      // (-) Kabel-BMK, z.B. -WD2.1
        public string Typ { get; set; }        // Typnummer, z.B. "OELFLEX CLASSIC 110 3G2,5"
        public string Quelle { get; set; }     // volle Bezeichnung Kabelanfang
        public string Ziel { get; set; }       // volle Bezeichnung Kabelende
    }

    // Daten aus der LoopListe
    public class LoopRow
    {
        public string Funktion { get; set; }       // (=) z.B. =000EEA507
        public string Ort { get; set; }            // (+) z.B. +H011
        public string Bmk { get; set; }            // (-) Schutzorgan, z.B. -QA1
        public string Loop { get; set; }           // Loop-Nr., z.B. 507
        public string Bauform { get; set; }        // z.B. 15-36A
        public string Nennstrom { get; set; }      // z.B. 15A
        public string Charakteristik { get; set; } // z.B. K: 8 / gG
        public string Kommentar { get; set; }
    }

    // Für die Daten in der ZLPE Tabelle
    public class ZlpeEintrag
    {
        public string Funktion { get; set; }       // Spalte B oben: (=) Stromkreis
        public string Kabel { get; set; }          // Spalte D: (-) Kabel-BMK (leer bei Abgang ohne Kabel)
        public string Loop { get; set; }           // Spalte C: Loop-Nr. des speisenden Abgangs
        public string Ort { get; set; }            // Gruppierung/Banner: +Ort des speisenden Schranks
        public string SchutzBmk { get; set; }      // Spalte B unten: (-) speisendes Schutzorgan
        public string Bauform { get; set; }        // Spalte H
        public string Nennstrom { get; set; }      // Spalte I
        public string Charakteristik { get; set; } // Spalte J
        public string Querschnitt { get; set; }    // Spalte G (nur wenn sicher aus Typ ableitbar)
        public string Kommentar { get; set; }      // Spalte AA
    }

    // Für die Verbindungen aus den Erdungsverbindungen
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

    // Ergebnis eines Ausfüll-Vorgangs (für die Statusmeldung).
    public class FillResult
    {
        public int ZlpeRows { get; set; } = -1; // -1 = nicht ausgefuehrt
        public int RpeRows { get; set; } = -1;

        public List<string> Warnings { get; } = new List<string>();
    }
}
