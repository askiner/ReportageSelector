using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace ReportageSelector
{
    public interface IProductionMethod
    {
        bool Produce(string file, string prefix);
    }

    public abstract class ProductionMethod : IProductionMethod
    {
        public abstract bool Produce(string file, string prefix);

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
                    Arguments = "-IPTC:Caption-Abstract \"" + file + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();

                if (line != null)
                    if (line.Contains("*** Local Caption *** "))
                        return line.Substring(line.IndexOf(" *** Local Caption *** ") + " *** Local Caption *** ".Length, line.Length - (line.IndexOf(" *** Local Caption *** ") + " *** Local Caption *** ".Length));
                // do something with line
            }

            return null;
        }

        protected Metadata UpdateMetadata(string file)
        {
            string metadataUpdate = "-IPTC:ReleaseDate=now -IPTC:ReleaseTime=now";
            string fixtureIdentifier = Guid.NewGuid().ToString().Replace("-", "");

            Metadata NewMetadata = new Metadata();
            NewMetadata.FixedIdentifier = fixtureIdentifier;

            string Caption = this.GetCaption(file);
            if (Caption != null)
            {
                NewMetadata.Caption = Caption;
                metadataUpdate = metadataUpdate + " -IPTC:Caption-Abstract=\"" + Caption + "\"";
            }

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Config.Exiftool,
                    Arguments = metadataUpdate + " -charset Cyrillic -overwrite_original \"" + file + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();

                if (line.Contains("1 image files updated"))
                    return NewMetadata;
            }

            return null;
        }


        protected bool CreateXML(Metadata data, string destination)
        {
            XmlDocument doc = new XmlDocument();

            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

            //Encoding win1251 = Encoding.GetEncoding("windows-1251");
            //Encoding utf8 = Encoding.UTF8;

            //byte[] win1251Bytes = win1251.GetBytes(data.Caption);
            //byte[] utf8bytes = Encoding.Convert(win1251, utf8, win1251Bytes);

            //// Convert the new byte[] into a char[] and then into a string.
            //char[] utf8Chars = new char[utf8.GetCharCount(utf8bytes, 0, utf8bytes.Length)];
            //utf8.GetChars(utf8bytes, 0, utf8bytes.Length, utf8Chars, 0);
            //string CaptionUTF8String = new string(utf8Chars);
            XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement("assets"));
            //root.AppendChild(doc.CreateElement("captionweb")).InnerText = CaptionUTF8String;
            root.AppendChild(doc.CreateElement("captionweb")).InnerText = data.Caption;
            root.AppendChild(doc.CreateElement("fixident")).InnerText = data.FixedIdentifier;
            doc.Save(destination);
            return true;
        }

    }

    public class VisibleReportageMethod : ProductionMethod
    {
        public override bool Produce(string file, string prefix)
        {
            Metadata data = this.UpdateMetadata(file);

            if (Directory.Exists(Config.VisibleReportageFolder))
                if (prefix != null && prefix.Length > 0)
                    File.Move(file, Path.Combine(Config.VisibleReportageFolder, prefix + Config.ReportageDelimeter + Path.GetFileName(file)));
                else
                    File.Move(file, Path.Combine(Config.VisibleReportageFolder, Path.GetFileName(file)));

            this.CreateXML(data, Path.Combine(Config.XMLFolder, Path.GetFileNameWithoutExtension(file) + ".xml"));

            return true;
        }
    }

    public class HiddenReportageMethod : ProductionMethod
    {
        public override bool Produce(string file, string prefix)
        {
            throw new NotImplementedException();
        }
    }

    public class NoReportageMethod : ProductionMethod
    {
        public override bool Produce(string file, string prefix)
        {
            throw new NotImplementedException();
            
        }
    }
}
