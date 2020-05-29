using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_Management.Models;
using Caliburn.Micro;

namespace Library_Management.ViewModels.MainPages
{
    public class AccountViewModel: ViewBaseModel
    {
        #region Properties
        public TAIKHOAN User { get; set; }
        private string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                NotifyOfPropertyChange("Password");
            }
        }
        private string _ConfirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return _ConfirmPassword;
            }
            set
            {
                _ConfirmPassword = value;
                NotifyOfPropertyChange("ConfirmPassword");
            }
        }
        private string _Avartar;
        public string Avartar
        {
            get
            {
                return _Avartar;
            }
            set
            {
                _Avartar = value;
                NotifyOfPropertyChange("Avartar");
            }
        }
        #endregion
        public AccountViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            User = dataProvider.User;
            if (string.IsNullOrEmpty(User.HINHANH))
                Avartar = "pack://application:,,,/Library_Management;component/Resources/Images/LibrarianAccount/DefaultAccount.png";
            else
                Avartar = User.HINHANH;
        }
    }
}
