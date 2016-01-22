using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ReportageSelector
{
    public abstract class Config
    {
        public static string ConnectionString 
        { 
            get
            {
                return ConfigurationManager.ConnectionStrings["Orphea"].ToString();
            }  
        }

        public static string LibrariesXML
        {
            get { return ConfigurationManager.AppSettings["LibariesXML"].ToString(); }
        }



        public static KeyValuePair<string, int> LibraryId_Lenta 
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LibraryId_Lenta"]))
                {
                    if (ConfigurationManager.AppSettings["LibraryId_Lenta"].IndexOf(";") > 0)
                    {
                        string[] parts = ConfigurationManager.AppSettings["LibraryId_Lenta"].Split(';');
                        return new KeyValuePair<string, int>(parts[0], Convert.ToInt32(parts[1]));
                    }
                        
                }

                return new KeyValuePair<string, int>("Лента ТАСС", 9);
            }
        }

        public static KeyValuePair<string, int> LibraryId_Stock
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LibraryId_Stock"]))
                {
                    if (ConfigurationManager.AppSettings["LibraryId_Stock"].IndexOf(";") > 0)
                    {
                        string[] parts = ConfigurationManager.AppSettings["LibraryId_Stock"].Split(';');
                        return new KeyValuePair<string, int>(parts[0], Convert.ToInt32(parts[1]));
                    }

                }

                return new KeyValuePair<string, int>("Архив ленты ТАСС", 30);
            }
        }

        public static int LibraryId
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LibraryId"]);
            }
        }

        public static string ReportageQuery(int libraryId, int period)
        {
            return String.Format(ReportageQueryTemplate, libraryId, period);
        }

        public static string ReportageQueryTemplate
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportageQuery"];
            }
        }

        public static string PreviewUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PreviewTemplate"].ToString();
            }
        }

        public static string VisibleReportageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["VisibleReportageFolder"].ToString();
            }
        }

        public static string HiddenReportageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["HiddenReportageFolder"].ToString();
            }
        }

        public static string NoReportageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["NoReportageFolder"].ToString();
            }
        }

        public static string StockVisibleReportageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["StockVisibleReportageFolder"].ToString();
            }
        }

        public static string StockHiddenReportageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["StockHiddenReportageFolder"].ToString();
            }
        }

        public static string StockNoReportageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["StockNoReportageFolder"].ToString();
            }
        }

        public static string ReportageDelimeter
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportageDelimeter"].ToString();
            }
        }

        public static string KeywordsSeparator
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("KeywordsSeparator"))
                    return ConfigurationManager.AppSettings["KeywordsSeparator"].ToString();
                else
                    return null;
            }
        }

        public static string XMLFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["XMLFolder"].ToString();
            }
        }

        public static string StockXMLFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["StockXMLFolder"].ToString();
            }
        }

        public static string Exiftool
        {
            get
            {
                return ConfigurationManager.AppSettings["Exiftool"].ToString();
            }
        }

        public static string TraceSourceName { get { return ConfigurationManager.AppSettings["TraceSourceName"].ToString(); } }
        public static string TraceLogPath { get { return ConfigurationManager.AppSettings["TraceLogPath"].ToString(); } }
        public static string TraceListenerName { get { return ConfigurationManager.AppSettings["TraceListenerName"].ToString(); } }
        public static string TmpFolderName { get { return ConfigurationManager.AppSettings["TmpFolderName"].ToString(); } }

        public static string PublicationInfoFile { get { return ConfigurationManager.AppSettings["PublicationInfoFile"].ToString(); } }

        public static string AttachDiskLetter { get { return ConfigurationManager.AppSettings["AttachDiskLetter"].ToString(); } }
        public static string AttachPath { get { return ConfigurationManager.AppSettings["AttachPath"].ToString(); } }
        public static string AttachUserName 
        { 
            get 
            {
                if (ConfigurationManager.AppSettings["AttachUserName"] != null)
                    return ConfigurationManager.AppSettings["AttachUserName"].ToString();
                else
                    return null;
            } 
        }
        public static string AttachPassword 
        { 
            get 
            {
                if (ConfigurationManager.AppSettings["AttachPassword"] != null)
                    return ConfigurationManager.AppSettings["AttachPassword"].ToString();
                else
                    return null;
            }
        }
    }
}
