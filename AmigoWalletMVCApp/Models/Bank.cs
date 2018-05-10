using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Contains bank information. Used for bank value transfer
    /// </summary>
    public class Bank
  {
        [RegularExpression(@"^[a-z]{0,3}\d{7}$", ErrorMessage = "Invalid IFSC Number")]
        [Required(ErrorMessage = "IFSC Number is mandatory.")]
        [DisplayName("IFSC Number")]
        public byte BankId { get; set; }
        public string BankName { get; set; }
    }
}