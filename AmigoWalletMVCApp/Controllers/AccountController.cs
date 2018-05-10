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
    /// Controller class for all account related operations
    /// </summary>
    public class AccountController : Controller
    {

        /// <summary>
        /// Generates the account homepage with transactions, balance, cards, merchants, and utilities
        /// </summary>
        /// <param name="from">from date for the transactions to be fetched</param>
        /// <param name="from">to date for the transactions to be fetched</param>
        /// <returns></returns>
        public ActionResult Home(DateTime? from = null, DateTime? to = null)
        {

            try
            {
                // Data access layer object
                AmigoWalletRepository repObj = new AmigoWalletRepository();

                // Calculate rewards points and pass it to the view
                ViewBag.ViewRewards = repObj.GetRewardPoints(Session["email"].ToString());
                Session["Rewards"] = repObj.GetRewardPoints(Session["email"].ToString());

                // Calculate balance points and pass it to the view
                ViewBag.ViewBalance = repObj.ViewBalance(Session["email"].ToString());


                //------- Get cards -------
                // Mapper for UserCard model to DAL
                AmigoWalletMapper<UserCard, Models.UserCard> map = new AmigoWalletMapper<UserCard, Models.UserCard>();

                // Get the list of all cards current user has, and add to a model list via mapping
                List<UserCard> lstCards = repObj.getCardsBySessionEmail(Session["email"].ToString());
                List<Models.UserCard> cards = new List<Models.UserCard>();
                foreach (var card in lstCards)
                {
                    cards.Add(map.Translate(card));
                }
                // Pass cards to the view
                ViewBag.UserCards = cards;


                //------- Get transactions -------
                // Mapper for the UserTransaction DAL to model
                var mapObj = new AmigoWalletMapper<UserTransaction, Models.UserTransaction>();

                // Create non-nullable DateTime objects for from and to and set them to the required range if passed in as null
                var f = from ?? DateTime.Today.AddMonths(-1);
                var t = to ?? DateTime.Today.AddDays(2);  // added two days to compensate for time difference

                // Get user's list of transactions
                var transList = repObj.ViewUserTransaction(Session["email"].ToString(), f, t);
                var transModelList = new List<Models.UserTransaction>();

                // Pass date range as passed in to display on the view's date range selectors
                ViewBag.fromDate = f.ToString("yyyy-MM-dd");
                ViewBag.toDate = t.ToString("yyyy-MM-dd");

                // Fill the model list with mapper and pass it to the view for display
           
                foreach (var transaction in transList)
                {
                    transModelList.Add(mapObj.Translate(transaction));
                }
                ViewBag.Transactions = transModelList;


                //------- Pay bill utility list -------
                // Fill list with the available service/utility types
                var utilList = new List<SelectListItem>();
                var serviceTypeList = repObj.GetServiceTypes();

                // Create a list of selection options for the utility list dropdown and pass it to the view
                utilList.Add(new SelectListItem { Text = "Please select a utility type", Value = "0" });
                for (int i = 0; i < serviceTypeList.Count; i++)
                {
                    utilList.Add(new SelectListItem { Text = serviceTypeList[i].ServiceType, Value = (i + 1).ToString() });
                }
                ViewData["utilities"] = utilList;

                // Draw the account homepage
                return View();
            }
            catch (Exception)
            {
                // Show session expiration page and allow re-log
                return View("_SessionExpired");
            }
        }

        /// <summary>
        /// Refreshes the page and displays transactions based on the selected date range
        /// </summary>
        /// <param name="collection">Form collection for the dates</param>
        /// <returns></returns>
        public ActionResult UpdateTransaction(FormCollection collection)
        {
            try
            {
                // Get the user selected dates and reload the account page
                var fromDate = collection["fromDate"];
                var toDate = collection["toDate"];
                return RedirectToAction("Home", new { from = fromDate, to = toDate });
            }
            catch (Exception)
            {
                // Return to the landing page when error
                return View("~/Views/Home/Index");
            }
        }

        /// <summary>
        /// Adds a new card to the current user's account
        /// </summary>
        /// <param name="cardNumber">New card's number</param>
        /// <param name="expMonth">New card's expiry month</param>
        /// <param name="expYear">New card's expiry year</param>
        /// <returns></returns>
        public ActionResult AddNewCard(string cardNumber, int expMonth, int expYear)
        {
            // Data access layer object
            AmigoWalletRepository dal = new AmigoWalletRepository();

            // New UserCard object with the required fields
            UserCard newCard = new UserCard();
            newCard.CardNumber = cardNumber;
            newCard.EmailId = Session["email"].ToString();
            newCard.ExpiryDate = new DateTime(expYear, expMonth, 1);

            // Add the card to the database
            var status = dal.AddNewCard(newCard);
            
            // Check for success
            if (status == 1)
            {
                // Refresh homepage to show new card information
                return RedirectToAction("Home");
            }
            else
            {
                // Show error if DAL failure
                return View("Error");
            }
        }

        /// <summary>
        /// Transfers amount to bank
        /// </summary>
        /// <param name="collection">Form collection for bank info</param>
        /// <returns></returns>
        public ActionResult TransferToBank(FormCollection collection)
        {
            // Data access layer object
            var repObj = new AmigoWalletRepository();

            // Reset previous session alert messages
            Session["message"] = null;
            Session["messageState"] = 0;

            // Put together a info string to be sent to database
            string info = "IFSC: " + collection["ifsc"] + " Account Number: " + collection["accountNumber"] + " Account Holder: " + collection["accountHolder"];
            try
            {
                // Make the transaction and send success alert to view
                repObj.TransferToBank(Session["email"].ToString(), Convert.ToDecimal(collection["amount"]), info, collection["remarks"]);
                Session["message"] = info;
                Session["messageState"] = 1;
            }
            catch (Exception ex)
            {
                // Show error alert to the view with exception message
                Session["message"] = "Some error occurred: " + ex.Message;
                Session["messageState"] = -1;
            }

            // Refresh homepage
            return RedirectToAction("Home");
        }

        /// <summary>
        /// Transfer money from selected user's card
        /// </summary>
        /// <param name="card">User's selected card ID</param>
        /// <param name="amount">Desired amount to transfer to wallet</param>
        /// <returns></returns>
        public ActionResult TransferFromCard(int card, decimal amount)
        {
            // Data access layer object
            AmigoWalletRepository dal = new AmigoWalletRepository();

            // Add money to wallet and check return status
            var status = dal.AddMoneyUsingCard(Session["email"].ToString(), card, amount, "none");
            if (status == 1)
            {
                return RedirectToAction("Home");
            }
            // Refresh homepage
            return RedirectToAction("Home");
        }

        /// <summary>
        /// Fetches a JSON result for the selected service type for bill payment
        /// </summary>
        /// <param name="id">Selected service type ID</param>
        /// <returns></returns>
        public JsonResult GetMerchants(string id)
        {
            // Make a list of options for the select dropdown
            var merchants = new List<SelectListItem>();

            // Data access layer object
            var repObj = new AmigoWalletRepository();

            // Get a list of merchants who belongs to the selected service type and add them to the list of choices
            var merchantList = repObj.GetMerchantsByServiceType(Convert.ToByte(id));
            for (int i = 0; i < merchantList.Count; i++)
            {
                merchants.Add(new SelectListItem { Text = merchantList[i].EmailId, Value = merchantList[i].EmailId });
            }

            // Return the JSON result
            return Json(new SelectList(merchants, "Value", "Text"));
        }

        /// <summary>
        /// Handles bill payment when user clicks Pay
        /// </summary>
        /// <param name="collection">Form collection for bank info</param>
        /// <returns></returns>
        public ActionResult PayBill(FormCollection collection)
        {
            // Data access layer object
            var repObj = new AmigoWalletRepository();

            // Reset previous session alert messages
            Session["message"] = null;
            Session["messageState"] = 0;

            // Put together a info string to be sent to database
            string info = "Utility Type: " + collection["Utilities"] + " Merchant: " + collection["Merchants"];
            try
            {
                // Make the transaction and send success alert to view
                repObj.PayMerchant(Session["email"].ToString(), collection["Merchants"], Convert.ToDecimal(collection["amount"]));
                Session["message"] = info;
                Session["messageState"] = 1;
            }
            catch (Exception ex)
            {
                // Show error alert to the view with exception message
                Session["message"] = "Some error occurred: " + ex.Message;
                Session["messageState"] = -1;
            }

            // Refresh homepage
            return RedirectToAction("Home");
        }

        /// <summary>
        /// Draws the change password page
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Push the password change to the database
        /// </summary>
        /// <param name="oldPassword">User's old password</param>
        /// <param name="newPassword">User's new password</param>
        /// <returns></returns>
        public ActionResult SaveNewPassword(string oldPassword, string newPassword)
        {
            // Data access layer object
            AmigoWalletRepository dal = new AmigoWalletRepository();

            // Make the password change for the user
            var status = dal.ResetPassword(Session["email"].ToString(), oldPassword, newPassword);
            if (status == 1)
            {
                // Password changed, refresh homepage
                return RedirectToAction("Home");
            }
            else
            {
                // Something went wrong, show status
                return View(status.ToString());
            }
        }
    }
}
