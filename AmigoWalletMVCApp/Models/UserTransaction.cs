using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Models each user transaction. Used for transfering funds to another user or bank, 
    /// paying bills, receiving funds, and adding funds
    /// </summary>
    public class UserTransaction
    {
        public int UserTransactionId { get; set; }
        public string EmailId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public byte PaymentTypeId { get; set; }
        public string Remarks { get; set; }
        public string Info { get; set; }
        public byte StatusId { get; set; }
        public int PointsEarned { get; set; }
        public bool IsRedeemed { get; set; }
    }
}