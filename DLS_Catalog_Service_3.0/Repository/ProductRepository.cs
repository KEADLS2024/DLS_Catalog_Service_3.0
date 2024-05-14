using DLS_Catalog_Service.Model;
using MongoDB.Driver;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DLS_Catalog_Service.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MongoDbContext _context;
        private readonly IModel _channel;

        public ProductRepository(MongoDbContext context, IConnection connection)
        {
            _context = context;
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "products", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        private void PublishProductMessage(Product product, string action)
        {
            var messageObject = new { ProductId = product.Id, Name = product.Name, Action = action };
            var messageString = JsonSerializer.Serialize(messageObject);
            var body = Encoding.UTF8.GetBytes(messageString);
            _channel.BasicPublish(exchange: "", routingKey: "products", basicProperties: null, body: body);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            if (!int.TryParse(id, out int productId))
            {
                return null;  // Optionally handle invalid id format
            }

            return await _context.Products.Find(product => product.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            PublishProductMessage(product, "Added");
            return product;
        }

        public async Task<bool> UpdateAsync(string id, Product product)
        {
            if (!int.TryParse(id, out int productId))
            {
                return false;
            }

            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            var updateResult = await _context.Products.ReplaceOneAsync(filter, product);
            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                PublishProductMessage(product, "Updated");
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!int.TryParse(id, out int productId))
            {
                return false;
            }

            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            var deleteResult = await _context.Products.DeleteOneAsync(filter);
            if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
            {
                PublishProductMessage(new Product { Id = productId }, "Deleted");
                return true;
            }
            return false;
        }
    }
}
