using DLS_Catalog_Service.Model;
using MongoDB.Driver;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DLS_Catalog_Service.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MongoDbContext _context;
        private readonly IModel _channel;  // RabbitMQ channel for publishing messages

        public CategoryRepository(MongoDbContext context, IConnection connection)
        {
            _context = context;
            _channel = connection.CreateModel(); // Create a channel per instance, or manage lifecycle elsewhere

            // Ensure the queue exists when the repository is instantiated
            _channel.QueueDeclare(queue: "categories",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        private void PublishCategoryMessage(Category category, string action)
        {
            var messageObject = new
            {
                CategoryId = category.Id,
                Name = category.Name,
                Action = action  // "Added", "Updated", "Deleted"
            };

            var messageString = JsonSerializer.Serialize(messageObject);
            var body = Encoding.UTF8.GetBytes(messageString);

            _channel.BasicPublish(exchange: "",
                routingKey: "categories",
                basicProperties: null,
                body: body);
        }

        // Get all categories that are not logically deleted
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var filter = Builders<Category>.Filter.Eq(c => c.IsDeleted, false);
            return await _context.Categories.Find(filter).ToListAsync();
        }

        // Get a specific category by id, only if it is not logically deleted
        public async Task<Category> GetByIdAsync(string id)
        {
            if (!int.TryParse(id, out int categoryId))
            {
                return null;  // Optionally handle invalid id format
            }

            var filter = Builders<Category>.Filter.And(
                Builders<Category>.Filter.Eq(c => c.Id, categoryId),
                Builders<Category>.Filter.Eq(c => c.IsDeleted, false)
            );
            return await _context.Categories.Find(filter).FirstOrDefaultAsync();
        }

        // Create a new category and publish an "Added" message
        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.InsertOneAsync(category);
            PublishCategoryMessage(category, "Added");
            return category;
        }

        // Update an existing category and publish an "Updated" message
        public async Task<bool> UpdateAsync(string id, Category category)
        {
            if (!int.TryParse(id, out int categoryId))
            {
                return false;
            }

            // Ensuring that IsDeleted is not changed through the normal update process
            var updateDefinition = Builders<Category>.Update
                .Set(c => c.Name, category.Name)
                .Set(c => c.CatalogId, category.CatalogId);

            var updateResult = await _context.Categories.UpdateOneAsync(
                g => g.Id == categoryId && !g.IsDeleted,
                updateDefinition);

            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                PublishCategoryMessage(category, "Updated");
                return true;
            }
            return false;
        }

        // Logically delete a category by setting the IsDeleted flag to true and publish a "Deleted" message
        public async Task<bool> DeleteAsync(string id)
        {
            if (!int.TryParse(id, out int categoryId))
            {
                return false;
            }
            var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
            var update = Builders<Category>.Update.Set(c => c.IsDeleted, true);

            var updateResult = await _context.Categories.UpdateOneAsync(filter, update);
            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                PublishCategoryMessage(new Category { Id = categoryId, IsDeleted = true }, "Deleted");
                return true;
            }
            return false;
        }
    }
}
