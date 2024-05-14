using DLS_Catalog_Service.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Catalog> Catalog => _database.GetCollection<Catalog>("Catalog");
    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Category");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Product");
    public IMongoCollection<ProductDetail> ProductDetails => _database.GetCollection<ProductDetail>("ProductDetail");
}