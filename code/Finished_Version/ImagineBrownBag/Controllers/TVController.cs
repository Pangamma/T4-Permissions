using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImagineBrownBag.Controllers
{
    public class TVController : Controller
    {
        // GET: TV
        public ActionResult Index()
        {
            // Before
            if (this.User.IsInRole("Parent") || this.User.IsInRole("Student without homework") || this.User.IsInRole("Baby"))
            {
                // do TV watching logic here
            }


            // After
            if (Permissions.HasPermission(Permissions.CanWatchTV))
            {
                // do TV watching logic here
            }

            return View();
        }
    }
}