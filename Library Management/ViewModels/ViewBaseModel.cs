using Caliburn.Micro;
using Library_Management.Models;
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
        internal DataProvider dataProvider;
        public ViewBaseModel()
        { }
        public ViewBaseModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }
        public ViewBaseModel(DataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.dataProvider = dataProvider;            
        }
    }
}
