﻿using System.Linq;
using System.Text.Json.Serialization;
using SolarSystemManager.RESTAPI.Repos;

namespace SolarSystemManager.RESTAPI.Entities
{
    public class SolarSystem
    {

        //<\summary>        
        /// <summary>
        /// Primary key of solar system for data base, generates upon creation
        /// <\summary>        
        public int systemId { get; set; }

        /// <summary>
        /// Owner foreign key, retrieved from user info
        /// </summary>
        public int ownerId { get; set; }

        ///<summary>
        /// Name of solar system, prompted from user upon creation
        /// </summary>
        public string? systemName { get; set; }

        ///<summary>
        /// privacy flag, true = private, false = public
        /// Prompted from user upon creation
        /// </summary>
        public Visibility systemVisibility { get; set; }

        ///<summary>
        /// List of space objects within solar system, empty by default
        /// </summary>
        public IEnumerable<SpaceObject> spaceObjects { get; set;}

        /// <summary>
        /// Solar system constuctor, sets up basic info like primary key, owner foreign key, name, and privacy setting
        /// </summary>
        public SolarSystem(int _solarSystemID, int _ownerID, string _systemName, Visibility _systemVisibility)
        {
            systemId = _solarSystemID;
            ownerId = _ownerID;
            systemName = _systemName;
            systemVisibility = _systemVisibility;
        }

        /// <summary>
        /// Parameters are to create a new space object, creates and adds space object to list. May need to modifiy later but this works for now.
        /// </summary>
        public void addSpaceObject(int objID, string objName, string objType, int x, int y, int objSize, string objColor)
        {
            var newObject = new SpaceObject(objID, systemId, objName, objType, x, y, objSize, objColor);
            spaceObjects = spaceObjects.Append(newObject);
        }

        ///<summary>
        ///Removes space object from the solar system from ID
        ///</summary>
        public void removeSpaceObject(int objID)
        {
            spaceObjects = spaceObjects.Where(s => s.spaceObjectID != objID);
        }
    }

    public class NewSolarSystemRequest
    {
       public NewSolarSystem solarSystem { get; set; }

        public LoginRequest credentials { get; set; }
    }

    public class NewSolarSystem
    {
        public string systemName { get; set; }
        public Visibility systemVisibility { get; set; }
    }   

    public enum Visibility
    {
        Public,
        Private
    }

    public class DleteSolarSystemRequest
    {
        [JsonPropertyName("username")]
        public String username { get; set; }

        [JsonPropertyName("password")]
        public String password { get; set; }

        [JsonPropertyName("dSolarSystemID")]
        public int dSolarSystemID { get; set; }

        public DleteSolarSystemRequest(String username, String password, int dSolarSystemID)
        {
            this.username = username;
            this.password = password;
            this.dSolarSystemID = dSolarSystemID;
        }
    }
}