using MyFirstAthenticationAthorize.Models;
using MyFirstAthenticationAthorize.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstAthenticationAthorize.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Login(string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            catch (Exception e)
            {
                // log.Error("", e);
                return View("error");
            }
        }
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {

            try
            {
                //var result = SecurityManager.Login(model.UserName, model.Password);

                if (true)//result.Success)
                {
                    if (AuthenticationModules.Login(model.UserName, model.Password))
                    {
                        string WelcomeMessage = "کاربر گرامی : " + ((UserIdentity)this.User).Name + "  به پنل کاربری خود خوش آمدید. ";

                        if (this.User.IsInRole("expertor"))
                            return RedirectToAction("Transactions_View", "Management");
                        else if (this.User.IsInRole("Guess"))
                        {
                            //return Redirect(returnUrl);
                            return RedirectToAction("Index", "CheckAthenticate");
                        }
                        else
                            return RedirectToAction("Requests_View", "Management");
                    }
                    else
                    {
                        AuthenticationModules.Logoff();

                        ViewBag.Error = "نام کاربری و رمز عبور شما یافت نشد";
                        return View("Login");
                    }
                }
                else
                {
                    AuthenticationModules.Logoff();

                    //ViewBag.Error = "در اتصال به امنیت متمرکز خطایی رخ داده است" +
                    //    result.ErrorDesc;
                    return View("Login");
                }

            }
            catch (Exception e)
            {
                ViewBag.Error = "نام کاربری و رمز عبور شما یافت نشد";
                return View("Login");
            }

        }

        //[Authorize]
        public ActionResult Logout()
        {
            AuthenticationModules.Logoff();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}