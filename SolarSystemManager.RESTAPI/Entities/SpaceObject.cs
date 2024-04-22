using System.Text.Json.Serialization;
using SolarSystemManager.RESTAPI.Repos;

namespace SolarSystemManager.RESTAPI.Entities
{
    public class SpaceObject
    {
        /// <summary>
        /// Primary key of the space object within the database
        /// </summary>
        [JsonPropertyName("spaceObjectID")]
        public int spaceObjectID { get; set; }
        /// <summary>
        /// Solar system foreign key, pulled from solar system when space object is created
        /// </summary>
        [JsonPropertyName("solarSystemID")]
        public int solarSystemID { get; set; }
        ///<summary>
        /// Name of the space object
        /// </summary>
        [JsonPropertyName("objectName")]
        public string? objectName { get; set; }
        ///<summary>
        /// Type of object, star or planet
        ///</summary>
        [JsonPropertyName("objectType")]
        public string objectType { get; set; }
        /// <summary>
        /// X coordinate of space object in solar system
        /// </summary>
        [JsonPropertyName("xCoord")]
        public int xCoord { get; set; }
        /// <summary>
        /// Y coordinate of space object in solar system
        /// </summary>
        [JsonPropertyName("yCoord")]
        public int yCoord { get; set; }
        ///<summary>
        /// Size of space object (will we add realistic units?)
        /// </summary> 
        [JsonPropertyName("objectSize")]
        public int objectSize { get; set; }
        ///<summary>
        /// Object color as hexidecimal code
        /// </summary>
        [JsonPropertyName("objectColor")]
        public string objectColor { get; set; }
        ///<summary>
        /// Constructor for space object, may add default values for some things like size, color
        /// spaceObjectID, solarSystemID are generated and not accessible by end user.
        /// Name, type, and coords may be prompted upon creation (depends on UI design)
        /// </summary>
        public SpaceObject(int spaceObjectID, int solarSystemID, string objectName,
            string objectType, int xCoord, int yCoord, int objectSize, string objectColor)
        {
            this.spaceObjectID = spaceObjectID;
            this.solarSystemID = solarSystemID;
            this.objectName = objectName;
            this.objectType = objectType;
            this.xCoord = xCoord;
            this.yCoord = yCoord;
            this.objectSize = objectSize;
            this.objectColor = objectColor;
        }

    }

    public class AddSpaceObjectRequest
    {
        public SpaceObject spaceObject { get; set; }

        public LoginRequest credentials { get; set; }

    }
}
