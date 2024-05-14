using DLS_Catalog_Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DLS_Catalog_Service.Repositories
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<Catalog>> GetAllCatalogsAsync();
        Task<Catalog> GetCatalogByIdAsync(string id);  
        Task<Catalog> CreateCatalogAsync(Catalog catalog);
        Task<bool> UpdateCatalogAsync(string id, Catalog catalog);
        Task<bool> DeleteCatalogAsync(string id);
    }
}
