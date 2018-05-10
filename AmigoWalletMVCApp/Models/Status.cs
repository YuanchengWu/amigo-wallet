using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Used for status states (not utilized in our implementation)
    /// </summary>
    public class Status
    {
        public byte StatusId { get; set; }
        public string StatusValue { get; set; }
    }
}