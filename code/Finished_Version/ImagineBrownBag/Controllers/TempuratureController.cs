using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImagineBrownBag.Controllers
{
    [HasPermission(Permissions.CanViewTempurature)]
    public class TempuratureController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            //Fetch temp and display it
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(Permissions.CanAdjustTempurature, Permissions.CanViewTempurature)]
        [HasPermission(Permissions.CanPlayVideoGames)]
        public ActionResult SetTempurature(int newTempurature)
        {
            // Set tempurature 
            return View();
        }
    }
}



