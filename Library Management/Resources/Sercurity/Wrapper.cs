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
    public class Wrapper: DependencyObject
    {
        public static readonly DependencyProperty MaxAgeProperty =
             DependencyProperty.Register("MaxAge", typeof(int),
             typeof(Wrapper), new FrameworkPropertyMetadata(100));

        public int MaxAge
        {
            get { return (int)GetValue(MaxAgeProperty); }
            set { SetValue(MaxAgeProperty, value); }
        }

        public static readonly DependencyProperty MinAgeProperty =
             DependencyProperty.Register("MinAge", typeof(int),
             typeof(Wrapper), new FrameworkPropertyMetadata(0));

        public int MinAge
        {
            get { return (int)GetValue(MinAgeProperty); }
            set { SetValue(MinAgeProperty, value); }
        }

        public static readonly DependencyProperty ClientAccountListProperty =
            DependencyProperty.Register("ClientAccountList", typeof(BindableCollection<LOAIDOCGIA>),
            typeof(Wrapper), new FrameworkPropertyMetadata(null));
        public BindableCollection<LOAIDOCGIA> ClientAccountList
        {
            get { return (BindableCollection<LOAIDOCGIA>)GetValue(ClientAccountListProperty); }
            set { SetValue(ClientAccountListProperty, value); }
        }
    }
}
