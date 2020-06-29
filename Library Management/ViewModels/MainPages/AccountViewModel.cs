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
using Library_Management.Resources.Sercurity;

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
        #endregion
        public AccountViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            eventAggregator.Subscribe(this);
            User = new TAIKHOAN(dataProvider.User);           
            if (string.IsNullOrEmpty(User.HINHANH) == true || File.Exists(User.HINHANH) == false)               
                User.HINHANH = @"/Resources/Images/LibrarianAccount/DefaultAccount.png";             
        }
        public void SaveChange()
        {
            
            bool IsSuccess = dataProvider.Update<TAIKHOAN>(User, g => g.MATK == User.MATK);
            if (IsSuccess)
            {
                this.eventAggregator.PublishOnCurrentThread("UpdatedUserAccountSuccessful");
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Lưu thành công"));
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Lưu thất bại"));
            }    
        }       
        public void ChangePassword()
        {
            string oldpass = Password;
            string newpass = NewPassword;
            bool IsSuccess = true;
            if (string.IsNullOrWhiteSpace((oldpass ?? " ").ToString()) && string.IsNullOrWhiteSpace((newpass ?? " ").ToString()))
            {
                IsSuccess = false;
            }
            else
            {
                oldpass = MD5Sercurity.MD5Hash(oldpass);
                newpass = MD5Sercurity.MD5Hash(newpass);
                if (dataProvider.GetItem<TAIKHOAN>(g => g.MATK == User.MATK && g.MATKHAU == oldpass) != null)
                {
                    User.MATKHAU = newpass;
                    IsSuccess = dataProvider.Update<TAIKHOAN>(User, g => g.MATK == User.MATK);
                }
                
            }           
            if (IsSuccess)
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
            User = new TAIKHOAN(dataProvider.User);
            if (string.IsNullOrEmpty(User.HINHANH) == true || File.Exists(User.HINHANH) == false)
                User.HINHANH = @"/Resources/Images/LibrarianAccount/DefaultAccount.png";
        }
        public async Task ChangeAvartar()
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
                bool IsSuccess = await dataProvider.ChangeAvartar(openFile.FileName);
                if (IsSuccess)
                {
                    this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Đổi ảnh đại diện thành công"));                   
                }
                else
                {
                    this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Đổi ảnh đại diện thất bại"));
                }    
            }
        }
    }
}
