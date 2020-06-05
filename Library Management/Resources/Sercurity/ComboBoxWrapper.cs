using Caliburn.Micro;
using Library_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Library_Management.Resources.Sercurity
{
    class ComboBoxWrapper: DependencyObject
    {
        public static readonly DependencyProperty ListSourceProperty =
            DependencyProperty.Register("ListSource", typeof(BindableCollection<LOAIDOCGIA>),
            typeof(ComboBoxWrapper), new FrameworkPropertyMetadata(null));
        public BindableCollection<LOAIDOCGIA> ListSource
        {
            get { return (BindableCollection<LOAIDOCGIA>)GetValue(ListSourceProperty); }
            set { SetValue(ListSourceProperty, value); }
        }
    }
}
