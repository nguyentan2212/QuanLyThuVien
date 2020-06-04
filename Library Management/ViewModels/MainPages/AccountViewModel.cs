using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_Management.Models;
using Caliburn.Micro;
using System.Windows;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;

namespace Library_Management.ViewModels.MainPages
{
    public class AccountViewModel: ViewBaseModel
    {
        #region Properties
        private TAIKHOAN _User;
        public TAIKHOAN User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value;
                NotifyOfPropertyChange("User");
            }
        }
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
        private string _NewPassword;
        public string NewPassword
        {
            get
            {
                return _NewPassword;
            }
            set
            {
                _NewPassword = value;
                NotifyOfPropertyChange("NewPassword");
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
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyOfPropertyChange("Name");
            }
        }
        private string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                NotifyOfPropertyChange("UserName");
            }
        }
        private string _Address;
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                NotifyOfPropertyChange("Address");
            }
        }
        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                NotifyOfPropertyChange("Email");
            }
        }       
        #endregion
        public AccountViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            eventAggregator.Subscribe(this);
            User = dataProvider.User;
            UserName = User.TAIKHOAN1;
            Name = User.HOTEN;
            Address = User.DCHI;
            Email = User.EMAIL;
            if (string.IsNullOrEmpty(User.HINHANH))
                Avartar = "pack://application:,,,/Library_Management;component/Resources/Images/LibrarianAccount/DefaultAccount.png";
                
            else
                Avartar = User.HINHANH;
        }
        public async Task SaveChange()
        {
            
            bool save = await dataProvider.UpdateUserAccount(User);
            if (save)
            {
                this.eventAggregator.PublishOnCurrentThread("UpdatedUserAccountSuccessful");
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Lưu thành công"));
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Lưu thất bại"));
            }    
        }       
        public async Task ChangePassword()
        {
            
            bool change = await dataProvider.ChangePassword(Password, NewPassword);
            if (change)
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Đổi mật khẩu thành công"));
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Mật khẩu sai hoặc nhập lại"));
            }    
        }
        
        public void Cancel()
        {
            //User = dataProvider.User;
            UserName = User.TAIKHOAN1;
            Name = User.HOTEN;
            Address = User.DCHI;
            Email = User.EMAIL;
        }
        public void ChangeAvartar()
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.InitialDirectory = @"C:\";
            openFile.RestoreDirectory = true;
            openFile.Title = "Đổi ảnh đại diện";
            openFile.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg";
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.Multiselect = false;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                System.Windows.MessageBox.Show(openFile.FileName);
            }
        }
    }
}
