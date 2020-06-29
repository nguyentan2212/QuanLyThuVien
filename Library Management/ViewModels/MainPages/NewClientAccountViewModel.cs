using Caliburn.Micro;
using Library_Management.Models;
using Library_Management.Views.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Library_Management.ViewModels.MainPages
{
    public class NewClientAccountViewModel:ViewBaseModel
    {
        #region Properties         
        
        private BindableCollection<LOAIDOCGIA> _ClientTypeList;
        public BindableCollection<LOAIDOCGIA> ClientTypeList
        {
            get { return _ClientTypeList; }
            set { _ClientTypeList = value; NotifyOfPropertyChange("ClientTypeList"); }
        }
        private LOAIDOCGIA _ClienType;
        public LOAIDOCGIA ClienType
        {
            get { return _ClienType; }
            set { _ClienType = value; NotifyOfPropertyChange("ClienType"); }
        }
        
        private int _ClientMinAge;
        public int ClientMinAge
        {
            get { return _ClientMinAge; }
            set { _ClientMinAge = value; NotifyOfPropertyChange("ClientMinAge"); }
        }

        private int _ClientMaxAge;
        public int ClientMaxAge
        {
            get { return _ClientMaxAge; }
            set { _ClientMaxAge = value; NotifyOfPropertyChange("ClientMaxAge"); }
        }

        private DOCGIA _Client;
        public DOCGIA Client
        {
            get { return _Client; }
            set { _Client = value; NotifyOfPropertyChange("Client"); }
        }

        #endregion
        public NewClientAccountViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            ClientTypeList = new BindableCollection<LOAIDOCGIA>();
            foreach (var item in dataProvider.ClientTypeList)
            {
                ClientTypeList.Add(item);
            }
            Client = new DOCGIA();
            if (string.IsNullOrEmpty(Client.HINHANH) == true || File.Exists(Client.HINHANH) == false)
                Client.HINHANH = @"/Resources/Images/ClientAccount/DefaultAccount.png";
            ClientMinAge = dataProvider.LibraryRules.TUOITOITHIEU ?? 18;
            ClientMaxAge = dataProvider.LibraryRules.TUOITOIDA ?? 55;            
            eventAggregator.Subscribe(this);                   
        }        
        public void Save()
        {
            Client.MALDG = ClienType.MALDG;          
            bool IsSuccess = dataProvider.Create<DOCGIA>(Client);
            if (IsSuccess)
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo mới thành công"));
                string imgPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\ClientAccount\" + Client.MADG.ToString() + ".png";
                Client.HINHANH = imgPath;
                File.Copy(Client.HINHANH, imgPath, true);
                IsSuccess = dataProvider.Update<DOCGIA>(Client, g => g.MADG == Client.MADG);
                Cancel();
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo mới thất bại"));
            }    
        }
        public void Cancel()
        {
            Client = new DOCGIA() { HINHANH = @"/Resources/Images/ClientAccount/DefaultAccount.png"};           
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
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Client.HINHANH = openFile.FileName;
            }
        }
        #region ClientTypeDialog
        private ClientTypeDialog ClientTypeForm = new ClientTypeDialog();
        private UserControl _DialogContent;
        public UserControl DialogContent
        {
            get { return _DialogContent; }
            set { _DialogContent = value; NotifyOfPropertyChange("DialogContent"); }
        }

        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get { return _IsDialogOpen; }
            set { _IsDialogOpen = value; NotifyOfPropertyChange("IsDialogOpen"); }
        }

        private LOAIDOCGIA _SelectedClientType;
        public LOAIDOCGIA SelectedClientType
        {
            get { return _SelectedClientType; }
            set 
            { 
                _SelectedClientType = value;
                TypeString = (value is null) ? null : value.LOAIDOCGIA1;
                NotifyOfPropertyChange("SelectedClientType"); 
            }
        }

        private string _TypeString;
        public string TypeString
        {
            get { return _TypeString; }
            set { _TypeString = value; NotifyOfPropertyChange("TypeString"); }
        }

        public void OpenEditClientType()
        {
            this.DialogContent = ClientTypeForm;
            ClientTypeForm.DataContext = this;
            IsDialogOpen = true;
            SelectedClientType = new LOAIDOCGIA();
        }
        public void Exit()
        {
            IsDialogOpen = false;
        }
        public void UpdateClientType()
        {           
            SelectedClientType.LOAIDOCGIA1 = TypeString;
            bool IsSuccess = dataProvider.Update<LOAIDOCGIA>(SelectedClientType, g => g.MALDG == SelectedClientType.MALDG);            
            if (IsSuccess)
            {                
                ShowMessage(new Models.Message("Thông báo", "Cập nhật thành công"));               
            }
            else
            {                
                ShowMessage(new Models.Message("Thông báo", "Cập nhật không thành công"));
            }
        }
        public void NewClientType()
        {           
            bool IsSuccess = dataProvider.Create<LOAIDOCGIA>(new LOAIDOCGIA() { LOAIDOCGIA1 = TypeString });
            if (IsSuccess)
            {                
                ShowMessage(new Models.Message("Thông báo", "Tạo mới thành công"));
                ClientTypeList = dataProvider.GetList<LOAIDOCGIA>();
            }
            else
            {
                ShowMessage(new Models.Message("Thông báo", "Tạo mới không thành công"));
            }
        }
        public void DeleteClientType()
        {                      
            bool IsSuccess = dataProvider.Delete<LOAIDOCGIA>(g => g.MALDG == SelectedClientType.MALDG);
            if (IsSuccess)
            {                
                ShowMessage(new Models.Message("Thông báo", "Xóa thành công"));
                ClientTypeList = dataProvider.GetList<LOAIDOCGIA>();               
            }           
            else
            {
                ShowMessage(new Models.Message("Thông báo", "Xóa không thành công"));
            }    
        }
        private void ShowMessage(Models.Message mess)
        {
            MessageBox.Show(mess.Note, mess.Title);         
        }
        #endregion
    }
}
