using Caliburn.Micro;
using Library_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Library_Management.ViewModels.MainPages
{
    public class NewClientAccountViewModel:ViewBaseModel
    {
        #region Properties         
        private BindableCollection<LOAIDOCGIA> _ClientTypeList;
        public BindableCollection<LOAIDOCGIA> ClientTypeList
        {
            get
            {
                return _ClientTypeList;
            }
            set
            {
                _ClientTypeList = value;
                NotifyOfPropertyChange("ClientTypeList");
            }
        }
        private int _ClientMinAge;
        public int ClientMinAge
        {
            get
            {
                return _ClientMinAge;
            }
            set
            {
                _ClientMinAge = value;
                NotifyOfPropertyChange("ClientMinAge");
            }
        }
        private int _ClientMaxAge;
        public int ClientMaxAge
        {
            get
            {
                return _ClientMaxAge;
            }
            set
            {
                _ClientMaxAge = value;
                NotifyOfPropertyChange("ClientMaxAge");
            }
        }
        private string _ClientName;
        public string ClientName
        {
            get
            {
                return _ClientName;
            }
            set
            {
                _ClientName = value;
                NotifyOfPropertyChange("ClientName");
            }
        }
        private LOAIDOCGIA _ClientType;
        public LOAIDOCGIA ClientType
        {
            get
            {
                return _ClientType;
            }
            set
            {
                _ClientType = value;
                NotifyOfPropertyChange("ClientType");
            }
        }
        private DateTime _ClientBirdthday;
        public DateTime ClientBirdthday
        {
            get
            {
                return _ClientBirdthday;
            }
            set
            {
                _ClientBirdthday = value;
                NotifyOfPropertyChange("ClientBirdthday");
            }
        }
        private string _ClientEmail;
        public string ClientEmail
        {
            get
            {
                return _ClientEmail;
            }
            set
            {
                _ClientEmail = value;
                NotifyOfPropertyChange("ClientEmail");
            }
        }
        private string _ClientAddress;
        public string ClientAddress
        {
            get
            {
                return _ClientAddress;
            }
            set
            {
                _ClientAddress = value;
                NotifyOfPropertyChange("ClientAddress");
            }
        }
        private string _ClientAvartar;
        public string ClientAvartar
        {
            get
            {
                return _ClientAvartar;
            }
            set
            {
                _ClientAvartar = value;
                NotifyOfPropertyChange("ClientAvartar");
            }
        }
        #endregion
        public NewClientAccountViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            ClientTypeList = dataProvider.ClientTypeList;
            ClientMinAge = dataProvider.LibraryRules.TUOITOITHIEU ?? 18;
            ClientMaxAge = dataProvider.LibraryRules.TUOITOIDA ?? 55;
            ClientBirdthday = DateTime.Today;
            eventAggregator.Subscribe(this);
            ClientAvartar = @"/Resources/Images/ClientAccount/DefaultAccount.png";           
        }
        public DOCGIA client { get; set; }
        public async Task Save()
        {
            client = new DOCGIA();           
            client.HOTEN = ClientName;
            client.MALDG = ClientType.MALDG;
            client.NGSINH = ClientBirdthday;
            client.EMAIL = ClientEmail;
            client.DCHI = ClientAddress;
            client.HINHANH = ClientAvartar;
            client.NGLAPTHE = DateTime.Today;
            client.NGUOILAP = dataProvider.User.MATK;
            bool save = await dataProvider.NewClient(client);
            if (save)
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo mới thành công"));
                Cancel();
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo mới thất bại"));
            }    
        }
        public void Cancel()
        {
            client = new DOCGIA();
            ClientName = null;
            ClientType = null;
            ClientBirdthday = DateTime.Today;
            ClientEmail = null;
            ClientAddress = null;
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
                ClientAvartar = openFile.FileName;
            }
        }
    }
}
