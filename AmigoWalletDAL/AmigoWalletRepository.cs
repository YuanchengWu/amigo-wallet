using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AmigoWalletDAL
{
    /// <summary>
    ///     The data access layer. Responsible for fetching date from the database.
    /// </summary> 
    public class AmigoWalletRepository
    {
        //Create the database context
        AmigoWalletDBContext context;

        //Constructor
        public AmigoWalletRepository()
        {
            //Create a new DB context object
            context = new AmigoWalletDBContext();
        }

        /// <summary>
        ///     Registers a user by calling the stored  imported from the database
        /// </summary>
        /// <param name="user">a UserObject</param>
        /// <returns>1 on success, 0 on failure</returns>
        public int RegisterUser(User user)
        {
            try
            {
                //calls the stored procedure from the database
                var status = context.usp_RegisterUser(user.EmailId, user.Name, user.Password, user.MobileNumber).FirstOrDefault();
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Checks to see if the User Name and Password combination exist in the database.
        /// </summary>
        /// <param name="userName">User Name to be validated</param>
        /// <param name="password">Password to be validated</param>
        /// <returns>1 if combination exists, 0 otherwise</returns>
        public int ValidateCredentials(string userName, string password)
        {
            //Calls the stored procedure from the database
            var status = context.usp_validateCredentials(userName, password).FirstOrDefault();
            //convert to int and return status
            return Convert.ToInt32(status);
        }

        /// <summary>
        ///     Resets the user associated with emailId if password and email combination are found in database
        /// </summary>
        /// <param name="emailId">EmailId to which associated password is to be changed</param>
        /// <param name="oldPassword">Existing password to be validated</param>
        /// <param name="newPassword">New password to be put into database</param>
        /// <returns>1 on success 0 on failure</returns>
        public int ResetPassword(string emailId, string oldPassword, string newPassword)
        {
            try
            {
                //Calls the stored procedure from the database
                var status = context.usp_ResetPassword(emailId, oldPassword, newPassword).FirstOrDefault();
                //Convert to int and return status
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        /// <summary>
        /// Adds money to a wallet from a card
        /// </summary>
        /// <param name="emailId">Email Id of user associated with wallet</param>
        /// <param name="cardId">Card Id od the card to be charged</param>
        /// <param name="amount">Amount of money to be transfered from card to wallet</param>
        /// <param name="remarks">Comment about the transaction</param>
        /// <returns>1 on success 0 on failure</returns>
        public int AddMoneyUsingCard(string emailId, int cardId, decimal amount, string remarks)
        {
            try
            {
                //Calls the stored procedure from the database
                var status = context.usp_AddMoneyWithCard(emailId, cardId, amount, remarks).FirstOrDefault();
                //convert to int and return
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        /// <summary>
        /// Adds a new card to the database
        /// </summary>
        /// <param name="card"></param>
        /// <returns>1 on Success 0 on failure</returns>
        public int AddNewCard(UserCard card)
        {
            try
            {
                //Calls the stored procedure from the database
                var status = context.usp_AddNewCard(card.EmailId, card.CardNumber, 1, card.ExpiryDate, 1).FirstOrDefault();
                //convert and return status
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        /// <summary>
        /// Gets all the cards associated with the given Email Id
        /// </summary>
        /// <param name="emailId">An Email Address</param>
        /// <returns>A List of type UserCard</returns>
        public List<UserCard> getCardsBySessionEmail(string emailId)
        {
            //Instanciates a list of type UserCard
            var cardList = new List<UserCard>();
            try
            {
                //Linq query to fetch the cards
                cardList = (from uc in context.UserCards
                             where emailId.Equals(uc.EmailId)
                             select uc).ToList();
                //return the list
                return cardList;
            }
            catch (Exception)
            {
                //return null on exception
                return null;
            }
        }
        /// <summary>
        /// Adds Money to the users account using a bank account
        /// </summary>
        /// <param name="emailId">An Email Address</param>
        /// <param name="amount">Amount of funds</param>
        /// <returns>1 on Success 0 on failure</returns>
        public int AddMoneyUsingBank(string emailId, decimal amount)
        {
            try
            {
                //calls the stored procedure from the database
                var status = context.usp_AddMoneyWithBank(emailId, amount).FirstOrDefault();
                //convert to int and return
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        /// <summary>
        /// Transfers Money from the account of the user associated with emailId to bank
        /// </summary>
        /// <param name="emailId">An Email address</param>
        /// <param name="amount">An Amount of funds to be transfered</param>
        /// <param name="info">Info on the transfer</param>
        /// <param name="remarks">User remarks on the transfer</param>
        /// <returns>1 on Success 0 on failure</returns>
        public int TransferToBank(string emailId, decimal amount, string info, string remarks)
        {
            try
            {
                //calls the stored procedure from the database
                var status = context.usp_WalletToBank(emailId, amount, info, remarks).FirstOrDefault();
                //convert to int and return
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        /// <summary>
        /// Transfers funds from one wallet to another
        /// </summary>
        /// <param name="fromEmailId">Account Sending funds</param>
        /// <param name="toEmailId">Account recieving funds</param>
        /// <param name="amount">Amount of funds to be transfered</param>
        /// <returns>1 on Success 0 on failure</returns>
        public int TransferToEWallet(string fromEmailId, string toEmailId, decimal amount)
        {
            try
            {
                //calls the stored procedure from the database
                var status = context.usp_WalletToWallet(fromEmailId, toEmailId, amount).FirstOrDefault();
                //convert to int and return
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        /// <summary>
        /// Pays merchand associated with merchantEmailId with funds from account associated with userEmailId
        /// </summary>
        /// <param name="userEmailId">User Email address</param>
        /// <param name="serviceType">The service type of the merchant</param>
        /// <param name="merchantEmailId">Merchant Email Address</param>
        /// <param name="amount">Amount of funds</param>
        /// <returns>1 on Success 0 on failure</returns>
        public int PayMerchant(string userEmailId, string merchantEmailId, decimal amount)
        {
            try
            {
                //call stored procedure from database
                var status = context.usp_PayBills(userEmailId, merchantEmailId, amount).FirstOrDefault();
                //convert to int and return
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }
           
        }

        /// <summary>
        /// Return the total rewards points earned by user associated with emailId
        /// </summary>
        /// <param name="emailId">User Email Address</param>
        /// <returns>1 on Success -1 on failure</returns>
        public int GetRewardPoints(string emailId)
        {
            int total = 0;
            try
            {
                //Get list of all points earned by user associated with email id
            var points = (from ut in context.UserTransactions
                          where emailId.Equals(ut.EmailId)
                          select ut.PointsEarned).ToList();
                //iterate through points list and add to total
            foreach (var item in points)
            {
                total += (int)item;
            }
            }
            catch (Exception ex)
            {
                //return -1 points on failure
                return -1;
            }
            //return the total amount of points
            return total;
        }
        /// <summary>
        /// Redeems the users rewards points by adding them to their wallet
        /// </summary>
        /// <param name="emailId">User Email Id</param>
        /// <returns>1 on success 0 on failure</returns>
        public int RedeemPoints(string emailId)
        {
            try
            {
                //calls the stored procedure from database
                var status = context.usp_RedeemRewardPoints(emailId).FirstOrDefault();
                //convert to int and return status
                return Convert.ToInt32(status);
            }
            catch (Exception)
            {
                return 0;
            }            
        }
        /// <summary>
        /// Gets the balance of the user associated with emailId
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns>The users balance</returns>
        public Decimal ViewBalance(string emailId)
        {
            try
            {
                // Calls the stored procedure, converts its output and returns
                return Convert.ToDecimal(context.usp_GetFullBalance(emailId).FirstOrDefault());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// Fetches all transactions associated with emailId between begin and end date.
        /// </summary>
        /// <param name="emailId">User Email Address</param>
        /// <param name="begin">Begining of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>A list of transactions</returns>
        public List<UserTransaction> ViewUserTransaction(string emailId, DateTime begin, DateTime end)
        {
            //Instantiate a transaction list
            var transList = new List<UserTransaction>();

            try
            {
                //Linq query to fetch transactions
                transList = (from ut in context.UserTransactions
                             where emailId.Equals(ut.EmailId)
                             && DateTime.Compare(ut.TransactionDateTime, begin) >= 0
                             && DateTime.Compare(ut.TransactionDateTime, end) <= 0
                             orderby ut.TransactionDateTime descending
                             select ut).ToList();
            }
            catch (Exception ex)
            {
                //return null on failure
                string msg = ex.Message.ToString();
                return null;
            }
            //return transaction list
            return transList;
        }
        /// <summary>
        /// Fetches all merchant transactions between the begin and end date
        /// </summary>
        /// <param name="emailId">Merchant Email Id</param>
        /// <param name="begin">Begining of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>A list of type MerchantTransaction</returns>
        public List<MerchantTransaction> ViewMerchantTransaction(string emailId, DateTime? begin = null, DateTime? end = null)
        {
            //instantiate 
            var b = begin ?? DateTime.Today.AddMonths(-1);
            var e = end ?? DateTime.Today.AddDays(1);
            var transList = new List<MerchantTransaction>();

            try
            {
                //Linq query to fetch transactions
                transList = (from ut in context.MerchantTransactions
                             where emailId.Equals(ut.EmailId)
                             && DateTime.Compare(ut.TransactionDateTime, b) >= 0
                             && DateTime.Compare(ut.TransactionDateTime, e) <= 0
                             orderby ut.TransactionDateTime descending
                             select ut).ToList();
            }
            catch (Exception)
            {
                //return null on failure
                return null;
            }
            //return list of Merchant Transactions
            return transList;
        }
        /// <summary>
        /// Fetches the service types for all merchants
        /// </summary>
        /// <returns>List of Service types</returns>
        public List<MerchantServiceType> GetServiceTypes()
        {
            var serviceList = new List<MerchantServiceType>();

            try
            {
                //Linq query
                serviceList = (from mst in context.MerchantServiceTypes
                               select mst).ToList();
            }
            catch (Exception)
            {
                //return null on failure
                return null;
            }

            return serviceList;
        }
        /// <summary>
        /// Fetches list of metchants based on service id
        /// </summary>
        /// <param name="serviceId">Service Id</param>
        /// <returns>List of Merchants</returns>
        public List<Merchant> GetMerchantsByServiceType(byte serviceId)
        {
            var merchantList = new List<Merchant>();

            try
            {
                //Linq Query to get merchants associated with serviceId
                merchantList = (from msm in context.MerchantServiceMappings
                                join m in context.Merchants
                                on msm.EmailId equals m.EmailId
                                where msm.ServiceId == serviceId
                                select m).ToList();
            }
            catch (Exception)
            {
                //return null on failure
                return null;
            }

            return merchantList;
        }
    }
}
