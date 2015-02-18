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

        public static int LibraryId 
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LibraryId"]);
            }
        }

        public static int LastDaysCount 
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LastDaysCount"]);
            }
        }

        public static string ReportageQuery()
        {
            return String.Format(ReportageQueryTemplate, Config.LibraryId, Config.LastDaysCount);
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
                return ConfigurationManager.ConnectionStrings["PreviewTemplate"].ToString();
            }
        }

        public static string VisibleReportageFolder
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["VisibleReportageFolder"].ToString();
            }
        }

        public static string HiddenReportageFolder
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["HiddenReportageFolder"].ToString();
            }
        }

        public static string NoReportageFolder
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NoReportageFolder"].ToString();
            }
        }

        public static string ReportageDelimeter
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ReportageDelimeter"].ToString();
            }
        }

        public static string TempFilePrefix
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["TempFilePrefix"].ToString();
            }
        }

    }
}
