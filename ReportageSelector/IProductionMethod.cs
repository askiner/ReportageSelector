﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Forms;

namespace ReportageSelector
{
    public interface IProductionMethod
    {
        bool Produce(string file, string prefix);
        string OutputFolder { get; set; }
        string XMLFolder { get; set; }
        string Prefix { get; set; }
        string ReportageDelimeter { get; set; }
        string KeywordsSeparator { get; set; }
        bool CheckOutputFolder();
    }

    public static class ExiftoolMetadataFieldName
    {
        public static string Caption { get { return "IPTC:Caption-Abstract"; } }
        public static string Keywords { get { return "IPTC:Keywords"; } }
        public static string Unused_1 { get { return "IPTC:IPTC_ApplicationRecord_1"; } } // special use for contains keywords
        public static string Unused_2 { get { return "IPTC:IPTC_ApplicationRecord_2"; } } // special use for contains credit
        public static string RDF_Bag { get { return "rdf:Bag"; } } // bag )container for list in exiftool XML format
        public static string RDF_LI { get { return "rdf:li"; } } // list item in exiftool XML format
        public static string CountryCode { get { return "IPTC:Country-PrimaryLocationCode"; } } // list item in exiftool XML format
        public static string CountryName { get { return "IPTC:Country-PrimaryLocationName"; } } // list item in exiftool XML format
        public static string CountryNameEn { get { return "Custom:Country-PrimaryLocationNameEn"; } } // list item in exiftool XML format
        public static string Source { get { return "IPTC:Source"; } }
    }

    public static class OrpheaXMLInputField
    {
        public static string Root { get { return "assets"; } }
        public static string ObjectId { get { return "id_objet"; } }
        public static string CaptionRus { get { return "captionweb"; } }
        public static string FixtureIdentifier { get { return "fixident"; } }
        public static string Keywords { get { return "keyword"; } }
        public static string CountryRus { get { return "country"; } }
        public static string CountryEng { get { return "country_en"; } }
        public static string CountryCode { get { return "countrycode"; } }
        public static string Source { get { return "source"; } }
    }

    public class ProductionMethod : IProductionMethod
    {
        private const string REPORTAGEDELIMETER = "_";
        private string _keywordsSeparator = ",";

        public string OutputFolder { get; set; }
        public string XMLFolder { get; set; }
        public string Prefix { get; set; }
        public string ReportageDelimeter { get; set; }
        public string KeywordsSeparator { get { return _keywordsSeparator; } set { _keywordsSeparator = value; } }

        protected Dictionary<string, string> fileMetadata = null;

        public ProductionMethod(string jpgOutput, string xmlOutput, string prefix)
        {
            this.OutputFolder = jpgOutput;
            this.XMLFolder = xmlOutput;
            this.Prefix = prefix;
            this.ReportageDelimeter = REPORTAGEDELIMETER;
        }

