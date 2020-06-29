using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library_Management.Models;

namespace Library_Management.ViewModels.MainPages
{
    public class BookSearchViewModel: ViewBaseModel
    {
        #region Properties

        private BindableCollection<SACH> _BookList;
        public BindableCollection<SACH> BookList
        {
            get { return _BookList; }
            set { _BookList = value; NotifyOfPropertyChange("BookList"); }
        }

        private int _SearchFilterIndex;
        public int SearchFilterIndex
        {
            get { return _SearchFilterIndex; }
            set { _SearchFilterIndex = value; NotifyOfPropertyChange("SearchFilterIndex"); }
        }

        private string _SearchString;
        public string SearchString
        {
            get { return _SearchString; }
            set { _SearchString = value; NotifyOfPropertyChange("SearchString"); }
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

        private int _SearchOrderIndex;
        public int SearchOrderIndex
        {
            get { return _SearchOrderIndex; }
            set 
            { 
                if (value != _SearchOrderIndex)
                {
                    SortBookList(value);
                }
                _SearchOrderIndex = value; 
                NotifyOfPropertyChange("SearchOrderIndex"); 
            }
        }

        #endregion
        public BookSearchViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            eventAggregator.Subscribe(this);
            BookList = dataProvider.GetBookList(x => true);
            SortBookList(0);
            SearchString = "";
            dataProvider.WhereToGoBack = 1;
        }
        public void Search()
        {           
            if (SearchFilterIndex < 0)
            {
                SearchFilterIndex = 0;
            }
            if (SearchString is null)
            {
                SearchString = "";
            }
            SearchString = SearchString.ToUpper();
            BindableCollection<SACH> books;
            switch (SearchFilterIndex)
            {
                case 1:
                    books = dataProvider.GetBookList(x => x.TENSACH.ToUpper().Contains(SearchString));
                    break;
                case 2:
                    books = dataProvider.GetBookList(x => x.TACGIA.TACGIA1.ToUpper().Contains(SearchString));
                    break;
                case 3:
                    books = dataProvider.GetBookList(x => x.THELOAI.THELOAI1.ToUpper().Contains(SearchString));
                    break;
                case 4:
                    books = dataProvider.GetBookList(x => x.NHAXUATBAN.NHAXUATBAN1.ToUpper().Contains(SearchString));
                    break;                
                default:
                    books = dataProvider.GetBookList(x => x.TENSACH.ToUpper().Contains(SearchString) || x.TACGIA.TACGIA1.ToUpper().Contains(SearchString) || 
                                                             x.THELOAI.THELOAI1.ToUpper().Contains(SearchString) || x.NHAXUATBAN.NHAXUATBAN1.ToUpper().Contains(SearchString));
                    break;
            }
            BookList = books;
            SortBookList(SearchOrderIndex);
        }
        public void SortBookList(int order)
        {
            BindableCollection<SACH> books;
            switch (order)
            {
                case 1:
                    books = new BindableCollection<SACH>(BookList.OrderByDescending(x => x.TENSACH));
                    break;
                case 2:
                    books = new BindableCollection<SACH>(BookList.OrderBy(x => x.LUOTMUON));
                    break;
                case 3:
                    books = new BindableCollection<SACH>(BookList.OrderByDescending(x => x.LUOTMUON));
                    break;
                default:
                    books = new BindableCollection<SACH>(BookList.OrderBy(x => x.TENSACH));
                    break;
            }
            BookList = books;
        }
    }
}
