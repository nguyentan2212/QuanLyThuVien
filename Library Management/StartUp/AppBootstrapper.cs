using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Caliburn.Micro;
using Library_Management.Models;
using Library_Management.ViewModels.MainPages;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Library_Management.StartUp
{
    public class AppBootstrapper : BootstrapperBase
    {
        SimpleContainer container;
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            string imgPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\LibrarianAccount";
            if (Directory.Exists(imgPath) == false)
            {
                Directory.CreateDirectory(imgPath);
            }
            imgPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\ClientAccount";
            if (Directory.Exists(imgPath) == false)
            {
                Directory.CreateDirectory(imgPath);
            }
            imgPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\Book";
            if (Directory.Exists(imgPath) == false)
            {
                Directory.CreateDirectory(imgPath);
            }
            container = new SimpleContainer();
            container.Instance(container);
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<DataProvider>();
            container.PerRequest<ShellViewModel>();
            container.PerRequest<AccountViewModel>();
            container.PerRequest<ClientAccountViewModel>();
            container.PerRequest<NewClientAccountViewModel>();
        }
        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
