//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library_Management.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PHIEUNHAPSACH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEUNHAPSACH()
        {
            this.CTSACH = new HashSet<CTSACH>();
        }
    
        public int MAPNS { get; set; }
        public Nullable<System.DateTime> NGAYLAP { get; set; }
        public Nullable<int> MATK { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTSACH> CTSACH { get; set; }
        public virtual TAIKHOAN TAIKHOAN { get; set; }
    }
}
