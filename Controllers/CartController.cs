using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using web_quanao.Core.Interfaces;
using web_quanao.Services;

namespace web_quanao.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController()
        {
            _cartService = new CartService();
        }

        // Simple demo product catalog (hard-coded)
        private static readonly (int id, string name, decimal price)[] DemoProducts = new[]
        {
            (1, "Áo thun nam", 199000m),
            (2, "Quần jeans", 499000m),
            (3, "Áo khoác", 899000m)
        };

        // GET: /Cart
        public ActionResult Index()
        {
            var userId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null;
            var sessionId = GetSessionId();
            var cart = _cartService.GetCart(userId, sessionId) ?? _cartService.GetOrCreateCart(userId, sessionId);
            ViewBag.Total = _cartService.CalculateCartTotal(cart);
            ViewBag.DemoProducts = DemoProducts; // show add buttons
            return View(cart);
        }

        [HttpPost]
        public ActionResult Add(int productId, int quantity = 1)
        {
            var prod = System.Array.Find(DemoProducts, p => p.id == productId);
            if (prod.id == 0) return Json(new { success = false, message = "Sản phẩm không tồn tại" });
            var userId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null;
            _cartService.AddItem(userId, GetSessionId(), prod.id, prod.name, prod.price, null, quantity);
            var count = _cartService.GetItemCount(userId, GetSessionId());
            return Json(new { success = true, count });
        }

        [HttpPost]
        public ActionResult Update(int cartItemId, int quantity)
        {
            var userId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null;
            _cartService.UpdateQuantity(userId, GetSessionId(), cartItemId, quantity);
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult Remove(int cartItemId)
        {
            var userId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null;
            _cartService.RemoveItem(userId, GetSessionId(), cartItemId);
            return Json(new { success = true });
        }

        private string GetSessionId()
        {
            if (Session["SID"] == null)
            {
                Session["SID"] = Guid.NewGuid().ToString("N");
            }
            return (string)Session["SID"]; 
        }
    }
}