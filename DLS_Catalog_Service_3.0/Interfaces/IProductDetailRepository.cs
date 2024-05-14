using DLS_Catalog_Service.Model;

namespace DLS_Catalog_Service.Repositories;

public interface IProductDetailRepository
{
    Task<IEnumerable<ProductDetail>> GetAllAsync();
    Task<ProductDetail> GetByIdAsync(string id);
    Task<ProductDetail> CreateAsync(ProductDetail productDetail);
    Task<bool> UpdateAsync(string id, ProductDetail productDetail);
    Task<bool> DeleteAsync(string id);
}