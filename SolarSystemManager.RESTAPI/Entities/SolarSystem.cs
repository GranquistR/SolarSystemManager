using System.Linq;
namespace SolarSystemManager.RESTAPI.Entities
{
    public class SolarSystem
    {

        //<\summary>        
        /// <summary>
        /// Primary key of solar system for data base, generates upon creation
        /// <\summary>        
        public int solarSystemID { get; set; }

        /// <summary>
        /// Owner foreign key, retrieved from user info
        /// </summary>
        public int ownerID { get; set; }

        ///<summary>
        /// Name of solar system, prompted from user upon creation
        /// </summary>
        public string? systemName { get; set; }

        ///<summary>
        /// privacy flag, true = private, false = public
        /// Prompted from user upon creation
        /// </summary>
        public bool systemIsPrivate { get; set; }

        ///<summary>
        /// List of space objects within solar system, empty by default
        /// </summary>
        public IEnumerable<SpaceObject> spaceObjects = Enumerable.Empty<SpaceObject>();

        /// <summary>
        /// Solar system constuctor, sets up basic info like primary key, owner foreign key, name, and privacy setting
        /// </summary>
        public SolarSystem(int _solarSystemID, int _ownerID, string _systemName, bool _systemIsPrivate)
        {
            solarSystemID = _solarSystemID;
            ownerID = _ownerID;
            systemName = _systemName;
            systemIsPrivate = _systemIsPrivate;
        }

        /// <summary>
        /// Parameters are to create a new space object, creates and adds space object to list. May need to modifiy later but this works for now.
        /// </summary>
        public void addSpaceObject(int objID, string objName, string objType, int x, int y, int objSize, int objColor)
        {
            var newObject = new SpaceObject(objID, solarSystemID, objName, objType, x, y, objSize, objColor);
            //Console.WriteLine("Object to be added: " + newObject.objectName); //for testing purposes
            spaceObjects = spaceObjects.Append(newObject);
            //print current list of space objects, for testing
            /*Console.WriteLine("Current list");
            foreach(var s in spaceObjects)
            {
                Console.WriteLine(s.objectName);
            }*/

        }

        ///<summary>
        ///Removes space object from the solar system from ID
        ///</summary>
        public void removeSpaceObject(int objID)
        {
            spaceObjects = spaceObjects.Where(s => s.spaceObjectID != objID);
            //print current list of space objects, for testing
            /*Console.WriteLine("Current List");
            foreach (var s in spaceObjects)
            {
                Console.WriteLine(s.objectName);
            }*/
        }
    }
}