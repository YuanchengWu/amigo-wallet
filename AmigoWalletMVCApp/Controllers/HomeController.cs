using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmigoWalletMVCApp.Controllers
{
    /// <summary>
    /// Landing page controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Draws the landing page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}