using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Used for transactions for merchants. When users pay for bills, merchants get a transaction
    /// </summary>
    public class MerchantTransaction
    {
        public int TransactionId { get; set; }
        public string EmailId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public byte PaymentTypeId { get; set; }
        public string Remarks { get; set; }
        public string Info { get; set; }
        public byte StatusId { get; set; }
    }
}