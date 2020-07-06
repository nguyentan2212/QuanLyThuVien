using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library_Management.Models;

namespace Library_Management.ViewModels.MainPages
{
    public class RecommentBookViewModel:ViewBaseModel
    {
        #region Properties

        private BindableCollection<SACH> _NewBookList;
        public BindableCollection<SACH> NewBookList
        {
            get { return _NewBookList; }
            set { _NewBookList = value; NotifyOfPropertyChange("NewBookList"); }
        }

        private BindableCollection<SACH> _MostViewBookList;
        public BindableCollection<SACH> MostViewBookList
        {
            get { return _MostViewBookList; }
            set { _MostViewBookList = value; NotifyOfPropertyChange("MostViewBookList"); }
        }
        private SACH _SelectedBook;
        public SACH SelectedBook
        {
            get { return _SelectedBook; }
            set
            {
                _SelectedBook = value;
                dataProvider.SelectedBook = value;
                if (value != null)
                {
                    eventAggregator.PublishOnCurrentThread("OpenBookDetailView");
                }
                NotifyOfPropertyChange("SelectedBook");
            }
        }
        #endregion
        public RecommentBookViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            NewBookList = dataProvider.GetNewBookList();
            MostViewBookList = new BindableCollection<SACH>(dataProvider.GetBookList(x => true).OrderByDescending(y => y.LUOTMUON).Where(x => x.CTSACH != null && x.CTSACH.Count > 0));
            dataProvider.WhereToGoBack = 2;
        }
    }
}
