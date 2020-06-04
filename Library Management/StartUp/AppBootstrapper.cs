using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library_Management.Models;
using Library_Management.ViewModels.MainPages;

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
            container = new SimpleContainer();
            container.Instance(container);
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<DataProvider>();
            container.PerRequest<ShellViewModel>();
            container.PerRequest<AccountViewModel>();
            container.PerRequest<ClientAccountViewModel>();
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
