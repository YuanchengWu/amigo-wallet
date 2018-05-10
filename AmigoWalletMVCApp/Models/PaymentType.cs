using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Used for transaction payment type with source and destination
    /// </summary>
    public class PaymentType
    {
        public byte PaymentTypeId { get; set; }
        public string PaymentFrom { get; set; }
        public string PaymentTo { get; set; }
        public bool PaymentType1 { get; set; }
    }
}