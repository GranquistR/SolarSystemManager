using System.Linq;
namespace SolarSystemManager.RESTAPI.Entities
{
/*   public class Object
    {
        /// <summary>
        /// The id of the object
        /// </summary>
        private int id { get; set; }
        /// <summary>
        /// Name of the object
        /// </summary>
        private string name { get; set; }

        /// <summary>
        /// Optional parameter
        /// </summary>
        private string? optionalParam { get; set; }
    }
*/

    public class SolarSystem
    {
        //<summary>
        //solar system object, contains primary key, owner foreign key
        //name, and privacy bool
        //<\summary>
        private int solarID { get; set; }
        /// <summary>
        /// Primary key of solar system for data base
        /// <\summary>
        private int ownerID { get; set; }
        /// <summary>
        /// owner foreign key
        /// </summary>
        private string? name { get; set; }
        ///<summary>
        /// name of solar system
        /// </summary>
        private bool privacy { get; set; }
        ///<summary>
        /// privacy flag, true = private, false = public
        /// </summary>
        private IEnumerable<SpaceObject>? spaceObjects { get; set; }
        ///<summary>
        /// list of space objects within solar system
        /// </summary>
    }

    public class SpaceObject
    {
        /// <summary>
        /// Object in space such as stars, planets, etc.
        /// </summary>
        private int spaceID { get; set; }
        /// <summary>
        /// primary key in database
        /// </summary>
        private int solarID { get; set; }
        /// <summary>
        /// solar system foreign key
        /// </summary>
        private string? name { get; set; }
        ///<summary>
        ///name of solar object
        /// </summary>
        private string? type { get; set; }
        ///<summary>
        ///type of object (star, planet, etc.)
        ///</summary>
        private int? xCoord { get; set; }
        private int? yCoord { get; set; }
        ///<summary>
        /// x and y coords
        /// </summary>
        private int? size {  get; set; }
        ///<summary>
        /// size of space object
        /// </summary>
        private int? color {  get; set; }
       ///<summary>
       /// hexadecimal code for color
       /// </summary>
    }
}