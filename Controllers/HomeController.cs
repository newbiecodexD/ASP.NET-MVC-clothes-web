using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_quanao.Models;

namespace web_quanao.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var products = new List<ProductViewModel>
            {
                new ProductViewModel { Id = 1, Name = "Áo Thun Cổ Tròn Ngắn Tay", Description="Vải cotton thoáng mát.", Price = 399000, ImageUrl = "https://dongphucunicorn.com/wp-content/uploads/2019/10/ao-thun-co-tron-mau-xanh.jpg", Gender = "Unisex" },
                new ProductViewModel { Id = 2, Name = "Quần Jeans Dáng Rộng", Description="Phong cách và thoải mái.", Price = 999000, ImageUrl = "https://down-vn.img.susercontent.com/file/6bbb5aca7e4caa1e29ed64fa7e6d5123", Gender = "Nam" },
                new ProductViewModel { Id = 3, Name = "Áo Sơ Mi Vải Linen", Description="Nhẹ và thấm hút tốt.", Price = 799000, ImageUrl = "https://image.uniqlo.com/UQ/ST3/AsianCommon/imagesgoods/465780/item/goods_01_465780.jpg", Gender = "Nữ" },
        
            };

            return View(products); // Truyền danh sách sản phẩm cho View
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult MenClothes()
        {
            return View();
        }
        public ActionResult WomenClothes()
        {
            return View();
        }
        public ActionResult KidsClothes()
        {
            return View();
        }
        public ActionResult Collections()
        {
            return View();
        }
    }
}