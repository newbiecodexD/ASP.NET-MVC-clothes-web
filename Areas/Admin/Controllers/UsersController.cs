using System.Linq;
using System.Web.Mvc;
using web_quanao.Services;

namespace web_quanao.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private bool IsAdmin() => Session["IsAdmin"] as string == "true";

        public ActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            var users = InMemoryAuthStore.GetAll();
            return View(users);
        }

        [HttpPost]
        public ActionResult Delete(string email)
        {
            if (!IsAdmin()) return new HttpUnauthorizedResult();
            if (!string.IsNullOrWhiteSpace(email)) InMemoryAuthStore.Remove(email);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ResetPassword(string email, string newPassword)
        {
            if (!IsAdmin()) return new HttpUnauthorizedResult();
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(newPassword))
            {
                InMemoryAuthStore.UpdatePassword(email, newPassword);
            }
            return RedirectToAction("Index");
        }
    }
}
