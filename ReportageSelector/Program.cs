using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ReportageSelector
{
    static class Program
    {

        public static List<string> Files = new List<string>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Files.AddRange(args);

            if (args.Length > 0)
                MessageBox.Show(args.Length + " - " + args[0]);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
