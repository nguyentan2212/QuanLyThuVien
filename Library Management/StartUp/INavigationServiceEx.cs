using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Library_Management.StartUp
{
    public interface INavigationServiceEx : INavigationService
    {
        void NavigateToViewModel<TViewModel>(string context, object extraData = null);
        void NavigateToViewModel(Type viewModel, string context, object extraData = null);
    }
    public class NavigationService : FrameAdapter, INavigationServiceEx
    {
        readonly Frame frame;
        public NavigationService(Frame frame, bool treatViewAsLoaded = false) : base(frame, treatViewAsLoaded)
        {
            this.frame = frame;
        }
        public void NavigateToViewModel<TViewModel>(string context, object extraData = null)
        {
            NavigateToViewModel(typeof(TViewModel), context, extraData);
        }

        public void NavigateToViewModel(Type viewModel, string context, object extraData = null)
        {
            var viewType = ViewLocator.LocateTypeForModelType(viewModel, null, context);

            var packUri = ViewLocator.DeterminePackUriFromType(viewModel, viewType);

            var uri = new Uri(packUri, UriKind.Relative);

            frame.Navigate(uri, extraData);
        }
    }
}