        public bool Produce(string origFile, string prefix)
        {            
            if (!Directory.Exists(OutputFolder))
                MessageBox.Show(string.Format("Нет подключения к каталогу выпуска: {0}", OutputFolder));

            using (TempFile tmpFile = new TempFile(origFile, Config.TempPath))
            {
                if (tmpFile.Error) {
                    MessageBox.Show(
                        string.Format("При копировании файла во временный каталог произошли ошибки:\n{0}", tmpFile.ErrorMessage),
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }

                string file = tmpFile.TempFileName;

                Metadata data = this.UpdateMetadata(file);

                if (data == null)
                {
                    //MessageBox.Show(string.Format("Ошибка при отправке файла {0}. Переименуйте файл и попробуйте еще раз.", file));
                    return false;
                }

#if !DEBUG
            if (OutputFolder != null && Directory.Exists(OutputFolder))
                MoveFileWithPrefix(file, prefix);
#endif

                if (XMLFolder != null && Directory.Exists(XMLFolder))
                    this.CreateXML(data, Path.Combine(XMLFolder, Path.GetFileNameWithoutExtension(file) + ".xml"));

                //Program.TraceMessage(System.Diagnostics.TraceEventType.Information, "")

                return true;
            }
        }

        public bool CheckOutputFolder()
        {
            if (!Directory.Exists(OutputFolder))
            {
                MessageBox.Show(string.Format("Нет подключения к каталогу выпуска: {0}", OutputFolder));
                return false;
            }
            else
                return true;
        }

        private void MoveFileWithPrefix(string file, string prefix)
        {
            string DestinationFileName = null;

            if (prefix != null && prefix.Length > 0)
                DestinationFileName = Path.Combine(OutputFolder, prefix + ReportageDelimeter + Path.GetFileName(file));
            else
                DestinationFileName = Path.Combine(OutputFolder, Path.GetFileName(file));

            if (File.Exists(DestinationFileName))            
                File.Delete(DestinationFileName);

            try
            {
                // File.Move(file, DestinationFileName);
                File.Copy(file, DestinationFileName);
                //File.Delete(file);
            }
            catch
            {
                
            }
        }

        protected string GetNameWithoutPrefix(string file, string removePrefix)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file");

            if (removePrefix == null || removePrefix.Length == 0)
                throw new ArgumentNullException("removePrefix");

            FileInfo fInfo = new FileInfo(file);

            if (fInfo.Exists)
            {
                if (fInfo.Name.StartsWith(removePrefix) && fInfo.Name.Length > removePrefix.Length)
                    return fInfo.Name.Substring(removePrefix.Length, fInfo.Name.Length - removePrefix.Length);
                    
            }

            return fInfo.Name;
        }

        protected Dictionary<string, dynamic> GetMetadata(string file)
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Config.Exiftool,
                    Arguments = "-X -IPTC:All -u -charset Latin \"" + file + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.GetEncoding("Windows-1251")
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                //string line = proc.StandardOutput.ReadLine();
                string Xmltext = proc.StandardOutput.ReadToEnd();

                if (Xmltext != null)
                {

                    Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Xmltext);

