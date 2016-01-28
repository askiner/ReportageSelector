//using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.IO;

namespace ReportageSelector
{
    public static class CountryUtility
    {
        private static Dictionary<string, Country> countries = null;

        static CountryUtility() 
        {
            countries = new Dictionary<string, Country>();
        }

        public static void LoadDictionary(string filename)
        {
            if (File.Exists(filename))
            {                
                foreach (string line in File.ReadAllLines(filename))
                {
                    if (!string.IsNullOrEmpty(line.Trim()))
                    {
                        try
                        {
                            Country country = GetCountryFromLine(line);

                            if (!countries.ContainsKey(country.NameRu))
                                countries.Add(country.NameRu, country);
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        public static Country GetCountryByName(string nameInRussian)
        {
            if (HasCountriesList && countries.ContainsKey(nameInRussian))
                return countries[nameInRussian];
            else
                return null;
        }

        public static bool HasCountriesList
        {
            get
            {
                return countries != null && countries.Count() > 0;
            }
        }

        private static Country GetCountryFromLine(string line)
        {
            string[] parts = line.Split(';');

            if (parts.Length >= 3)
                return new Country(parts[0].Trim(), parts[1].Trim(), parts[2].Trim());

            throw new System.IndexOutOfRangeException("String for country is short");
        }
    }
}
