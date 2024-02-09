namespace SolarSystemManager.RESTAPI.Entities
{
    public class Object
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
        private string? optionalParam{ get; set; }
    }
}
