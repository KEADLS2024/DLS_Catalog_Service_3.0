using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DLS_Catalog_Service.Model
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(150, ErrorMessage = "Product name cannot exceed 150 characters.")]
        public string Name { get; set; }

        [BsonElement("categoryId")]
        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }

        [BsonElement("price")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00.")]
        public double Price { get; set; }

        [BsonElement("available")]
        public bool Available { get; set; }
    }
}