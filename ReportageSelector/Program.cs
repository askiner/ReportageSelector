using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ReportageSelector
{
    static class Program
    {

        public static List<string> Files = new List<string>();

        public static List<LibraryInfo> Libraries = new List<LibraryInfo>();

        private static TraceSource ProgramTrace = null;

        public static string PreviousReportagePrefix = null;

        public static string InfoFilePath = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {            
            // Init trace for program work
            AddTrace();

            // map special disk to upload production files
            MapProductionDrive();

            TraceMessage(TraceEventType.Verbose, "Open", string.Join(";", args));

            Files.AddRange(args);

            if (args.Length > 0)
                PreviousReportagePrefix = GetPrefix(args[0]);

            GetLibraries();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());

            TraceMessage(TraceEventType.Verbose, "Closed");
        }

        private static void GetLibraries()
        {
            if (!string.IsNullOrEmpty(Config.LibrariesXML))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(Config.LibrariesXML);

                XmlNodeList nodeList = xmldoc.DocumentElement.SelectNodes(@"//library");

                foreach (XmlNode node in nodeList)
                {
                    LibraryInfo lib_info = new LibraryInfo();

                    if (!string.IsNullOrEmpty(node.Attributes["name"].Value))
                    {
                        lib_info.Name = node.Attributes["name"].Value;
                    }

                    if (!string.IsNullOrEmpty(node.Attributes["id"].Value))
                    {
                        lib_info.Id = Convert.ToInt32(node.Attributes["id"].Value);
                    }

                    if (!string.IsNullOrEmpty(node.Attributes["picture-path"].Value))
                    {
                        lib_info.PicturesPath = node.Attributes["picture-path"].Value;
                    }

                    if (!string.IsNullOrEmpty(node.Attributes["reportage-path"].Value))
                    {
                        lib_info.ReportagePath = node.Attributes["reportage-path"].Value;
                    }

                    if (!string.IsNullOrEmpty(node.Attributes["zero-reportage-path"].Value))
                    {
                        lib_info.ReportageZeroPath = node.Attributes["zero-reportage-path"].Value;
                    }

                    if (!string.IsNullOrEmpty(node.Attributes["xml-path"].Value))
                    {
                        lib_info.XmlPath = node.Attributes["xml-path"].Value;
                    }

                    if (!string.IsNullOrEmpty(node.Attributes["period"].Value))
                    {
                        if (node.Attributes["period"].Value.Length > 0)
                        {
                            try
                            {
                                lib_info.Period = Convert.ToInt32(node.Attributes["period"].Value);
                            }
                            catch 
                            {
                                lib_info.Period = LibraryInfo.DefaultPeriod; // By Default
                            }
                        }
                        else
                            lib_info.Period = LibraryInfo.DefaultPeriod; // By Default
                    }


                    Libraries.Add(lib_info);
                }

                //var xdoc = XDocument.Load(Config.LibrariesXML);
                //var libraries = xdoc.Descendants("libraries").Select(s => (string)s).ToArray();
                //int k = libraries.Count();
            }
        }

        private static void AddTrace()
        {
            ProgramTrace = new TraceSource(Config.TraceSourceName);

            //System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            if (!Directory.Exists(Config.TraceLogPath)){
                try
                {
                    Directory.CreateDirectory(Config.TraceLogPath);
                }
                catch(Exception e)
                {
                    MessageBox.Show(
                        string.Format("Невозможно создать каталог для записи log файлов: {0}", Config.TraceLogPath));
                }
            }

            TextWriterWithTimeTraceListener listener = new TextWriterWithTimeTraceListener(
                Path.Combine(
                    Config.TraceLogPath,                                // path to logs in config file
                    string.Format(
                        "{0:yyyy-MM-dd}-{1}-{2}.log",                   // format for log file    

                        DateTime.Today,                                 // every log with date

                        string.IsNullOrEmpty(Environment.UserName) ?    // name of the user
                            "UNKNOWN" : 
                            Environment.UserName,                       

                        string.IsNullOrEmpty(Environment.MachineName) ? // name of the computer
                            "UNKNOWN" : 
                            Environment.MachineName                     
                        )),
                        Config.TraceListenerName
                        );


                ProgramTrace.Listeners.Add(listener);
        }

        private static string GetPrefixFileName(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            string infoPath = null;


            if (File.Exists(path))
            {
                FileInfo finfo = new FileInfo(path);
                if (finfo.Directory.Name == Config.TmpFolderName)
                    infoPath = Path.Combine(finfo.Directory.FullName, Config.PublicationInfoFile);
            }
            else
                if (Directory.Exists(path))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(path);

                    if (dirInfo.Name == Config.TmpFolderName)
                        infoPath = Path.Combine(path, Config.PublicationInfoFile);
                }

            return infoPath;
        }

        public static void SetPrefix(string path, string prefix)
        {
            string infoPath = GetPrefixFileName(path);

            if (!string.IsNullOrEmpty(infoPath) && !string.IsNullOrEmpty(prefix))
                File.WriteAllText(infoPath, prefix);
        }

        public static string GetPrefix(string path)
        {
            string infoPath = GetPrefixFileName(path);

            if (!string.IsNullOrEmpty(infoPath) && File.Exists(infoPath))
            {
                string[] content = File.ReadAllLines(infoPath);
                if (content.Length > 0)
                    return content.FirstOrDefault(str => !string.IsNullOrEmpty(str));
                else
                    return null;
            }

            return null;
        }


        /// <summary>
        /// Форматирование выводимой в логи информации
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventId"></param>
        /// <param name="args"></param>
        public static void TraceMessage(TraceEventType type, params object[] args)
        {
            //object[] newArgs = new object[args.Length + 1];
            //newArgs[0] = DateTime.UtcNow;

            //for (int i = 0; i < args.Length; ++i)
            //    newArgs[i + 1] = args[i];

            //ProgramTrace.TraceData(type, eventId, newArgs);

            ProgramTrace.TraceData(type, 0, args);
        }

        public static void MapProductionDrive()
        {
            if (string.IsNullOrEmpty(Config.AttachDiskLetter))
            {
                TraceMessage(TraceEventType.Critical, "Не указан параметр: буква для подцепляемого диска - AttachDiskLetter");
            }

            if (string.IsNullOrEmpty(Config.AttachPath))
            {
                TraceMessage(TraceEventType.Critical, "Не указан параметр: путь для подключения диска - AttachPath");
            }

            DriveSettings.MapNetworkDrive(Config.AttachDiskLetter, Config.AttachPath, Config.AttachUserName, Config.AttachPassword);
        }
    }
}
