using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DLS_Catalog_Service.Model
{
    public class Catalog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "Catalog name is required.")]
        [StringLength(100, ErrorMessage = "Catalog name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [BsonElement("year")]
        [Range(1900, 9999, ErrorMessage = "Year must be between 1900 and 9999.")]
        public int Year { get; set; }
    }
}