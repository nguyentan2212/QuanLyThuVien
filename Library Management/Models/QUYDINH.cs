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
    public partial class QUYDINH:PropertyChangedBase
    {

        private Nullable<int> _SACHMUONTOIDA;
        public Nullable<int> SACHMUONTOIDA
        {
            get { return _SACHMUONTOIDA; }
            set { _SACHMUONTOIDA = value; NotifyOfPropertyChange("SACHMUONTOIDA"); }
        }

        private Nullable<int> _NGAYMUON;
        public Nullable<int> NGAYMUON
        {
            get { return _NGAYMUON; }
            set { _NGAYMUON = value; NotifyOfPropertyChange("NGAYMUON"); }
        }

        private Nullable<decimal> _TIENPHAT;
        public Nullable<decimal> TIENPHAT
        {
            get { return _TIENPHAT; }
            set { _TIENPHAT = value; NotifyOfPropertyChange("TIENPHAT"); }
        }

        private Nullable<int> _KHOANGCACHXB;
        public Nullable<int> KHOANGCACHXB
        {
            get { return _KHOANGCACHXB; }
            set { _KHOANGCACHXB = value; NotifyOfPropertyChange("KHOANGCACHXB"); }
        }

        private Nullable<int> _TUOITOIDA;
        public Nullable<int> TUOITOIDA
        {
            get { return _TUOITOIDA; }
            set { _TUOITOIDA = value; NotifyOfPropertyChange("TUOITOIDA"); }
        }

        private Nullable<int> _TUOITOITHIEU;
        public Nullable<int> TUOITOITHIEU
        {
            get { return _TUOITOITHIEU; }
            set { _TUOITOITHIEU = value; NotifyOfPropertyChange("TUOITOITHIEU"); }
        }

        private Nullable<decimal> _TIENPHATTOIDA;
        public Nullable<decimal> TIENPHATTOIDA
        {
            get { return _TIENPHATTOIDA; }
            set { _TIENPHATTOIDA = value; NotifyOfPropertyChange("TIENPHATTOIDA"); }
        }

        private Nullable<int> _THOIHANTHE;
        public Nullable<int> THOIHANTHE
        {
            get { return _THOIHANTHE; }
            set { _THOIHANTHE = value; NotifyOfPropertyChange("THOIHANTHE"); }
        }
        public int ID { get; set; }
        public Nullable<System.DateTime> NGAYSUA { get; set; }
        public Nullable<int> NGUOISUA { get; set; }
        
    }
}
