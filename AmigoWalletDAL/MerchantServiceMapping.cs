//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmigoWalletDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class MerchantServiceMapping
    {
        public short MerchantServiceMappingId { get; set; }
        public string EmailId { get; set; }
        public byte ServiceId { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedTimestamp { get; set; }
        public Nullable<System.DateTime> ModifiedTimestamp { get; set; }
    
        public virtual MerchantServiceType MerchantServiceType { get; set; }
    }
}
