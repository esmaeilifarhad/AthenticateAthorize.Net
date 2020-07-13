using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstAthenticationAthorize.Controllers
{
    [Authorize]
    public class CheckAthenticateController : Controller
    {
      
        [Authorize(Roles = "Admin,Guess")]
        // GET: CheckAthenticate
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Guess")]
        [Authorize(Roles = "Admin")]
        // GET: CheckAthenticate
        public ActionResult Index2()
        {
            return View();
        }
    }
}