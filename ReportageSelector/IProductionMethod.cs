using System;
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
        bool CheckOutputFolder();
    }

    public static class ExiftoolMetadataFieldName
    {
        public static string Caption { get { return "IPTC:Caption-Abstract"; } }
        public static string Keywords { get { return "IPTC:Keywords"; } }
        public static string Unused_1 { get { return "IPTC:IPTC_ApplicationRecord_1"; } } // special use for contains keywords
    }

    public class ProductionMethod : IProductionMethod
    {
        private const string REPORTAGEDELIMETER = "_";

        public string OutputFolder { get; set; }
        public string XMLFolder { get; set; }
        public string Prefix { get; set; }
        public string ReportageDelimeter { get; set; }

        protected Dictionary<string, string> fileMetadata = null;

        public ProductionMethod(string jpgOutput, string xmlOutput, string prefix)
        {
            this.OutputFolder = jpgOutput;
            this.XMLFolder = xmlOutput;
            this.Prefix = prefix;
            this.ReportageDelimeter = REPORTAGEDELIMETER;
        }

        public bool Produce(string file, string prefix)
        {
            if (!Directory.Exists(OutputFolder))
                MessageBox.Show(string.Format("Нет подключения к каталогу выпуска: {0}", OutputFolder));

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

            if (XMLFolder !=null && Directory.Exists(XMLFolder))
                this.CreateXML(data, Path.Combine(XMLFolder, Path.GetFileNameWithoutExtension(file) + ".xml"));

            //Program.TraceMessage(System.Diagnostics.TraceEventType.Information, "")

            return true;
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
                File.Move(file, DestinationFileName);
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

        protected Dictionary<string, string> GetMetadata(string file)
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

                    Dictionary<string, string> result = new Dictionary<string, string>();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Xmltext);

                    //TODO: Merge data in IPTC:IPTC_ApplicationRecord_1 & IPTC:Keywords
                    if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Caption).Count > 0)
                    {
                        string caption = xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Caption).Item(0).InnerText;

                        if (caption.Contains("*** Local Caption *** "))
                            result.Add(ExiftoolMetadataFieldName.Caption, caption.Substring(caption.IndexOf(" *** Local Caption *** ") + " *** Local Caption *** ".Length, caption.Length - (caption.IndexOf(" *** Local Caption *** ") + " *** Local Caption *** ".Length)));
                        else
                            result.Add(ExiftoolMetadataFieldName.Caption, caption);
                    }

                    //TODO: read keywords from standart place and add them to dictionary Keywords - list
                    if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Keywords).Count > 0)
                    {

                    }

                    //TODO: read keywords from Unused #1 and add them to dictionary Keywords - string
                    if (xmlDoc.GetElementsByTagName(ExiftoolMetadataFieldName.Unused_1).Count > 0)
                    {

                    }

                    return result;
                }
            }

            return null;
        }

        protected Metadata UpdateMetadata(string file)
        {
            string metadataUpdate = "-IPTC:ReleaseDate=now -IPTC:ReleaseTime=now -IPTC:DateCreated<DateTimeOriginal -IPTC:TimeCreated<DateTimeOriginal";
            string fixtureIdentifier = Guid.NewGuid().ToString().Replace("-", "");

            Metadata NewMetadata = new Metadata();
            NewMetadata.FixedIdentifier = fixtureIdentifier;

            if (!string.IsNullOrEmpty(NewMetadata.FixedIdentifier))
            {
                metadataUpdate = metadataUpdate + " -IPTC:FixtureIdentifier=\"" + NewMetadata.FixedIdentifier + "\"";
            }


            Dictionary<string, string> fileMetadata = GetMetadata(file);

            if (fileMetadata != null)
            {
                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.Caption))
                {
                    //NewMetadata.Caption = Caption;
                    NewMetadata.Caption = this.ConvertDate(fileMetadata[ExiftoolMetadataFieldName.Caption], true);
                    //metadataUpdate = metadataUpdate + " -IPTC:Caption-Abstract=\"" + this.ConvertDate(Caption) + "\"";
                    metadataUpdate = metadataUpdate + " -IPTC:Caption-Abstract=\"" + this.ConvertDate(fileMetadata[ExiftoolMetadataFieldName.Caption], false) + "\"";
                }


                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.Keywords))
                {

                }

                if (fileMetadata.ContainsKey(ExiftoolMetadataFieldName.Unused_1))
                {

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
                    Arguments = metadataUpdate + " -charset Latin -overwrite_original \"" + file + "\"",
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

            XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement("assets"));

            root.AppendChild(doc.CreateElement("captionweb")).InnerText = data.Caption;
            root.AppendChild(doc.CreateElement("fixident")).InnerText = data.FixedIdentifier;
            doc.Save(destination);
            return true;
        }

    }
}
