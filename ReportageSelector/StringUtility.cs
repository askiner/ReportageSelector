using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportageSelector
{
    public static class StringUtility
    {
        public static string EncodeCodepage(string strToEncode, Encoding sourceEncoding, Encoding destinationEncoding)
        {
            byte[] originalByteString = sourceEncoding.GetBytes(strToEncode);
            byte[] convertedByteString = Encoding.Convert(sourceEncoding,
            destinationEncoding, originalByteString);
            return destinationEncoding.GetString(convertedByteString);
        }
    }
}
