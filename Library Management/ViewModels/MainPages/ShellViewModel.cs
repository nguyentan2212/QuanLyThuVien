using Caliburn.Micro;
using Library_Management.Models;
using Library_Management.StartUp;
using Library_Management.Views.UserControls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Library_Management.ViewModels.MainPages
{
    public class ShellViewModel : ViewBaseModel
    {
        #region 
        private SimpleContainer _container;
        private INavigationServiceEx navigationService;
        private Login Loginform = new Login();
        private MetroWindow window;
        #endregion

        #region UI Properties
        private bool _DrawerOpen = false;
        public bool DrawerOpen
        {
            get
            {
                return _DrawerOpen;
            }
            set
            {
                _DrawerOpen = value;
                NotifyOfPropertyChange("DrawerOpen");
            }
        }
        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get
            {
                return _IsDialogOpen;
            }
            set
            {
                _IsDialogOpen = value;
                NotifyOfPropertyChange("IsDialogOpen");
            }
        }
        private UserControl _DialogContent;
        public UserControl DialogContent
        {
            get
            {
                return _DialogContent;
            }
            set
            {
                _DialogContent = value;
                NotifyOfPropertyChange("DialogContent");
            }
        }
        private bool isNotWorking = true;

        public bool IsNotWorking
        {
            get
            {
                return isNotWorking;
            }
            set
            {
                isNotWorking = value;
                if (value)
                {
                    IsWorking = Visibility.Collapsed;
                }
                else
                {
                    IsWorking = Visibility.Visible;
                }
                NotifyOfPropertyChange("IsWorking");
                NotifyOfPropertyChange("IsNotWorking");
            }
        }
        private string _DisplayName;
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
                NotifyOfPropertyChange("DisplayName");
            }
        }
        public Visibility IsWorking { private set; get; }
        #endregion
        #region Login Properties
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
        #endregion
        public ShellViewModel(SimpleContainer container, DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            this._container = container;
            eventAggregator.Subscribe(this);
            ShowLogin();
            
        }
        public void ShowLogin()
        {
            this.DialogContent = Loginform;
            Loginform.DataContext = this;
            UserName = Password = string.Empty;
            IsNotWorking = true;
            IsDialogOpen = true;
        }
        public async void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {
                    throw new Exception("Chưa nhập username hoặc mật khẩu");
                }
                IsNotWorking = false;
                //await Task.Delay(500);
                await dataProvider.FindUser(UserName, Password);
                if (dataProvider.User != null)
                {
                    DisplayName = dataProvider.User.HOTEN;
                    IsDialogOpen = false;
                }
                else
                {
                    throw new Exception("Username hoặc mật khẩu sai");
                }    
            }
            catch (Exception e)
            {
                await ShowMessage1("Lỗi đăng nhập", e.Message);
                return;
            }
            finally
            {
                IsNotWorking = true;
            }
        }
        public async Task Logout()
        {           
            await ShowMessage("Thông báo", string.Concat("Tạm biệt, ", DisplayName));
            DisplayName = UserName = Password = string.Empty;
            this.dataProvider.User = null;
            IsNotWorking = true;
            IsDialogOpen = true;
        }
        public void Exit()
        {
            Application.Current.Shutdown();
        }
        public void RegisterFrame(Frame frame)
        {
            navigationService = new NavigationService(frame);
            _container.Instance(navigationService);
            window = GetView() as MetroWindow;
            if (window is null)
            {
                throw new Exception("Can't get view");
            }
        }

        public void NavigateToView(string name)
        {
            switch (name)
            {
                case "Account": 
                    navigationService.NavigateToViewModel<AccountViewModel>();
                    break;
            }
        }
        #region Message
        private async Task ShowMessage1(string Title, string Note)
        {
            if (IsDialogOpen)
                IsDialogOpen = false;
            await ShowMessage(Title, Note).ContinueWith(_ => {
                if (!IsDialogOpen)
                    IsDialogOpen = true;
            });
        }

        private async Task ShowMessage(string Title, string Note)
        {
            await window.ShowMessageAsync(Title, Note);
        }
        #endregion
    }
}
