using SolarSystemManager.RESTAPI.Entities;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text.Json;
using System.IO;

namespace SolarSystemManager.RESTAPI.Services
{
    public class SQLService 
    {
        // Relative path to data base file
        SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory.Replace("RESTAPI", "DATABASE\\SpaceBox.db") + ";Version=3;Compress=True;");
        
        public List<User> GetUsers()
        {
            try
            {
                sqlite_conn.Open();
                List<User> users = new List<User>();
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT UserID,Username,Password,Role FROM User";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    users.Add(new User
                    {
                        username = sqlite_datareader.GetString(1),
                        password = sqlite_datareader.GetString(2),
                        role = sqlite_datareader.GetString(3),
                    });            
                }
                sqlite_conn.Close();
                    return users;
            }
            catch
            {
                throw new BadHttpRequestException("Bad SQL Request!");
            }
        }

       

    }
}

