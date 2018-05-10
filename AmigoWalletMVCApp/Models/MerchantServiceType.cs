using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Service types for bill payment
    /// </summary>
    public class MerchantServiceType
    {
        public byte ServiceId { get; set; }
        public string ServiceType { get; set; }
    }
}