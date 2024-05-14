using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DLS_Catalog_Service.Model
{
    public class ProductDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("productId")]
        [Required(ErrorMessage = "Product ID is required.")]
        public string ProductId { get; set; }

        [BsonElement("description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [BsonElement("weight")]
        [Range(0.1, 1000.0, ErrorMessage = "Weight must be between 0.1 and 1000.0 kilograms.")]
        public double Weight { get; set; }

        [BsonElement("dimensions")]
        [StringLength(200, ErrorMessage = "Dimensions cannot exceed 200 characters.")]
        public string Dimensions { get; set; }
    }
}