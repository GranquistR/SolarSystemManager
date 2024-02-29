using SolarSystemManager.RESTAPI.Entities;
using System.Data.SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolarSystemManager.RESTAPI.Repos
{
    public class BaseRepo
    {
        // Singlton class to handle SQL calls and data return
        // static classes are inharintly thread safe
        private static BaseRepo _instance;
        private SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory.Replace("RESTAPI", "DATABASE\\SpaceBox.db") + ";Version=3;Compress=True;");
        private BaseRepo() { }
        public static BaseRepo Instance()
        {
            if (_instance == null) {
               
                if (_instance == null)
                {
                    _instance = new BaseRepo();
                }     
            }
            return _instance;
        }

        #region Example
        // example of data retrieval to object
        //public List<SomeDataType> GetSomeData()
        //{
        //    List<UsSomeDataType> data = new List<SomeDataType>();

        //    sqlite_conn.Open();
        //    SQLiteDataReader sqlite_datareader;
        //    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
        //    sqlite_cmd.CommandText = "SELECT all collum data requests sepperated by commas FROM Table Name";
        //    sqlite_datareader = sqlite_cmd.ExecuteReader();
        //    while (sqlite_datareader.Read())
        //    {
        //        data.Add(new SomeDataType
        //        {
        //            Parameter1 = sqlite_datareader.GetInt32(0),
        //            Parameter2 = sqlite_datareader.GetString(1),
        //            // IMPORTANT, the string value 0 will be the first request in the SQL string
        //        });
        //    }
        //    sqlite_conn.Close();
        //    return data;
        //}
        #endregion

        #region UserTable
        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            sqlite_conn.Open();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT UserID, Username, Password, Role FROM User";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                users.Add(new User
                {
                    userID = sqlite_datareader.GetInt32(0),
                    username = sqlite_datareader.GetString(1),
                    password = sqlite_datareader.GetString(2),
                    role = (Role)sqlite_datareader.GetInt32(3),
                });
            }
            sqlite_conn.Close();
            return users;
        }

        public void CreateUser(User newUser)
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO User (Username, Password, Role) VALUES (@username, @password, @role);";
            sqlite_cmd.Parameters.AddWithValue("@username", newUser.username);
            sqlite_cmd.Parameters.AddWithValue("@password", newUser.password);
            sqlite_cmd.Parameters.AddWithValue("@role", (int)newUser.role);
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        #endregion

        #region SolarSystemTable
        public bool DeleteSolarSystem(int targetID)
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM SolarSystem WHERE SSID=" + targetID + ";";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
            return true;
        }
        #endregion
    }
}