using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceStation.Core.Auth;
using ServiceStation.Core.Shop;
using ServiceStation.Data.Services;
using ServiceStation.ViewModels;

namespace ServiceStation.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Checkout()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;


            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("UserManagement", _userManager.Users);

            var vm = new OrderViewModel() { Email = user.Email, UserName = user.UserName, Address = user.Address };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some products first");
            }

            if (ModelState.IsValid)
            {
                order.User = await _userManager.GetUserAsync(HttpContext.User);
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return await Checkout();
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order!";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var orders = _orderRepository.GetOrders(user.Id);

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var order = _orderRepository.GetOrder(id);

            var isOwner = user.Id == order.User.Id;

            if (!isOwner)
            {
                return View("AccessDenied");
            }

            var orderDetails = _orderRepository.GetOrderDetails(id);

            var model = new OrderDetailsViewModel
            {
                UserName = user.UserName,
                Address = order.Address,
                Email = user.Email,
                OrderDetails = orderDetails,
                OrderPlaced = order.OrderPlaced,
                OrderTotal = order.OrderTotal
            };

            return View(model);
        }
    }
}
