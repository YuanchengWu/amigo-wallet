using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Maps merchant information to the type of service they fall under
    /// </summary>
    public class MerchantServiceMapping
    {
        public Int16 MerchantServiceMappingId { get; set; }
        public string EmailId { get; set; }
        public byte ServiceId { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime ModifiedTimestamp { get; set; }
    }
}