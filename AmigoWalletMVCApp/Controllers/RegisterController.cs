using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmigoWalletDAL;
using AmigoWalletMVCApp.Repository;

namespace AmigoWalletMVCApp.Controllers
{
    /// <summary>
    /// Allows user registration
    /// </summary>
    public class RegisterController : Controller
    {
        /// <summary>
        /// Show the registration form page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// Register the user to the database with the entered user info
        /// </summary>
        /// <param name="user">User model object with user information</param>
        /// <returns></returns>
        public ActionResult RegisterUser(Models.User user)
        {
            try
            {
                // Object for data access layer
                AmigoWalletRepository dal = new AmigoWalletRepository();

                // Maps User from model to DAL
                var mapObj = new AmigoWalletMapper<Models.User, User>();

                // Send user object to database, and directly show user home page
                var status = dal.RegisterUser(mapObj.Translate(user));
                if (status == 1)
                {
                    Session["email"] = user.EmailId;
                    return RedirectToAction("Home","Account");
                }
                else
                {
                    Session["regError"] = "Something went wrong, your email/phone may already be in use";
                    return RedirectToAction("Index", "Register");
                }
            }
            catch (Exception ex)
            {
                // For debugging exceptions
                return RedirectToAction("Index","Register");
            }
        }
    }
}
