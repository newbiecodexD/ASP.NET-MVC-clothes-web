using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using web_quanao.Infrastructure.Data; // changed from Models
using web_quanao.Models; // add back for view models
using System.Diagnostics;
using System.Text.RegularExpressions;
using web_quanao.Services; // in-memory auth store

namespace web_quanao.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        // Hardcoded admin credentials (no DB)
        private const string AdminEmail = "admin@gmail.com";
        private const string AdminPassword = "Admin123@";

        public AccountController() { }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (TempData["RegisterSuccessMsg"] != null)
            {
                ViewBag.RegisterSuccess = TempData["RegisterSuccessMsg"]; // show alert on login page
            }
            return View();
        }

        // In-memory sign-in helper (user area)
        private void SignInInMemory(string email, bool remember)
        {
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, email));
            identity.AddClaim(new Claim(ClaimTypes.Name, email));
            // Add identity provider claim expected by antiforgery when using claims auth
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "InMemory"));
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = remember }, identity);
        }

        // Hardcoded admin session flag
        private void SignInAdmin()
        {
            Session["IsAdmin"] = "true";
        }

        private bool IsAdminSession() => (Session["IsAdmin"] as string) == "true";

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password, bool? rememberMe, string returnUrl)
        {
            // Admin short-circuit
            if (string.Equals(email, AdminEmail, StringComparison.OrdinalIgnoreCase) && password == AdminPassword)
            {
                SignInAdmin();
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            // Fallback to regular in-memory user store
            if (InMemoryAuthStore.Validate(email, password))
            {
                SignInInMemory(email, rememberMe ?? false);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (InMemoryAuthStore.Exists(model.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại (bộ nhớ tạm).");
                return View(model);
            }
            // basic password policy mimic (length >=6 already validated) no hashing (DEV ONLY)
            InMemoryAuthStore.AddUser(model.Email, model.Password);
            TempData["RegisterSuccessMsg"] = "Bạn đã đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // The rest (forgot/reset/external) kept but not functional in in-memory mode
        [AllowAnonymous]
        public ActionResult ForgotPassword() => View();
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            // In-memory: just show confirmation
            return View("ForgotPasswordConfirmation");
        }
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation() => View();
        [AllowAnonymous]
        public ActionResult ResetPassword(string code) => View("Error");
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            ModelState.AddModelError("", "Tính năng không khả dụng ở chế độ in-memory.");
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation() => View();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            // Clear both cookie auth and admin session flag
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Remove("IsAdmin");
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Remove("IsAdmin");
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Helpers
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}