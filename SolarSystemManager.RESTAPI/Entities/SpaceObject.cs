using SolarSystemManager.RESTAPI.Repos;

namespace SolarSystemManager.RESTAPI.Entities
{
    public class SpaceObject : Generic
    {
        /// <summary>
        /// Primary key of the space object within the database
        /// </summary>
        public int spaceObjectID { get; set; }
        /// <summary>
        /// Solar system foreign key, pulled from solar system when space object is created
        /// </summary>
        public int solarSystemID { get; set; }
        ///<summary>
        /// Name of the space object
        /// </summary>
        public string? objectName { get; set; }
        ///<summary>
        /// Type of object
        ///</summary>
        public string? objectType { get; set; }
        /// <summary>
        /// X coordinate of space object in solar system
        /// </summary>
        public int? xCoord { get; set; }
        /// <summary>
        /// Y coordinate of space object in solar system
        /// </summary>
        public int? yCoord { get; set; }
        ///<summary>
        /// Size of space object (will we add realistic units?)
        /// </summary> 
        public int? objectSize { get; set; }
        ///<summary>
        /// Object color as hexidecimal code
        /// </summary>
        public int? objectColor { get; set; }
        ///<summary>
        /// Constructor for space object, may add default values for some things like size, color
        /// spaceObjectID, solarSystemID are generated and not accessible by end user.
        /// Name, type, and coords may be prompted upon creation (depends on UI design)
        /// </summary>
        public SpaceObject(int _spaceObjectID, int _solarSystemID, string _objectName,
            string _objectType, int _xCoord, int _yCoord, int _objectSize, int _objectColor)
        {
            spaceObjectID = _spaceObjectID;
            solarSystemID = _solarSystemID;
            objectName = _objectName;
            objectType = _objectType;
            xCoord = _xCoord;
            yCoord = _yCoord;
            objectSize = _objectSize;
            objectColor = _objectColor;
        }

    }
}
