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
    using Caliburn.Micro;
    public partial class DOCGIA:PropertyChangedBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCGIA()
        {
            this.PHIEUMUONSACH = new HashSet<PHIEUMUONSACH>();
            this.PHIEUTHUTIENPHAT = new HashSet<PHIEUTHUTIENPHAT>();
            this.PHIEUTRASACH = new HashSet<PHIEUTRASACH>();
        }
    
        public int MADG { get; set; }
        private string _HOTEN;
        public string HOTEN
        {
            get { return _HOTEN; }
            set { _HOTEN = value; NotifyOfPropertyChange("HOTEN"); }
        }
        public Nullable<int> MALDG { get; set; }

        private Nullable<System.DateTime> _NGSINH;
        public Nullable<System.DateTime> NGSINH
        {
            get { return _NGSINH; }
            set { _NGSINH = value; NotifyOfPropertyChange("NGSINH"); }
        }

        private string _DCHI;
        public string DCHI
        {
            get { return _DCHI; }
            set { _DCHI = value; NotifyOfPropertyChange("DCHI"); }
        }

        private string _EMAIL;
        public string EMAIL
        {
            get { return _EMAIL; }
            set { _EMAIL = value; NotifyOfPropertyChange("EMAIL"); }
        }
        private Nullable<DateTime> _NGLAPTHE;
        public Nullable<DateTime> NGLAPTHE
        {
            get { return _NGLAPTHE; }
            set { _NGLAPTHE = value; NotifyOfPropertyChange("NGLAPTHE"); }
        }      
        private Nullable<Decimal> _TIENNO;
        public Nullable<Decimal> TIENNO
        {
            get { return _TIENNO; }
            set { _TIENNO = value; NotifyOfPropertyChange("TIENNO"); }
        }
       
        public Nullable<int> NGUOILAP { get; set; }

        private string _HINHANH;
        public string HINHANH
        {
            get { return _HINHANH; }
            set { _HINHANH = value; NotifyOfPropertyChange("HINHANH"); }
        }

        public virtual LOAIDOCGIA LOAIDOCGIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUMUONSACH> PHIEUMUONSACH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUTHUTIENPHAT> PHIEUTHUTIENPHAT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUTRASACH> PHIEUTRASACH { get; set; }
    }
}
