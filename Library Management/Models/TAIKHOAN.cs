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
    
    public partial class TAIKHOAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAIKHOAN()
        {
            this.PHIEUMUONSACH = new HashSet<PHIEUMUONSACH>();
            this.PHIEUNHAPSACH = new HashSet<PHIEUNHAPSACH>();
            this.PHIEUTHUTIENPHAT = new HashSet<PHIEUTHUTIENPHAT>();
            this.PHIEUTRASACH = new HashSet<PHIEUTRASACH>();
        }
    
        public int MATK { get; set; }
        public string TAIKHOAN1 { get; set; }
        public string MATKHAU { get; set; }
        public Nullable<System.DateTime> NGDK { get; set; }
        public string HOTEN { get; set; }
        public string DCHI { get; set; }
        public string EMAIL { get; set; }
        public string HINHANH { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUMUONSACH> PHIEUMUONSACH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUNHAPSACH> PHIEUNHAPSACH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUTHUTIENPHAT> PHIEUTHUTIENPHAT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUTRASACH> PHIEUTRASACH { get; set; }
    }
}
