using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmigoWalletDAL;

namespace AmigoWalletMVCApp.Controllers
{
    /// <summary>
    /// Deprecated transfer controller, refer to AccountController
    /// </summary>
    public class TransactionController : Controller
    {
        public ActionResult AddMoneyFromBank()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PerformAddMoneyFromBank(string emailId, decimal amount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var repObj = new AmigoWalletRepository();
                    var status = repObj.AddMoneyUsingBank(emailId, amount);
                    if (status == 1)
                        return View("Success");
                    else
                        return View("Error");
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
            return View("AddMoneyFromBank");
        }

        public ActionResult TransferToUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PerformTransferToUser(Models.UserTransaction trans)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var repObj = new AmigoWalletRepository();
                    var status = repObj.TransferToEWallet(Session["email"].ToString(),trans.EmailId, trans.Amount);
                    if (status == 1)
                        return RedirectToAction("Home", "Account");
                    else
                        return RedirectToAction("Home", "Account");
                }
                catch (Exception ex)
                {
                    return View(ex.Message.ToString());
                }
            }
            return RedirectToAction("Home","Account");
        }
        public ActionResult RedeemPoints()
        {
            if (Convert.ToInt32(Session["Rewards"]) < 10)
            {
                Session["message"] = "Not Enough Points To Redeem, Spend Some More Money";
                Session["messageState"] = -1;
            }
            var repObj = new AmigoWalletRepository();
            var status = repObj.RedeemPoints(Session["email"].ToString());
            // Calculate rewards points and pass it to the view
            ViewBag.ViewRewards = repObj.GetRewardPoints(Session["email"].ToString());
            Session["Rewards"] = repObj.GetRewardPoints(Session["email"].ToString());
            if (status == 1)
            {
                return RedirectToAction("Home", "Account");
            }
            return RedirectToAction("Home", "Account");
        }
    }
}