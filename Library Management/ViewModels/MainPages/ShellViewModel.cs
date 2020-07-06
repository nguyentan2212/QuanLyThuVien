using Caliburn.Micro;
using Library_Management.Models;
using Library_Management.StartUp;
using Library_Management.Views.UserControls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Library_Management.ViewModels.MainPages
{
    public class ShellViewModel : ViewBaseModel, IHandle<Models.Message>, IHandle<string>
    {
        #region

        private SimpleContainer _container;
        private INavigationServiceEx navigationService;
        private Login Loginform = new Login();
        private MetroWindow window;

        #endregion

        #region UI Properties

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

        private string _DisplayUserName;

        public string DisplayUserName
        {
            get
            {
                return _DisplayUserName;
            }
            set
            {
                _DisplayUserName = value;
                NotifyOfPropertyChange("DisplayUserName");
            }
        }

        private string _FrameTitle;

        public string FrameTitle
        {
            get
            {
                return _FrameTitle;
            }
            set
            {
                _FrameTitle = value;
                NotifyOfPropertyChange("FrameTitle");
            }
        }

        private Visibility _IsWorking;

        public Visibility IsWorking
        {
            get
            {
                return _IsWorking;
            }
            set
            {
                _IsWorking = value;
                NotifyOfPropertyChange("IsWorking");
            }
        }

        private bool _IsLogin;

        public bool IsLogin
        {
            get
            {
                return _IsLogin;
            }
            set
            {
                _IsLogin = value;

                IsNotLogin = !IsLogin;
                NotifyOfPropertyChange("IsLogin");
            }
        }

        private bool _IsNotLogin;

        public bool IsNotLogin
        {
            get
            {
                return _IsNotLogin;
            }
            set
            {
                _IsNotLogin = value;

                NotifyOfPropertyChange("IsNotLogin");
            }
        }

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
            FrameTitle = "Quản Lý Thư Viện";
            DisplayUserName = "Khách";
            IsLogin = false;
            //NavigateToView("Home");
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
            //UserName = "tannguyen";
            //Password = "tannguyen";
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
                    DisplayUserName = dataProvider.User.HOTEN;
                    IsDialogOpen = false;
                    IsLogin = true;
                    navigationService.NavigateToViewModel<RecommentBookViewModel>();
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

        public async void Logout()
        {
            await ShowMessage("Thông báo", string.Concat("Tạm biệt, ", DisplayUserName));
            DisplayUserName = UserName = Password = string.Empty;
            this.dataProvider.User = null;
            IsLogin = false;
            DisplayUserName = "Khách";
            navigationService.NavigateToViewModel<RecommentBookViewModel>();
        }

        public void Exit()
        {
            IsDialogOpen = false;
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
            NavigateToView("Home");
        }

        public void NavigateToView(string name)
        {
            switch (name)
            {
                case "Account":
                    navigationService.NavigateToViewModel<AccountViewModel>();
                    FrameTitle = "Thông Tin Nhân Viên";
                    break;

                case "Home":
                    navigationService.NavigateToViewModel<RecommentBookViewModel>();
                    FrameTitle = "Sách Đề Nghị";
                    break;

                case "FindBook":
                    navigationService.NavigateToViewModel<BookSearchViewModel>();
                    FrameTitle = "Tìm Sách";
                    break;

                case "NewClientAccount":
                    navigationService.NavigateToViewModel<NewClientAccountViewModel>();
                    FrameTitle = "Quản Lý Độc Giả";
                    break;

                case "ImportBook":
                    navigationService.NavigateToViewModel<ImportBookViewModel>();
                    FrameTitle = "Nhập Sách";
                    break;
                case "Report":
                    navigationService.NavigateToViewModel<ReportViewModel>();
                    FrameTitle = "Báo cáo";
                    break;
                case "Setting":
                    navigationService.NavigateToViewModel<SettingViewModel>();
                    FrameTitle = "Quy Định Thư Viện";
                    break;
                case "Client":
                    navigationService.NavigateToViewModel<ClientDetailViewModel>();
                    FrameTitle = "Thông Tin Độc Giả";
                    break;
                case "About":
                    navigationService.NavigateToViewModel<AboutUsViewModel>();
                    FrameTitle = "Về Chúng Tôi";
                    break;
                default:
                    navigationService.For<BookDetailViewModel>().WithParam(x => x.IsLogin, this.IsLogin).Navigate();
                    FrameTitle = "Thông Tin Sách";
                    break;
            }
        }

        public void HamburgerMenu_ItemClick(ItemClickEventArgs e)
        {
            HamburgerMenuIconItem item = e.ClickedItem as HamburgerMenuIconItem;
            if (item.Tag.ToString() == "Logout")
                Logout();
            else if (item.Tag.ToString() == "Login")
                ShowLogin();
            else
                NavigateToView(item.Tag.ToString());
        }

        #region Message

        private async Task ShowMessage1(string Title, string Note)
        {
            if (IsDialogOpen)
                IsDialogOpen = false;
            await ShowMessage(Title, Note).ContinueWith(_ =>
            {
                if (!IsDialogOpen)
                    IsDialogOpen = true;
            });
        }

        private async Task ShowMessage(string Title, string Note)
        {
            await window.ShowMessageAsync(Title, Note);
        }

        public async void Handle(Models.Message message)
        {
            await window.ShowMessageAsync(message.Title, message.Note);
        }

        public void Handle(string message)
        {
            if (message == "UpdatedUserAccountSuccessful")
            {
                DisplayUserName = dataProvider.User.HOTEN;
            }
            else if (message == "OpenBookDetailView")
            {
                NavigateToView(message);
            }
            else if (message == "GoBack")
            {
                if (dataProvider.WhereToGoBack == 1)
                {
                    NavigateToView("FindBook");
                }
                else if (dataProvider.WhereToGoBack == 2)
                {
                    NavigateToView("Home");
                }
                else
                {
                    NavigateToView("NewClientAccount");
                }
            }
            else if (message == "OpenClientView")
            {
                NavigateToView("Client");
            }
        }

        #endregion
    }
}