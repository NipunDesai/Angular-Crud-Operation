using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using Angular.Core.Global;
using Angular.Repository.ApplicationClass;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web;
using Angular.Repository.Logger;
using System;

namespace Angular.Core.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private readonly ILogger _errorLog;


       
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ILogger errorLog)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _errorLog = errorLog;

        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        //
        // GET: /Account/Login
        /// <summary>
        /// User will be redirected to Home screen if authenticated otherwise on login screen.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // POST: /Account/Login
        /// <summary>
        /// Method that will be used when user provide user name and password.
        /// </summary>
        /// <param name="model">Loginview model with userdetails</param>
        /// <param name="returnUrl">home page url.</param>
        /// <returns>authenticatted user role is admin then it will redirect to the home page otherwise it will redirect the login page.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);

                    if (user != null)
                    {
                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                            switch (result)
                            {
                                case SignInStatus.Success:
                                    HttpContext.Session["UserId"] = user.Id;
                                    return RedirectToLocal(returnUrl);
                                case SignInStatus.Failure:
                                default:
                                    ModelState.AddModelError("", "Invalid login attempt.");
                                    return View(model);
                            }
                        }
                        else
                        {
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt");
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);

            }
            catch (Exception ex)
            {
                _errorLog.LogException(ex);
                throw;
            }
        }


        // GET: /Account/UserResetPassword
        /// <summary>
        /// Method will be rediretd to the resetpassword page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {

            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        /// <summary>
        /// user will redirect to the resetpassword confirmation page.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // POST: /Account/LogOff
        /// <summary>
        /// Method that will be used to log off the reporting screen.
        /// </summary>
        /// <returns>redirect to the login page</returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                AuthenticationManager.SignOut();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _errorLog.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Disposing the user manager and signin manager.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Method that is used to redirect user to admin screen if return url is null.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>redirect to  home page</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}