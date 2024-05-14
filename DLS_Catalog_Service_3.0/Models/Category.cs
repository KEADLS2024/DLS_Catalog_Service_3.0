using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DLS_Catalog_Service.Model
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        private const string ValidNamePattern = @"^[a-zA-Z0-9\s\-_]*$";

        [BsonElement("name")]
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        [RegularExpression(ValidNamePattern, ErrorMessage = "Category name contains invalid characters.")]
        public string Name { get; set; }

        [BsonElement("catalogId")]
        [Required(ErrorMessage = "Catalog ID is required.")]
        public int CatalogId { get; set; }

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;  // Tombstone flag
    }
}