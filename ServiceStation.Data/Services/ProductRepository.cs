using Microsoft.EntityFrameworkCore;
using ServiceStation.Core.Shop;
using ServiceStation.Data.Paging;
using System;
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

        public Task<PagedList<Product>> GetProducts(PagingParameters pagingParameters, string sortOrder, string searchString)
        {
            var list = FindAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.Name.Contains(searchString)
                                       || s.Manufacturer.Contains(searchString));
            }

            list = sortOrder switch
            {
                "name_desc" => list.OrderByDescending(s => s.Name),
                "Manufacturer" => list.OrderBy(s => s.Manufacturer),
                "manufacturer_desc" => list.OrderByDescending(s => s.Manufacturer),
                "Category" => list.OrderBy(s => s.Category),
                "category_desc" => list.OrderByDescending(s => s.Category),
                "Price" => list.OrderBy(s => s.Price),
                "price_desc" => list.OrderByDescending(s => s.Price),
                _ => list.OrderBy(s => s.Name),
            };

            return Task.FromResult(PagedList<Product>.GetPagedList(list, pagingParameters.PageNumber, pagingParameters.PageSize));
        }
    }
}
