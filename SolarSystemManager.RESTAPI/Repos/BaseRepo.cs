﻿using SolarSystemManager.RESTAPI.Entities;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography.Xml;
using static SolarSystemManager.RESTAPI.Entities.SolarSystem;
using static System.Data.Entity.Infrastructure.Design.Executor;
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
                    sqlite_cmd.CommandText = "SELECT UserID, Username, Password, Role, Salt FROM User";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {

                        users.Add(new User
                        {
                            userID = sqlite_datareader.GetInt32(0),
                            username = sqlite_datareader.GetString(1),
                            password = sqlite_datareader.GetString(2),
                            role = (Role)sqlite_datareader.GetInt32(3),
                            salt = sqlite_datareader.GetString(4),
                        });
                    }
                    sqlite_datareader.Close();
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
                    sqlite_cmd.CommandText = "INSERT INTO User (Username, Password, Role, Salt) VALUES (@username, @password, @role, @salt);";
                    sqlite_cmd.Parameters.AddWithValue("@username", newUser.username);
                    sqlite_cmd.Parameters.AddWithValue("@password", newUser.password);
                    sqlite_cmd.Parameters.AddWithValue("@role", (int)newUser.role);
                    sqlite_cmd.Parameters.AddWithValue("@salt", newUser.salt);
                    sqlite_cmd.ExecuteNonQuery();
                    return;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }
        public void ModifyUser(User newUserData)
        {

            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "UPDATE User SET Username = \'" + newUserData.username + "\', Password = \'" + newUserData.password + "\' WHERE UserID = " + newUserData.userID + ";";
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
                        solarSystems.Add(new SolarSystem(sqlite_datareader.GetInt32(0), 
                                                         sqlite_datareader.GetInt32(1), 
                                                         sqlite_datareader.GetString(2), 
                                                         (Visibility)sqlite_datareader.GetInt32(3)));
                    }
                    sqlite_datareader.Close();
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT SOID, SSID, Name, Type, LocationX, LocationY, Size, Color  FROM SpaceObject";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
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

                    foreach (var solarSystem in solarSystems)
                    {
                        solarSystem.spaceObjects = spaceObjects.FindAll(s => s.solarSystemID == solarSystem.systemId);
                    }
                    sqlite_datareader.Close();
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
                    sqlite_cmd.CommandText = "DELETE FROM SpaceObject WHERE SSID=" + targetID + ";";
                    sqlite_cmd.ExecuteNonQuery();
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
        public bool DeleteSpaceObject(int targetID)
        {
            lock (countLock)
            {
                try
                {
                    //Console.WriteLine("DeleteSpaceObject started");
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "DELETE FROM SpaceObject WHERE SOID=" + targetID + ";";
                    sqlite_cmd.ExecuteNonQuery();
                    //Console.WriteLine("DeleteSpaceObject ended");
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
                    sqlite_datareader.Close();
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
                    sqlite_datareader.Close();
                    dummySolarSystem.spaceObjects = spaceObjects;
                    return dummySolarSystem;
                }
                catch
                {
                    throw new BadHttpRequestException("Solar System not found for ID: " + targetID);
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }

        public int AddSolarSystem(SolarSystem newSystem)
        {
            lock (countLock)
            {
                try
                {
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "INSERT INTO SolarSystem (OwnerID, Name, Visibility) VALUES (@ownerID, @name, @visibility);";
                    sqlite_cmd.Parameters.AddWithValue("@ownerID", newSystem.ownerId);
                    sqlite_cmd.Parameters.AddWithValue("@name", newSystem.systemName);
                    sqlite_cmd.Parameters.AddWithValue("@visibility", (int)newSystem.systemVisibility);
                    sqlite_cmd.ExecuteNonQuery();

                    sqlite_cmd.CommandText = "SELECT last_insert_rowid();";
                    int newId = Convert.ToInt32(sqlite_cmd.ExecuteScalar());

                    return newId;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }   

        #endregion


        #region SpaceObjectTable

        public int AddSpaceObject(SpaceObject spaceObject)
        {
            lock (countLock)
            {
                try
                {
                    //Console.WriteLine("Add space object started");
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "INSERT INTO SpaceObject (SSID, Name, Type, LocationX, LocationY, Size, Color) VALUES " +
                        "(@ssid, @name, @type, @xCoord, @yCoord, @size, @color);";
                    sqlite_cmd.Parameters.AddWithValue("@ssid", spaceObject.solarSystemID);
                    sqlite_cmd.Parameters.AddWithValue("@name", spaceObject.objectName);
                    sqlite_cmd.Parameters.AddWithValue("@type", spaceObject.objectType);
                    sqlite_cmd.Parameters.AddWithValue("@xCoord", spaceObject.xCoord);
                    sqlite_cmd.Parameters.AddWithValue("@yCoord", spaceObject.yCoord);
                    sqlite_cmd.Parameters.AddWithValue("@size", spaceObject.objectSize);
                    sqlite_cmd.Parameters.AddWithValue("@color", spaceObject.objectColor);
                    sqlite_cmd.ExecuteNonQuery();
                    //Console.WriteLine("Add space object completed");

                    sqlite_cmd.CommandText = "SELECT last_insert_rowid();";
                    int newId = Convert.ToInt32(sqlite_cmd.ExecuteScalar());

                    return newId;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }
        
        public bool RemoveSpaceObject(int targetID)
        {
            lock (countLock)
            {
                try
                {
                    Console.WriteLine("RemoveSpaceObject started");
                    sqlite_conn.Open();
                    SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "DELETE FROM SpaceObject WHERE SOID=" + targetID + ";";
                    sqlite_cmd.ExecuteNonQuery();
                    Console.WriteLine("RemoveSpaceObject ended");
                    return true;
                }
                finally
                {
                    sqlite_conn.Close();
                }
            }
        }

        public SpaceObject GetSpaceOBjectById(int spaceObjectId)
        {
            lock (countLock)
            {
                try
                {


                    SQLiteDataReader sqlite_datareader;
                    SQLiteCommand sqlite_cmd;
                    sqlite_conn.Open();

                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT SOID, SSID, Name, Type, LocationX, LocationY, Size, Color  FROM SpaceObject WHERE SOID=" + spaceObjectId + ";";
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
                    sqlite_datareader.Close();
                    return spaceObjects[0];
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
                    sqlite_datareader.Close();
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