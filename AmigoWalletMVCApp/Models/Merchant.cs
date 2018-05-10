using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Merchant information. Used for bill payment and merchant login
    /// </summary>
    public class Merchant
    {
        public Int16 MerchantId { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime ModifiedTimestamp { get; set; }
    }
}