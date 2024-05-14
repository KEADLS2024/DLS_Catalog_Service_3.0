using DLS_Catalog_Service.Model;

namespace DLS_Catalog_Service.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(string id);
    Task<Category> CreateAsync(Category category);
    Task<bool> UpdateAsync(string id, Category category);
    Task<bool> DeleteAsync(string id);
}