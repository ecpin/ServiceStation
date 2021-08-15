using Microsoft.AspNetCore.Mvc;
using ServiceStation.Core.Shop;
using ServiceStation.Data.Paging;
using ServiceStation.Data.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceStation.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ShopController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Index([FromQuery] PagingParameters pagingParameters)
        {
            return View(await _productRepository.GetProducts(pagingParameters));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = _productRepository.GetProduct(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }
    }
}
