using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections;
using System.Data;
using Caliburn.Micro;
using Library_Management.Models;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

namespace Library_Management.ViewModels.MainPages
{
    public class ReportViewModel : ViewBaseModel
    {
        #region Properties

        private DateTime _BeginDate1;
        public DateTime BeginDate1
        {
            get { return _BeginDate1; }
            set { _BeginDate1 = value; NotifyOfPropertyChange("BeginDate1"); }
        }

        private DateTime _EndDate1;
        public DateTime EndDate1
        {
            get { return _EndDate1; }
            set { _EndDate1 = value; NotifyOfPropertyChange("EndDate1"); }
        }

        private DateTime _BeginDate2;
        public DateTime BeginDate2
        {
            get { return _BeginDate2; }
            set { _BeginDate2 = value; NotifyOfPropertyChange("BeginDate2"); }
        }

        private DateTime _EndDate2;
        public DateTime EndDate2
        {
            get { return _EndDate2; }
            set { _EndDate2 = value; NotifyOfPropertyChange("EndDate2"); }
        }

        private int _Sum;
        public int Sum
        {
            get { return _Sum; }
            set { _Sum = value; NotifyOfPropertyChange("Sum"); }
        }

        private BindableCollection<ReportItem1> _List1;
        public BindableCollection<ReportItem1> List1
        {
            get { return _List1; }
            set { _List1 = value; NotifyOfPropertyChange("List1"); }
        }

        private BindableCollection<ReportItem2> _List2;
        public BindableCollection<ReportItem2> List2
        {
            get { return _List2; }
            set { _List2 = value; NotifyOfPropertyChange("List2"); }
        }

        #endregion
        public ReportViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            BeginDate1 = EndDate1 = BeginDate2 = EndDate2 = DateTime.Today;
        }
        public void CreateReport1()
        {
            List1 = dataProvider.Report1(BeginDate1, EndDate1);
            Sum = List1.Sum(x => x.SOLUOT);
            int t = 0;
            foreach (ReportItem1 item in List1)
            {
                item.STT = ++t;
                item.TILE = (double)item.SOLUOT / Sum;
            }
        }
        public void CreateReport2()
        {
            int t = 0;
            List2 = dataProvider.Report2(BeginDate2, EndDate2);
            foreach (ReportItem2 item in List2)
            {
                item.STT = ++t;
                item.SONGAYTRE = (item.NGAYTRA - item.NGAYMUON).Days;
            }
        }
       
    }
}
