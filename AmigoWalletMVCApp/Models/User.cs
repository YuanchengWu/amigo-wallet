using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AmigoWalletMVCApp.Models
{
    /// <summary>
    /// Used to model user information. Models registration validation and login
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "EmailId is mandatory.")]
        [DisplayName("Email Id:")]
        public string EmailId { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Mobile Number")]
        [Required(ErrorMessage = "MobileNumber is mandatory.")]
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Invalid Name")]
        [Required(ErrorMessage = "Name is mandatory.")]
        [DisplayName("Name:")]
        public string Name { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,20}$", ErrorMessage = "Invalid Password.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is mandatory.")]
        [DisplayName("Password")]
        public string Password { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime ModifiedTimestamp { get; set; }
    }
}