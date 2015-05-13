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
    }

    public class ProductionMethod : IProductionMethod
    {
        private const string REPORTAGEDELIMETER = "_";

        public string OutputFolder { get; set; }
        public string XMLFolder { get; set; }
        public string Prefix { get; set; }
        public string ReportageDelimeter { get; set; }

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

            if (OutputFolder != null && Directory.Exists(OutputFolder))
                MoveFileWithPrefix(file, prefix);

            if (XMLFolder !=null && Directory.Exists(XMLFolder))
                this.CreateXML(data, Path.Combine(XMLFolder, Path.GetFileNameWithoutExtension(file) + ".xml"));

            //Program.TraceMessage(System.Diagnostics.TraceEventType.Information, "")

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

        protected string GetCaption(string file)
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Config.Exiftool,
                    Arguments = "-X -IPTC:All -charset Latin \"" + file + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
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
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(Xmltext);

                    if (xmlDoc.GetElementsByTagName("IPTC:Caption-Abstract").Count > 0)
                    {
                        string caption = xmlDoc.GetElementsByTagName("IPTC:Caption-Abstract").Item(0).InnerText;

                        if (caption.Contains("*** Local Caption *** "))
                            return caption.Substring(caption.IndexOf(" *** Local Caption *** ") + " *** Local Caption *** ".Length, caption.Length - (caption.IndexOf(" *** Local Caption *** ") + " *** Local Caption *** ".Length));
                        else
                            return caption;
                    }

                }
            }

            return null;
        }

        protected Metadata UpdateMetadata(string file)
        {
            string metadataUpdate = "-IPTC:ReleaseDate=now -IPTC:ReleaseTime=now -IPTC:DateCreatedDateTimeOriginal -IPTC:TimeCreated<DateTimeOriginal";
            string fixtureIdentifier = Guid.NewGuid().ToString().Replace("-", "");

            Metadata NewMetadata = new Metadata();
            NewMetadata.FixedIdentifier = fixtureIdentifier;

            if (!string.IsNullOrEmpty(NewMetadata.FixedIdentifier))
            {
                metadataUpdate = metadataUpdate + " -IPTC:FixtureIdentifier=\"" + NewMetadata.FixedIdentifier + "\"";
            }

            string Caption = this.GetCaption(file);
            if (Caption != null)
            {
                //NewMetadata.Caption = Caption;
                NewMetadata.Caption = this.ConvertDate(Caption, true);
                //metadataUpdate = metadataUpdate + " -IPTC:Caption-Abstract=\"" + this.ConvertDate(Caption) + "\"";
                metadataUpdate = metadataUpdate + " -IPTC:Caption-Abstract=\"" + this.ConvertDate(Caption, false) + "\"";
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
