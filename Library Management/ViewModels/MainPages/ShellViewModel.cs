using Caliburn.Micro;
using Library_Management.StartUp;
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
        #endregion

        #region Properties
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
                NotifyOfPropertyChange(() => DrawerOpen);
            }
        }
        #endregion

        public ShellViewModel(SimpleContainer container, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            this._container = container;
            eventAggregator.Subscribe(this);
        }

        public void RegisterFrame(Frame frame)
        {
            navigationService = new NavigationService(frame);

            _container.Instance(navigationService);
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
    }
}
