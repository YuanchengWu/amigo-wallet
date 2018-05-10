using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmigoWalletDAL;

namespace AmigoWalletMVCApp.Controllers
{
    /// <summary>
    /// Shows and handles user login
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// Show the login page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        { 
            return View();
        }

        /// <summary>
        /// Handles login process
        /// </summary>
        /// <param name="email">User's email input</param>
        /// <param name="password">User's password input</param>
        /// <returns></returns>
        public ActionResult Login(string email, string password)
        {
            // Create a error message variable for login failure alert
            ViewBag.ErrorMessage = null;

            // Object for data access layer
            AmigoWalletRepository dal = new AmigoWalletRepository();

            // Check if the entered credentials match database records
            var status = dal.ValidateCredentials(email, password);
            if (status == 1)
            {
                // Upon success, create a session with user's emailID and direct user to account home page
                Session["email"] = email;
                Session.Timeout = 20;
                return RedirectToAction("Home", "Account");
            }
            // Return the error message on failure
            else
                ViewBag.ErrorMessage = "Password/Email Combination Incorrect, Try Again";
                return View("Index");
        }

        /// <summary>
        /// Handles logout and abandon session
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            // Abandon user session and redirect to landing page
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
