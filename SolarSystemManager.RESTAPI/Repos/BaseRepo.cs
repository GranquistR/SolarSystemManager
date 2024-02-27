using SolarSystemManager.RESTAPI.Entities;
using System.Data.SQLite;

namespace SolarSystemManager.RESTAPI.Repos
{
    public class BaseRepo
    {
        // Singlton class to handle SQL calls and data return
        private static BaseRepo _instance;
        private static readonly object _lock = new object();
        private SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory.Replace("RESTAPI", "DATABASE\\SpaceBox.db") + ";Version=3;Compress=True;");
        private BaseRepo() { }
        public static BaseRepo Instance()
        {
            if (_instance == null) {
                // prevents multiple instances os BaseRepo
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BaseRepo();
                    }
                }
            }
            return _instance;
        }

        public List<Generic> GetData(string request)
        {
            List<Generic> data = new List<Generic>();

            if(request.Contains("User"))
            {
                sqlite_conn.Open();
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = request;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    data.Add(new User
                    {
                        username = sqlite_datareader.GetString(1),
                        password = sqlite_datareader.GetString(2),
                        role = sqlite_datareader.GetString(3),
                    });
                }
                sqlite_conn.Close();
                return data;
            }
            return data;
        }

    }
}
