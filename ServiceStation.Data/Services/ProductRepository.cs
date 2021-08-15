using Microsoft.EntityFrameworkCore;
using ServiceStation.Core.Shop;
using ServiceStation.Data.Paging;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStation.Data.Services
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDBContext)
            : base(appDBContext)
        {

        }

        public Product GetProduct(int id)
        {
            return FindByCondition(p => p.Id == id).FirstOrDefault();
        }

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void UpdateProduct(Product product)
        {
            Update(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }

        public Task<PagedList<Product>> GetProducts(PagingParameters pagingParameters)
        {
            return Task.FromResult(PagedList<Product>.GetPagedList(FindAll().OrderBy(p => p.Id), pagingParameters.PageNumber, pagingParameters.PageSize));
        }
    }
}
