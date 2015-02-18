using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace ReportageSelector
{
    public interface IReportage
    {
        int Id { get; set; }
        string Reference { get; set; }
        DateTime Date { get; set; }        
        string Name { get; set; }
        string ImageUrl { get; set; }
        string DisplayName { get; }
    }
    public class Reportage : IReportage
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }        
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string DisplayName
        {
            get
            {
                return Date.ToString("dd.MM.yyyy") + " " + Name;
            }
        }

        public Reportage()
        {
        }

        public Reportage(int id, string reference, DateTime date, string name, string imageUrl)
        {
            Id = id;
            Reference = reference;
            Date = date;            
            Name = name;
            ImageUrl = imageUrl;
        }
    }

    public interface IReportageRepository
    {
        List<IReportage> GetReportageList();
    }

    public class ReportageRepository : IReportageRepository
    {
        public List<IReportage> GetReportageList()
        {
            List<IReportage> newList = new List<IReportage>();

            OleDbConnection connection = new OleDbConnection(Config.ConnectionString);
            {
                connection.Open();
                OleDbCommand Command = new OleDbCommand(Config.ReportageQuery(), connection);
                using(OleDbDataReader reader = Command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        newList.Add(new Reportage(
                            Convert.ToInt32(reader[0]),
                            reader[1].ToString(),
                            Convert.ToDateTime(reader[2]),
                            reader[3].ToString(),
                            reader[4].ToString()
                            ));
                    }
                }
                
            }

            return newList;
        }
    }
}
