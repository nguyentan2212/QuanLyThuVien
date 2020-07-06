using AForge.Video;
using AForge.Video.DirectShow;
using Caliburn.Micro;
using Library_Management.Models;
using Library_Management.Views.UserControls;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZXing;

namespace Library_Management.ViewModels.MainPages
{
    public class ImportBookViewModel : ViewBaseModel
    {
        #region New Book Properties

        private TACGIA _Author;
        private BindableCollection<TACGIA> _AuthorList;
        private SACH _Book;

        private CTSACH _BookDetail;

        private THELOAI _Category;

        private BindableCollection<THELOAI> _CategoryList;

        private UserControl _DialogContent;

        private bool _IsDialogOpen;

        private NHAXUATBAN _Publisher;

        private BindableCollection<NHAXUATBAN> _PublisherList;

        public TACGIA Author
        {
            get { return _Author; }
            set { _Author = value; NotifyOfPropertyChange("Author"); }
        }

        public BindableCollection<TACGIA> AuthorList
        {
            get { return _AuthorList; }
            set { _AuthorList = value; NotifyOfPropertyChange("AuthorList"); }
        }

        public SACH Book
        {
            get { return _Book; }
            set { _Book = value; NotifyOfPropertyChange("Book"); }
        }
        public CTSACH BookDetail
        {
            get { return _BookDetail; }
            set { _BookDetail = value; NotifyOfPropertyChange("BookDetail"); }
        }
        public THELOAI Category
        {
            get { return _Category; }
            set { _Category = value; NotifyOfPropertyChange("Category"); }
        }
        public BindableCollection<THELOAI> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; NotifyOfPropertyChange("CategoryList"); }
        }

        public UserControl DialogContent
        {
            get { return _DialogContent; }
            set { _DialogContent = value; NotifyOfPropertyChange("DialogContent"); }
        }

        public bool IsDialogOpen
        {
            get { return _IsDialogOpen; }
            set { _IsDialogOpen = value; NotifyOfPropertyChange("IsDialogOpen"); }
        }

        public NHAXUATBAN Publisher
        {
            get { return _Publisher; }
            set { _Publisher = value; NotifyOfPropertyChange("Publisher"); }
        }
        public BindableCollection<NHAXUATBAN> PublisherList
        {
            get { return _PublisherList; }
            set { _PublisherList = value; NotifyOfPropertyChange("PublisherList"); }
        }
        #endregion New Book Properties

        #region ImportBill Properties

        private int _Amount;
        private decimal _CoverPrice;
        private BindableCollection<SACH> _CurrentBookList;
        private BindableCollection<ImportItem> _ImportList;

        private decimal _ImportPrice;

        private string _SearchString;

        private SACH _SelectedBook;

        private int _TotalBooks;

        private decimal _TotalPrice;

        public int Amount
        {
            get { return _Amount; }
            set { _Amount = value; NotifyOfPropertyChange("Amount"); }
        }

        public decimal CoverPrice
        {
            get { return _CoverPrice; }
            set { _CoverPrice = value; NotifyOfPropertyChange("CoverPrice"); }
        }

        public BindableCollection<SACH> CurrentBookList
        {
            get { return _CurrentBookList; }
            set { _CurrentBookList = value; NotifyOfPropertyChange("CurrentBookList"); }
        }

        public BindableCollection<ImportItem> ImportList
        {
            get { return _ImportList; }
            set { _ImportList = value; NotifyOfPropertyChange("ImportList"); }
        }
        public decimal ImportPrice
        {
            get { return _ImportPrice; }
            set { _ImportPrice = value; NotifyOfPropertyChange("ImportPrice"); }
        }

        public string SearchString
        {
            get { return _SearchString; }
            set
            {
                _SearchString = value;
                CurrentBookList = dataProvider.GetBookList(x => x.MASACH.ToString().Contains(SearchString) || x.TENSACH.ToUpper().Contains(SearchString) || x.TACGIA.TACGIA1.ToUpper().Contains(SearchString) ||
                                                             x.THELOAI.THELOAI1.ToUpper().Contains(SearchString) || x.NHAXUATBAN.NHAXUATBAN1.ToUpper().Contains(SearchString));
                NotifyOfPropertyChange("SearchString");
            }
        }

        public SACH SelectedBook
        {
            get { return _SelectedBook; }
            set
            {
                if (_SelectedBook != value)
                {
                    Amount = 0;
                    CoverPrice = 0;
                    ImportPrice = 0;
                }
                _SelectedBook = value;
                NotifyOfPropertyChange("SelectedBook");
            }
        }
        public int TotalBooks
        {
            get { return _TotalBooks; }
            set { _TotalBooks = value; NotifyOfPropertyChange("TotalBooks"); }
        }
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; NotifyOfPropertyChange("TotalPrice"); }
        }
        #endregion ImportBill Properties

        #region History Properties

        private DateTime _BeginDate;
        private DateTime _EndDate;
        private BindableCollection<ImportItem> _HistoryDetailList;
        private BindableCollection<PHIEUNHAPSACH> _HistoryList;
        private string _HistorySearchString;

        private PHIEUNHAPSACH _SelectedImportBill;

        public DateTime BeginDate
        {
            get { return _BeginDate; }
            set
            {
                _BeginDate = value;
                NotifyOfPropertyChange("BeginDate");
            }
        }

        public DateTime EndDate
        {
            get { return _EndDate; }
            set
            {
                _EndDate = value;
                NotifyOfPropertyChange("EndDate");
            }
        }

        public BindableCollection<ImportItem> HistoryDetailList
        {
            get { return _HistoryDetailList; }
            set { _HistoryDetailList = value; NotifyOfPropertyChange("HistoryDetailList"); }
        }

        public BindableCollection<PHIEUNHAPSACH> HistoryList
        {
            get { return _HistoryList; }
            set { _HistoryList = value; NotifyOfPropertyChange("HistoryList"); }
        }

        public string HistorySearchString
        {
            get { return _HistorySearchString; }
            set
            {
                _HistorySearchString = value;
                NotifyOfPropertyChange("HistorySearchString");
            }
        }
        public PHIEUNHAPSACH SelectedImportBill
        {
            get { return _SelectedImportBill; }
            set
            {
                if (_SelectedImportBill != value && value != null)
                {
                    HistoryDetailList = new BindableCollection<ImportItem>(value.CTSACH.GroupBy(x => new { x.MASACH, x.GIANHAP, x.GIABIA }).Select(p => new ImportItem()
                    {
                        BookId = p.Key.MASACH ?? 0,
                        Amount = p.Count(),
                        BookTitle = dataProvider.GetItem<SACH>(u => u.MASACH == p.Key.MASACH).TENSACH,
                        CoverPrice = p.Key.GIABIA ?? 0,
                        ImportPrice = p.Key.GIANHAP ?? 0,
                        TotalPrice = p.Sum(i => i.GIANHAP) ?? 0
                    }));
                }
                _SelectedImportBill = value;
                NotifyOfPropertyChange("SelectedImportBill");
            }
        }
        #endregion History Properties

        public ImportBookViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            eventAggregator.Subscribe(this);
            PublishRange = dataProvider.LibraryRules.KHOANGCACHXB ?? 10;
            AuthorList = dataProvider.AuthorList;
            CategoryList = dataProvider.CategoryList;
            PublisherList = dataProvider.PublisherList;
            Book = new SACH() { ANHBIA = @"/Resources/Images/Book/DefaultBook.jpg" };
            CurrentBookList = dataProvider.GetBookList(x => true);
            HistoryList = dataProvider.GetImportBillList(x => true);
            ImportList = new BindableCollection<ImportItem>();
            BeginDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        public int PublishRange { get; set; }
        #region Dialog properties

        private string _AuthorString;

        private string _CategoryString;

        private string _PublisherString;

        private TACGIA _SelectedAuthor;

        private THELOAI _SelectedCategory;

        private NHAXUATBAN _SelectedPublisher;

        public string AuthorString
        {
            get { return _AuthorString; }
            set { _AuthorString = value; NotifyOfPropertyChange("AuthorString"); }
        }
        public string CategoryString
        {
            get { return _CategoryString; }
            set
            { _CategoryString = value; NotifyOfPropertyChange("CategoryString"); }
        }

        public string PublisherString
        {
            get { return _PublisherString; }
            set { _PublisherString = value; NotifyOfPropertyChange("PublisherString"); }
        }

        public TACGIA SelectedAuthor
        {
            get { return _SelectedAuthor; }
            set
            {
                _SelectedAuthor = value;
                AuthorString = (value is null) ? null : value.TACGIA1;
                NotifyOfPropertyChange("SelectedAuthor");
            }
        }
        public THELOAI SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                _SelectedCategory = value;
                CategoryString = (value is null) ? null : value.THELOAI1;
                NotifyOfPropertyChange("SelectedCategory");
            }
        }
        public NHAXUATBAN SelectedPublisher
        {
            get { return _SelectedPublisher; }
            set
            {
                _SelectedPublisher = value;
                PublisherString = (value is null) ? null : value.NHAXUATBAN1;
                NotifyOfPropertyChange("SelectedPublisher");
            }
        }

        #endregion Dialog properties

        #region Dialog methods

        public void Create(string s)
        {
            bool IsSuccess = false;
            switch (s)
            {
                case "Author":
                    IsSuccess = dataProvider.Create<TACGIA>(new TACGIA() { TACGIA1 = AuthorString });
                    AuthorList = dataProvider.GetList<TACGIA>();
                    break;

                case "Category":
                    IsSuccess = dataProvider.Create<THELOAI>(new THELOAI() { THELOAI1 = CategoryString });
                    CategoryList = dataProvider.GetList<THELOAI>();
                    break;

                default:
                    IsSuccess = dataProvider.Create<NHAXUATBAN>(new NHAXUATBAN() { NHAXUATBAN1 = PublisherString });
                    PublisherList = dataProvider.GetList<NHAXUATBAN>();
                    break;
            }
            if (IsSuccess)
            {
                ShowMessage(new Models.Message("Thông báo", "Tạo mới thành công"));
            }
            else
            {
                ShowMessage(new Models.Message("Thông báo", "Tạo mới không thành công"));
            }
        }

        public void Delete(string s)
        {
            bool IsSuccess = false;
            switch (s)
            {
                case "Author":
                    if (dataProvider.GetItem<SACH>(g => g.MATG == SelectedAuthor.MATG) is null)
                    {
                        IsSuccess = dataProvider.Delete<TACGIA>(g => g.MATG == SelectedAuthor.MATG);
                        AuthorList = dataProvider.GetList<TACGIA>();
                    }
                    break;

                case "Category":
                    if (dataProvider.GetItem<SACH>(g => g.MATL == SelectedCategory.MATL) is null)
                    {
                        IsSuccess = dataProvider.Delete<THELOAI>(g => g.MATL == SelectedCategory.MATL);
                        CategoryList = dataProvider.GetList<THELOAI>();
                    }
                    break;

                default:
                    if (dataProvider.GetItem<SACH>(g => g.MANXB == SelectedPublisher.MANXB) is null)
                    {
                        IsSuccess = dataProvider.Delete<NHAXUATBAN>(g => g.MANXB == SelectedPublisher.MANXB);
                        PublisherList = dataProvider.GetList<NHAXUATBAN>();
                    }
                    break;
            }
            if (IsSuccess)
            {
                ShowMessage(new Models.Message("Thông báo", "Xóa thành công"));
            }
            else
            {
                ShowMessage(new Models.Message("Thông báo", "Xóa không thành công"));
            }
        }

        public void Exit()
        {
            IsDialogOpen = false;
        }

        public void OpenDialog(string s)
        {
            switch (s)
            {
                case "Author":
                    AuthorDialog ad = new AuthorDialog();
                    ad.DataContext = this;
                    DialogContent = ad;
                    break;

                case "Category":
                    CategoryDialog cd = new CategoryDialog();
                    cd.DataContext = this;
                    DialogContent = cd;
                    break;

                default:
                    PublisherDialog pd = new PublisherDialog();
                    pd.DataContext = this;
                    DialogContent = pd;
                    break;
            }
            IsDialogOpen = true;
        }
        public void Update(string s)
        {
            bool IsSuccess = false;
            switch (s)
            {
                case "Author":
                    SelectedAuthor.TACGIA1 = AuthorString;
                    IsSuccess = dataProvider.Update<TACGIA>(SelectedAuthor, g => g.MATG == SelectedAuthor.MATG);
                    break;

                case "Category":
                    SelectedCategory.THELOAI1 = CategoryString;
                    IsSuccess = dataProvider.Update<THELOAI>(SelectedCategory, g => g.MATL == SelectedCategory.MATL);
                    break;

                default:
                    SelectedPublisher.NHAXUATBAN1 = PublisherString;
                    IsSuccess = dataProvider.Update<NHAXUATBAN>(SelectedPublisher, g => g.MANXB == SelectedPublisher.MANXB);
                    break;
            }
        }

        #endregion Dialog methods     

        #region NewBook Methods

        public void Cancel()
        {
            Book = new SACH() { ANHBIA = @"/Resources/Images/Book/DefaultBook.jpg" };
            BookDetail = new CTSACH();
            Author = null;
            Category = null;
            Publisher = null;

        }

        public void ChangeImage()
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.InitialDirectory = @"C:\";
            openFile.RestoreDirectory = true;
            openFile.Title = "Đổi ảnh bìa";
            openFile.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg";
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.Multiselect = false;
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Book.ANHBIA = openFile.FileName;
            }
        }

        public void ImportNewBook()
        {
            bool IsSuccess;
            Book.MATG = Author.MATG;
            Book.MATL = Category.MATL;
            Book.MANXB = Publisher.MANXB;
            IsSuccess = dataProvider.Create<SACH>(Book);
            if (IsSuccess)
            {
                ShowMessage(new Models.Message("Thông báo", "Nhập thành công"));
            }
            else
            {
                ShowMessage(new Models.Message("Thông báo", "Nhập không thành công"));
            }
            CurrentBookList = dataProvider.GetBookList(x => true);
            SearchString = "";
        }
        #endregion NewBook Methods

        #region ImportBill Methods

        public void AddBookToBill()
        {
            if (SelectedBook is null)
            {
                ShowMessage(new Models.Message("Thông báo", "Chưa chọn sách"));              
                return;
            }
            ImportItem item = new ImportItem(SelectedBook, Amount, CoverPrice, ImportPrice);
            ImportItem check = ImportList.FirstOrDefault(x => x.BookId == item.BookId && x.CoverPrice == item.CoverPrice && x.ImportPrice == item.ImportPrice);
            if (check is null)
                ImportList.Add(item);
            else
                check.Amount += item.Amount;
            TotalBooks += item.Amount;
            TotalPrice += item.TotalPrice;
        }

        public void Import()
        {
            if (ImportList.Any() == false)
            {
                ShowMessage(new Models.Message("Thông báo", "Không có sách trong Phiếu nhập sách"));
                return;
            }
            PHIEUNHAPSACH p = new PHIEUNHAPSACH() { MATK = dataProvider.User.MATK, NGAYLAP = DateTime.Today };
            dataProvider.Create<PHIEUNHAPSACH>(p);
            foreach (ImportItem item in ImportList)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    CTSACH c = new CTSACH() { MASACH = item.BookId, MAPNS = p.MAPNS, GIABIA = item.CoverPrice, GIANHAP = item.ImportPrice, MATT = 1 };
                    dataProvider.Create<CTSACH>(c);
                }
            }
            HistoryList = dataProvider.GetImportBillList(x => true);
            ImportList = new BindableCollection<ImportItem>();
            ShowMessage(new Models.Message("Thông báo", "Đã nhập thành công"));          
        }

        public void Remove(ImportItem item)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Bạn có muốn xóa phần tử này hay không?", "Cảnh báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                ImportList.Remove(item);
                UpdateValue();
            }
        }

        public void UpdateValue()
        {
            TotalBooks = 0;
            TotalPrice = 0;
            foreach (ImportItem item in ImportList)
            {
                TotalBooks += item.Amount;
                TotalPrice += item.TotalPrice;
            }
        }
        #endregion ImportBill Methods

        #region History Methods

        public void GetDetailList()
        {
        }

        public void HistorySearch()
        {
            
            if (string.IsNullOrEmpty(HistorySearchString))
            {
                HistoryList = dataProvider.GetImportBillList(x => x.NGAYLAP >= BeginDate && x.NGAYLAP <= EndDate);
                return;
            }
            HistoryList = dataProvider.GetImportBillList(x => (x.TAIKHOAN.TAIKHOAN1.ToUpper().Contains(HistorySearchString.ToUpper()) || x.TAIKHOAN.HOTEN.ToUpper().Contains(HistorySearchString)) &&
                                                               x.NGAYLAP >= BeginDate && x.NGAYLAP <= EndDate);
        }
        #endregion History Methods

        private void ShowMessage(Models.Message mess)
        {
            MessageBox.Show(mess.Note, mess.Title);
        }
    }
}