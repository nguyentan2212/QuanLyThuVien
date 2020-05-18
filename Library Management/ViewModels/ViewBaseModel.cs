using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management.ViewModels
{
    public class ViewBaseModel : Screen
    {
        internal readonly IEventAggregator eventAggregator;
        
        public ViewBaseModel()
        { }
        public ViewBaseModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }    
    }
}
