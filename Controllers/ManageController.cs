using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace web_quanao.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Email = User?.Identity?.Name;
            return View();
        }
    }
}