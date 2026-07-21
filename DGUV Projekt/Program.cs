using System;
using System.Net;
using System.Windows.Forms;

namespace DGUV_Projekt
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Unter .NET Framework sicherstellen, dass TLS 1.2 fuer die
            // HTTPS-Aufrufe an die Siemens-API aktiviert ist.
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
