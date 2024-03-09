using SolarSystemManager.RESTAPI.Entities;
using System.Data;
using System.Data.SQLite;
using static SolarSystemManager.RESTAPI.Entities.SolarSystem;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolarSystemManager.RESTAPI.Repos
{
    public class BaseRepo
    {
        // Singlton class to handle SQL calls and data return
        // static classes are inharintly thread safe
        private static BaseRepo _instance;
        private SQLiteConnection sqlite_conn;
        private static object countLock = new object();
        private BaseRepo() 
        {
            sqlite_conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory.Replace("RESTAPI", "DATABASE\\SpaceBox.db") + ";Version=3;Compress=True;");
        }
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
        // //This is an example of how to get data from the database
        //public List<SomeDataType> GetSomeData()
        //{
        //    lock (countLock)
        //    {
        //        try
        //        {
        //            sqlite_conn.Open();
        //            List<UsSomeDataType> data = new List<SomeDataType>();
        //            SQLiteDataReader sqlite_datareader;
        //            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
        //            sqlite_cmd.CommandText = "SELECT all collum data requests sepperated by commas FROM Table Name";
        //            sqlite_datareader = sqlite_cmd.ExecuteReader();
        //            while (sqlite_datareader.Read())
        //            {
        //                data.Add(new SomeDataType
        //                {
        //                    Parameter1 = sqlite_datareader.GetInt32(0),
        //                    Parameter2 = sqlite_datareader.GetString(1),
        //                    // IMPORTANT, the string value 0 will be the first request in the SQL string
        //                });
        //            }
        //            return data;
        //        }
        //        finally
        //        {
        //            sqlite_conn.Close();
        //        }
        //    }
        //}
        #endregion

        #region UserTable
        public List<User> GetAllUsers()
        {

            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();
                    var users = new List<User>();
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
                    return users;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }

        public void CreateUser(User newUser)
        {

            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "INSERT INTO User (Username, Password, Role) VALUES (@username, @password, @role);";
                    sqlite_cmd.Parameters.AddWithValue("@username", newUser.username);
                    sqlite_cmd.Parameters.AddWithValue("@password", newUser.password);
                    sqlite_cmd.Parameters.AddWithValue("@role", (int)newUser.role);
                    sqlite_cmd.ExecuteNonQuery();
                    return;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }

        }

        #endregion

        #region SolarSystemTable

        public List<SolarSystem> GetAllSolarSystems()
        {
            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();

                    var solarSystems = new List<SolarSystem>();
                    var spaceObjects = new List<SpaceObject>();

                    SQLiteDataReader sqlite_datareader;
                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT SSID, OwnerID, Name, Visibility from SolarSystem";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        solarSystems.Add(new SolarSystem(sqlite_datareader.GetInt32(0), sqlite_datareader.GetInt32(1), sqlite_datareader.GetString(2), (Visibility)sqlite_datareader.GetInt32(3)));
                    }

                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT SOID, SSID, Name, Type, LocationX, LocationY, Size, Color  FROM SpaceObject";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        spaceObjects.Add(new SpaceObject(sqlite_datareader.GetInt32(0), sqlite_datareader.GetInt32(1), sqlite_datareader.GetString(2), sqlite_datareader.GetString(3), sqlite_datareader.GetInt32(4), sqlite_datareader.GetInt32(5), sqlite_datareader.GetInt32(6), sqlite_datareader.GetString(7)));
                    }

                    foreach (var solarSystem in solarSystems)
                    {
                        solarSystem.spaceObjects = spaceObjects.FindAll(s => s.solarSystemID == solarSystem.systemId);
                    }
                    return solarSystems;
                }
                finally
                {
                    sqlite_conn.Close();
                }

            }
        }

        public bool DeleteSolarSystem(int targetID)
        {
            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "DELETE FROM SolarSystem WHERE SSID=" + targetID + ";";
                    sqlite_cmd.ExecuteNonQuery();
                    return true;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }

        public SolarSystem GetSolarSystemByID(int targetID)
        {
            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();

                    SQLiteDataReader sqlite_datareader;
                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT SSID, OwnerID, Name, Visibility from SolarSystem WHERE SSID=" + targetID + ";";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    sqlite_datareader.Read();
                    SolarSystem dummySolarSystem = new SolarSystem(sqlite_datareader.GetInt32(0), 
                                                                    sqlite_datareader.GetInt32(1), 
                                                                    sqlite_datareader.GetString(2), 
                                                                    (Visibility)sqlite_datareader.GetInt32(3));

                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT SOID, SSID, Name, Type, LocationX, LocationY, Size, Color  FROM SpaceObject WHERE SSID=" + targetID + ";";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                  

                    var spaceObjects = new List<SpaceObject>();
                    while (sqlite_datareader.Read())
                    {
                        spaceObjects.Add(new SpaceObject(sqlite_datareader.GetInt32(0), 
                                                        sqlite_datareader.GetInt32(1), 
                                                        sqlite_datareader.GetString(2), 
                                                        sqlite_datareader.GetString(3), 
                                                        sqlite_datareader.GetInt32(4), 
                                                        sqlite_datareader.GetInt32(5), 
                                                        sqlite_datareader.GetInt32(6), 
                                                        sqlite_datareader.GetString(7)));
                    }
                    dummySolarSystem.spaceObjects = spaceObjects;
                    return dummySolarSystem;
                }
                finally
                {
                    sqlite_conn.Close();
                }

            }
        }

        #endregion


        #region SpaceObjectTable

        public int AddSpaceObject(int size) //currently, adds a space object of specified size to the solar system alpha centauri
                                            //one parameter is for debugging purposes
        {
            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "INSERT INTO SpaceObject (SSID, Name, Type, LocationX, LocationY, Size, Color) VALUES " +
                        "(@ssid, @name, @type, @xCoord, @yCoord, @size, @color);";
                    sqlite_cmd.Parameters.AddWithValue("@ssid", 22);
                    sqlite_cmd.Parameters.AddWithValue("@name", "Earth");
                    sqlite_cmd.Parameters.AddWithValue("@type", "planet");
                    sqlite_cmd.Parameters.AddWithValue("@xCoord", 000);
                    sqlite_cmd.Parameters.AddWithValue("@yCoord", 000);
                    sqlite_cmd.Parameters.AddWithValue("@size", size);
                    sqlite_cmd.Parameters.AddWithValue("@color", "#5DE2E7");
                    sqlite_cmd.ExecuteNonQuery();
                    return 0; //success
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }

        public int RemoveSpaceObjectByID(int targetId) //removes space object by ID
        {
            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "DELETE FROM SpaceObject WHERE SOID = " + targetId + ";";
                    sqlite_cmd.ExecuteNonQuery();
                    return 0; //success
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }

        #endregion

        #region DynamicSQL

        public int Count(string tableName)
        {
            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();
                    int rowcount = 0;
                    SQLiteDataReader sqlite_datareader;
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT COUNT(*) FROM " + tableName;
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        rowcount = sqlite_datareader.GetInt32(0);
                    }
                    return rowcount;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }

        }


        #endregion
    }
}