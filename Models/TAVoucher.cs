//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplication1.Models
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    public partial class TAVoucher
    {
        public int VoucherID { get; set; }
        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }
        [Required(ErrorMessage = "Destination field is required")]
        public string Destination { get; set; }
        [Display(Name = "Leave Duration(Days)")]
        [Required(ErrorMessage = "Leave duration field is required")]
        public Nullable<int> LeaveDuration { get; set; }
        public Nullable<System.DateTime> SubmitDate { get; set; }
    }
}
