using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace web_quanao.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private bool IsAdmin() => Session["IsAdmin"] as string == "true";

        // Simple in-memory product store (static for demo)
        private static readonly List<ProductVm> _products = new List<ProductVm>
        {
            new ProductVm{ Id=1, Name="Áo thun demo", Price=199000, Stock=10 },
            new ProductVm{ Id=2, Name="Qu?n jean demo", Price=499000, Stock=5 }
        };

        public class ProductVm
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
        }

        public ActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            return View(_products.OrderBy(p => p.Id));
        }

        public ActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVm model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            if (!ModelState.IsValid) return View(model);
            model.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(model);
            TempData["Msg"] = "?ã thêm s?n ph?m";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            var p = _products.FirstOrDefault(x => x.Id == id);
            if (p == null) return HttpNotFound();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductVm model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            if (!ModelState.IsValid) return View(model);
            var p = _products.FirstOrDefault(x => x.Id == model.Id);
            if (p == null) return HttpNotFound();
            p.Name = model.Name; p.Price = model.Price; p.Stock = model.Stock;
            TempData["Msg"] = "?ã c?p nh?t";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            var p = _products.FirstOrDefault(x => x.Id == id);
            if (p == null) return HttpNotFound();
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            var p = _products.FirstOrDefault(x => x.Id == id);
            if (p != null) _products.Remove(p);
            TempData["Msg"] = "?ã xóa";
            return RedirectToAction("Index");
        }
    }
}
