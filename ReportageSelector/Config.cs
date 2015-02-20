﻿using System;
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

        public static string ReportageDelimeter
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportageDelimeter"].ToString();
            }
        }

        public static string XMLFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["XMLFolder"].ToString();
            }
        }

        public static string Exiftool
        {
            get
            {
                return ConfigurationManager.AppSettings["Exiftool"].ToString();
            }
        }
    }
}