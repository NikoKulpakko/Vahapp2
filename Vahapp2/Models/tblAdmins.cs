//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vahapp2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class tblAdmins
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        [DisplayName("Email address")]
        public string AdminEmail { get; set; }
        [Required]
        [DisplayName("Password")]
        public string AdminPass { get; set; }
        public string LoginErrorMessage { get; set; }
    }
}