using Microsoft.AspNetCore.Mvc;
using ServiceStation.Core.Shop;
using ServiceStation.Data.Paging;
using ServiceStation.Data.Services;
using System;
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
        public async Task<ActionResult<PagedList<Product>>> Index([FromQuery] PagingParameters pagingParameters, string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ManufacturerSortParm"] = sortOrder == "Manufacturer" ? "manufacturer_desc" : "Manufacturer";
            ViewData["CategorySortParm"] = sortOrder == "Category" ? "category_desc" : "Category";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;


            return View(await _productRepository.GetProducts(pagingParameters, sortOrder, searchString));
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
