using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Models user credit cards. Used for adding credit cards in user account page
    /// </summary>
    public class UserCard
    {
        public int UserCardMappingId { get; set; }
        public string EmailId { get; set; }
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid Card Number")]
        [Required(ErrorMessage = "Card Number is mandatory.")]
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }
        public byte BankId { get; set; }
        [RegularExpression(@"^(0[1-9]|1[0-2])\/[0-9]{2}$", ErrorMessage = "Invalid Expiry Date")]
        [Required(ErrorMessage = "Expiry Date is mandatory.")]
        [DisplayName("Expiry Date")]
        public DateTime ExpiryDate { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime ModifiedTimestamp { get; set; }
    }
}