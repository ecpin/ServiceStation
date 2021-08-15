using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceStation.Core.Shop;
using ServiceStation.Data.Paging;
using ServiceStation.Data.Services;

namespace ServiceStation.Controllers
{
    [Authorize(Roles = "ProductManager")]
    public class ProductManagerController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductManagerController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var param = new PagingParameters();
            return View(_productRepository.GetProducts(param));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.CreateProduct(product);
                return RedirectToAction("Details", new { id = product.Id });
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _productRepository.GetProduct(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                return RedirectToAction("Details", new { id = product.Id });
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _productRepository.GetProduct(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection form)
        {
            var model = _productRepository.GetProduct(id);
            _productRepository.Delete(model);

            return RedirectToAction("Index");
        }

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