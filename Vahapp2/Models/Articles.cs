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
    
    public partial class Articles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articles()
        {
            this.Loans = new HashSet<Loans>();
        }
    
        public int ArticleID { get; set; }
        public int CategoryID { get; set; }
        public string SerialNumber { get; set; }
        public string ArticleName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public byte[] Image { get; set; }
        public Nullable<System.DateTime> Purchasedate { get; set; }
        public string Status { get; set; }
        public string Warranty { get; set; }
        public string Photopath { get; set; }
        public string Info { get; set; }
    
        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Loans> Loans { get; set; }
    }
}
