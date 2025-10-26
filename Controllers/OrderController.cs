using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace web_quanao.Controllers
{
    public class OrderController : Controller
    {
        // L?ch s? mua hàng: hi?n th? t? LocalStorage b?ng JS (server ch? render khung)
        public ActionResult History()
        {
            return View();
        }
    }
}