                    if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Caption).Count > 0)
                    {
                        string caption = xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Caption).Item(0).InnerText;

                        if (caption.Contains("*** Local Caption *** "))
                            result.Add(ExiftoolMetadataFieldName.Caption, caption.Substring(caption.IndexOf(" *** Local Caption *** ") + " *** Local Caption *** ".Length, caption.Length - (caption.IndexOf(" *** Local Caption *** ") + " *** Local Caption *** ".Length)));
                        else
                            result.Add(ExiftoolMetadataFieldName.Caption, caption);
                    }

                    List<string> keywords = new List<string>();

                    // read keywords from standart field and add them to temporary field
                    if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Keywords).Count > 0)
                    {

                        XmlNode XmlKeywordsNode = xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Keywords).Item(0);

                        if (XmlKeywordsNode.HasChildNodes)
                        {
                            foreach (XmlNode mBagNode in XmlKeywordsNode.ChildNodes)
                            {
                                if (mBagNode.Name == ExiftoolMetadataFieldName.RDF_Bag)
                                {
                                    if (mBagNode.HasChildNodes)
                                    {
                                        foreach (XmlNode liNode in mBagNode.ChildNodes)
                                        {
                                            if (liNode.InnerText.Trim().Length > 2 && !keywords.Contains(liNode.InnerText.Trim()))
                                                keywords.Add(liNode.InnerText.Trim());
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    //read keywords from Unused #1 and add them to dictionary Keywords - string
                    if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Unused_1).Count > 0)
                    {
                        // this field contains a string, with list of keywords delimeters with "," or ";"
                        string AdditionalKeywords = xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Unused_1).Item(0).InnerText;

                        if (!string.IsNullOrEmpty(AdditionalKeywords))
                        {
                            foreach (string keyItem in AdditionalKeywords.Replace(',', ';').Trim().Split(';'))
                            {
                                if (!string.IsNullOrEmpty(keyItem.Trim()) && !keywords.Contains(keyItem.Trim()))
                                    keywords.Add(keyItem.Trim());
                            }
                        }
                    }

                    // if list of keywords collected - add them into object of collected metadata
                    if (keywords.Count > 0)
                    {
                        result[ExiftoolMetadataFieldName.Keywords] = keywords;
                    }

                    // countries - prepare to load to XML
                    if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.CountryName).Count > 0) // if we get countries only
                    {
                        string countryName = xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.CountryName).Item(0).InnerText;

                        string countryCode = null;

                        if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.CountryCode).Count > 0)
                        {
                            countryCode = xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.CountryCode).Item(0).InnerText;
                        }

                        // if country is not empty and code is null or empty, than try to find in dictionary
                        if (!string.IsNullOrEmpty(countryName) && string.IsNullOrEmpty(countryCode))
                        {
                            if (CountryUtility.GetCountryByName(countryName) != null)
                            {
                                result.Add(ExiftoolMetadataFieldName.CountryCode, CountryUtility.GetCountryByName(countryName).Code);
                                result.Add(ExiftoolMetadataFieldName.CountryName, CountryUtility.GetCountryByName(countryName).NameRu);
                                result.Add(ExiftoolMetadataFieldName.CountryNameEn, CountryUtility.GetCountryByName(countryName).NameEn);
                            }
                        }
                    }

                    // set default Source if empty
                    if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Source).Count > 0)
                    {
                        string source = xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Source).Item(0).InnerText;

                        if (source != null)
                            result.Add(ExiftoolMetadataFieldName.Source, source);
                        else
                            result.Add(ExiftoolMetadataFieldName.Source, Config.DefaultMetadataSource);
                    }
                    else
                        result.Add(ExiftoolMetadataFieldName.Source, Config.DefaultMetadataSource);

                    return result;
                }
            }

            return null;
        }

        protected Metadata UpdateMetadata(string file)
        {
            RemoveReadonlyFlag(file);

            string metadataUpdate = "-IPTC:ReleaseDate=now -IPTC:ReleaseTime=now -IPTC:DateCreated<ModifyDate -IPTC:DateCreated<CreateDate -IPTC:DateCreated<DateTimeOriginal -IPTC:TimeCreated<DateTimeOriginal";
            string fixtureIdentifier = Guid.NewGuid().ToString().Replace("-", "");

            Metadata NewMetadata = new Metadata();
            NewMetadata.FixedIdentifier = fixtureIdentifier;

            if (!string.IsNullOrEmpty(NewMetadata.FixedIdentifier))
            {
                metadataUpdate = metadataUpdate + " -IPTC:FixtureIdentifier=\"" + NewMetadata.FixedIdentifier + "\"";
            }


            Dictionary<string, dynamic> fileMetadata = GetMetadata(file);            

            if (fileMetadata != null)
            {
                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.Caption))
                {
                    //NewMetadata.Caption = Caption;
                    NewMetadata.Caption = this.ConvertDate(fileMetadata[ExiftoolMetadataFieldName.Caption], true);
                    //metadataUpdate = metadataUpdate + " -IPTC:Caption-Abstract=\"" + this.ConvertDate(Caption) + "\"";
                    metadataUpdate = metadataUpdate + " -IPTC:Caption-Abstract=\"" + this.ConvertDate(fileMetadata[ExiftoolMetadataFieldName.Caption], false) + "\"";
                }

                // add source to update
                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.Source))
                {
                    NewMetadata.Source = fileMetadata[ExiftoolMetadataFieldName.Source];
                    metadataUpdate = metadataUpdate + " -IPTC:Source=\"" + fileMetadata[ExiftoolMetadataFieldName.Source] + "\"";
                }

                // rdf:Bag, rdf:li
                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.Keywords) && fileMetadata[ExiftoolMetadataFieldName.Keywords] != null)
                {
                    NewMetadata.Keywords = fileMetadata[ExiftoolMetadataFieldName.Keywords];
                }

                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.CountryCode))
                {
                    NewMetadata.CountryCode = fileMetadata[ExiftoolMetadataFieldName.CountryCode];
                }

                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.CountryName))
                {
                    NewMetadata.CountryName = fileMetadata[ExiftoolMetadataFieldName.CountryName];
                }

                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.CountryNameEn))
                {
                    NewMetadata.CountryNameEn = fileMetadata[ExiftoolMetadataFieldName.CountryNameEn];
                }
            }


            // remove Fotostaion steps
            metadataUpdate = metadataUpdate + " -Fotostation:All= -IPTC:DocumentHistory=";

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Config.Exiftool,
                    //Arguments = metadataUpdate + " -charset UTF8 -overwrite_original \"" + file + "\"",
                    Arguments = metadataUpdate + " -charset Latin -overwrite_original -m \"" + file + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.GetEncoding("Windows-1251")
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();

                if (line.Contains("1 image files updated"))
                {
                    //RemoveIPTCDuplicate(file);
                    return NewMetadata; 

                }
                    
            }

            return null;
        }

        private void RemoveIPTCDuplicate(string file){
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Config.Exiftool,
                    //Arguments = metadataUpdate + " -charset UTF8 -overwrite_original \"" + file + "\"",
                    Arguments = "-Fotostion:All= -overwrite_original \"" + file + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.GetEncoding("Windows-1251")
                }

            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();

                if (line.Contains("1 image files updated"))
                {
                    return;

                }

            }
        }

        private string ConvertDate(string text, bool forXML)
        {
            Regex reg = new Regex(@"(\d{2}\.\d{2}\.\d{4})", RegexOptions.CultureInvariant);

            try
            {
                if (reg.IsMatch(text))
                {
                    DateTime date = DateTime.MinValue;
                    string dateString = reg.Match(text).Groups[0].Captures[0].Value;

                    if (!string.IsNullOrEmpty(dateString)){
                        date = Convert.ToDateTime(dateString);

                        //return text.Replace(dateString, date.ToString("dd MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture)).Replace("\"", "\\\"");

                        if (forXML)
                            return text.Replace(dateString, date.ToString("d MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture));
                        else
                            return text.Replace(dateString, date.ToString("d MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture)).Replace("\"", "\\\"");
                    }
                }
            }
            catch
            {

            }

            if (forXML)
                return text;
            else
                return text.Replace("\"", "\\\"");

            //return text.Replace("\"", "\\\"");
            //return text;
        }
        


        protected bool CreateXML(Metadata data, string destination)
        {
            XmlDocument doc = new XmlDocument();

            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement(OrpheaXMLInputField.Root));

            root.AppendChild(doc.CreateElement(OrpheaXMLInputField.CaptionRus)).InnerText = data.Caption;
            root.AppendChild(doc.CreateElement(OrpheaXMLInputField.FixtureIdentifier)).InnerText = data.FixedIdentifier;

            // keywords
            if (data.Keywords != null && data.Keywords.Count > 0)
            {
                data.Keywords.Sort();
                root.AppendChild(doc.CreateElement(OrpheaXMLInputField.Keywords)).InnerText = string.Join(this.KeywordsSeparator, data.Keywords.ToArray());
            }

            // countries related data
            if (!string.IsNullOrEmpty(data.CountryCode))
            {
                root.AppendChild(doc.CreateElement(OrpheaXMLInputField.CountryCode)).InnerText = data.CountryCode;
            }

            if (!string.IsNullOrEmpty(data.CountryName))
            {
                root.AppendChild(doc.CreateElement(OrpheaXMLInputField.CountryRus)).InnerText = data.CountryName;
            }

            if (!string.IsNullOrEmpty(data.Source))
            {
                root.AppendChild(doc.CreateElement(OrpheaXMLInputField.Source)).InnerText = data.Source;
            }

            // Commented to prevent save English keywords into russian newswires
            //if (!string.IsNullOrEmpty(data.CountryNameEn))
            //{
            //    root.AppendChild(doc.CreateElement(OrpheaXMLInputField.CountryEng)).InnerText = data.CountryNameEn;
            //}

            doc.Save(destination);
            return true;
        }

        public void RemoveReadonlyFlag(string file)
        {
            FileAttributes attributes = File.GetAttributes(file);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                File.SetAttributes(file, attributes & ~FileAttributes.ReadOnly);
        }

    }
}
