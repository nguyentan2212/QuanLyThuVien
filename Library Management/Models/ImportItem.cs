using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
namespace Library_Management.Models
{
    public class ImportItem: PropertyChangedBase
    {       
        private int _BookId;
        public int BookId
        {
            get { return _BookId; }
            set { _BookId = value; NotifyOfPropertyChange("BookId"); }
        }

        private string _BookTitle;
        public string BookTitle
        {
            get { return _BookTitle; }
            set { _BookTitle = value; NotifyOfPropertyChange("BookTitle"); }
        }

        private decimal _CoverPrice;
        public decimal CoverPrice
        {
            get { return _CoverPrice; }
            set { _CoverPrice = value; NotifyOfPropertyChange("CoverPrice"); }
        }

        private decimal _ImportPrice;
        public decimal ImportPrice
        {
            get { return _ImportPrice; }
            set 
            { 
                _ImportPrice = value;
                TotalPrice = _ImportPrice * Amount;
                NotifyOfPropertyChange("ImportPrice"); }
        }

        private int _Amount;
        public int Amount
        {
            get { return _Amount; }
            set 
            { 
                _Amount = value;
                TotalPrice = _ImportPrice * Amount;
                NotifyOfPropertyChange("Amount"); }
        }


        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; NotifyOfPropertyChange("TotalPrice"); }
        }

        public ImportItem(SACH s, int amount, decimal coverPrice, decimal importPrice)
        {
            BookId = s.MASACH;
            BookTitle = s.TENSACH;
            Amount = amount;
            CoverPrice = coverPrice;
            ImportPrice = importPrice;
        }
        public ImportItem()
        {

        }
    }
}
