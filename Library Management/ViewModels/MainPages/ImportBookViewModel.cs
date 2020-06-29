using AForge.Video;
using AForge.Video.DirectShow;
using Caliburn.Micro;
using Library_Management.Models;
using Library_Management.Views.MainPages;
using Library_Management.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        private SACH _Book;
        public SACH Book
        {
            get { return _Book; }
            set { _Book = value; NotifyOfPropertyChange("Book"); }
        }
        private CTSACH _BookDetail;
        public CTSACH BookDetail
        {
            get { return _BookDetail; }
            set { _BookDetail = value; NotifyOfPropertyChange("BookDetail"); }
        }
        private TACGIA _Author;
        public TACGIA Author
        {
            get { return _Author; }
            set { _Author = value; NotifyOfPropertyChange("Author"); }
        }
        private THELOAI _Category;
        public THELOAI Category
        {
            get { return _Category; }
            set { _Category = value; NotifyOfPropertyChange("Category"); }
        }
        private NHAXUATBAN _Publisher;
        public NHAXUATBAN Publisher
        {
            get { return _Publisher; }
            set { _Publisher = value; NotifyOfPropertyChange("Publisher"); }
        }
        private BindableCollection<THELOAI> _CategoryList;
        public BindableCollection<THELOAI> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; NotifyOfPropertyChange("CategoryList"); }
        }
        private BindableCollection<TACGIA> _AuthorList;
        public BindableCollection<TACGIA> AuthorList
        {
            get { return _AuthorList; }
            set { _AuthorList = value; NotifyOfPropertyChange("AuthorList"); }
        }
        private BindableCollection<NHAXUATBAN> _PublisherList;
        public BindableCollection<NHAXUATBAN> PublisherList
        {
            get { return _PublisherList; }
            set { _PublisherList = value; NotifyOfPropertyChange("PublisherList"); }
        }
        private UserControl _DialogContent;
        public UserControl DialogContent
        {
            get { return _DialogContent; }
            set { _DialogContent = value; NotifyOfPropertyChange("DialogContent"); }
        }
        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get { return _IsDialogOpen; }
            set { _IsDialogOpen = value; NotifyOfPropertyChange("IsDialogOpen"); }
        }
        #endregion
        #region ImportBill Properties

        private BindableCollection<ImportItem> _ImportList;
        public BindableCollection<ImportItem> ImportList
        {
            get { return _ImportList; }
            set { _ImportList = value; NotifyOfPropertyChange("ImportList"); }
        }

        private BindableCollection<SACH> _CurrentBookList;
        public BindableCollection<SACH> CurrentBookList
        {
            get { return _CurrentBookList; }
            set { _CurrentBookList = value; NotifyOfPropertyChange("CurrentBookList"); }
        }

        private SACH _SelectedBook;
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
                NotifyOfPropertyChange("SelectedBook"); }
        }

        private int _Amount;
        public int Amount
        {
            get { return _Amount; }
            set { _Amount = value; NotifyOfPropertyChange("Amount"); }
        }

        private decimal _CoverPrice;
        public decimal CoverPrice
        {
            get { return _CoverPrice; }
            set { _CoverPrice = value; NotifyOfPropertyChange("CoverPrice"); }
        }

        private decimal _ImportPrice;
        public decimal ImportPrice
        {
            get { return _ImportPrice; }
            set { _ImportPrice = value; NotifyOfPropertyChange("ImportPrice"); }
        }

        private int _TotalBooks;
        public int TotalBooks
        {
            get { return _TotalBooks; }
            set { _TotalBooks = value; NotifyOfPropertyChange("TotalBooks"); }
        }

        private decimal _TotalPrice;
        public decimal TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; NotifyOfPropertyChange("TotalPrice"); }
        }

        private string _SearchString;
        public string SearchString
        {
            get { return _SearchString; }
            set 
            { 
                _SearchString = value; 
                CurrentBookList = dataProvider.GetBookList(x => x.MASACH.ToString().Contains(SearchString) || x.TENSACH.ToUpper().Contains(SearchString) || x.TACGIA.TACGIA1.ToUpper().Contains(SearchString) ||
                                                             x.THELOAI.THELOAI1.ToUpper().Contains(SearchString) || x.NHAXUATBAN.NHAXUATBAN1.ToUpper().Contains(SearchString));
                NotifyOfPropertyChange("SearchString"); }
        }

        #endregion
        #region History Properties

        private string _HistorySearchString;
        public string HistorySearchString
        {
            get { return _HistorySearchString; }
            set 
            {               
                _HistorySearchString = value; 
                NotifyOfPropertyChange("HistorySearchString"); }
        }

        private DateTime _BeginDate;
        public DateTime BeginDate
        {
            get { return _BeginDate; }
            set 
            {                
                _BeginDate = value;
                NotifyOfPropertyChange("BeginDate"); 
            }
        }

        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set 
            {                
                _EndDate = value; 
                NotifyOfPropertyChange("EndDate"); 
            }
        }

        private PHIEUNHAPSACH _SelectedImportBill;
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

        private BindableCollection<PHIEUNHAPSACH> _HistoryList;
        public BindableCollection<PHIEUNHAPSACH> HistoryList
        {
            get { return _HistoryList; }
            set { _HistoryList = value; NotifyOfPropertyChange("HistoryList"); }
        }

        private BindableCollection<ImportItem> _HistoryDetailList;
        public BindableCollection<ImportItem> HistoryDetailList
        {
            get { return _HistoryDetailList; }
            set { _HistoryDetailList = value; NotifyOfPropertyChange("HistoryDetailList"); }
        }
        #endregion
        public int PublishRange { get; set; }
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
        #region Dialog properties

        private string _AuthorString;
        public string AuthorString
        {
            get { return _AuthorString; }
            set { _AuthorString = value; NotifyOfPropertyChange("AuthorString"); }
        }

        private TACGIA _SelectedAuthor;
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

        private string _CategoryString;
        public string CategoryString
        {
            get { return _CategoryString; }
            set 
            { _CategoryString = value; NotifyOfPropertyChange("CategoryString"); }
        }

        private THELOAI _SelectedCategory;
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

        private string _PublisherString;
        public string PublisherString
        {
            get { return _PublisherString; }
            set { _PublisherString = value; NotifyOfPropertyChange("PublisherString"); }
        }

        private NHAXUATBAN _SelectedPublisher;
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

        #endregion
        #region Dialog methods
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
        public void Update(string s)
        {            
            bool IsSuccess = false;
            switch (s)
            {
                case "Author":
                    SelectedAuthor.TACGIA1 = AuthorString;
                    IsSuccess = dataProvider.Update<TACGIA>(SelectedAuthor,g => g.MATG == SelectedAuthor.MATG);                    
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
        #endregion
        #region QRCode properties and methods 
        private Bitmap _bitmap;
        private IVideoSource _videoSource;
        private FilterInfo _currentDevice;       
        private BindableCollection<FilterInfo> _videoDevices;
        
        private BitmapImage _BitImage;
        public BitmapImage BitImage
        {
            get { return _BitImage; }
            set { _BitImage = value; NotifyOfPropertyChange("BitImage"); }
        }
        private DispatcherTimer timer;
        private void GetVideoDevice()
        {
            _videoDevices = new BindableCollection<FilterInfo>();
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in devices)
            {
                _videoDevices.Add(device);
            }
            if (_videoDevices.Any())
            {
                _currentDevice = _videoDevices[0];
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Lỗi", "No webcam found"));
            }
        }
        public void StartCamera()
        {
            if (_currentDevice != null)
            {
                _videoSource = new VideoCaptureDevice(_currentDevice.MonikerString);
                _videoSource.NewFrame += video_NewFrame;
                _videoSource.Start();
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Lỗi", "Current device can't be null"));
            }
        }
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                var bitmap = (Bitmap)eventArgs.Frame.Clone();
                var bi = BitmapToImageSource(bitmap);
                bi.Freeze();
                BitImage = bi;
            }
            catch (Exception e)
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Lỗi", "Error on _videoSource_NewFrame:\n" + e.Message));
                StopCamera();
                IsDialogOpen = false;
            }
        }
        public void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= video_NewFrame;
            }
            BitImage = null;
            _bitmap = null;
        }
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;                
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }    
        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        private string _QRString;
        public string QRString
        {
            get { return _QRString; }
            set { _QRString = value; NotifyOfPropertyChange("QRString"); }
        }
       
        private void timer_Tick(object sender, EventArgs e)
        {
            string qrdecode = "";
            if (BitImage is null)
                return;
            Bitmap bit = BitmapImageToBitmap(BitImage);          
            BarcodeReader Reader = new BarcodeReader();
            Result qrResult = Reader.Decode(bit);
            try
            {
                qrdecode = qrResult?.Text.Trim();
            }
            catch (Exception ex)
            {
                qrdecode = "error";
            }
            QRString = qrdecode;
            if (!string.IsNullOrEmpty(qrdecode))
            {
                StopCamera();
                IsDialogOpen = false;
                int id = int.Parse(qrdecode);
                Book = dataProvider.GetItem<SACH>(g => g.MASACH == id);
                BookDetail = dataProvider.GetItem<CTSACH>(g => g.MASACH == id);
                Author = AuthorList.FirstOrDefault(g => g.MATG == Book.MATG);
                Category = CategoryList.FirstOrDefault(g => g.MATL == Book.MATL);
                Publisher = PublisherList.FirstOrDefault(g => g.MANXB == Book.MANXB);
                if (int.TryParse(Book.ANHBIA.Substring(0, 1),out int result) && result < 6)
                {
                    Book.ANHBIA = @"/Resources/Images/Book/" + Book.ANHBIA;
                }
            }
        }
        public void CloseQRDialog()
        {
            StopCamera();
            timer.Stop();
            IsDialogOpen = false;
        }
        #endregion
        #region NewBook Methods
        public void Cancel()
        {
            Book = new SACH() { ANHBIA = @"/Resources/Images/Book/DefaultBook.jpg" };
            BookDetail = new CTSACH();           
            Author = null;
            Category = null;
            Publisher = null;
            /*bool c = dataProvider.Delete<SACH>(g => g.MASACH == 18);
            if (c)
            {
                MessageBox.Show("Thanh cong");
            }*/
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
        #endregion
        #region ImportBill Methods
        public void AddBookToBill()
        {
            if (SelectedBook is null)
            {
                MessageBox.Show("Chưa chọn sách");
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
            if(ImportList.Any() == false)
            {
                MessageBox.Show("Không có sách trong Phiếu nhập sách");
                return;
            }
            PHIEUNHAPSACH p = new PHIEUNHAPSACH() { MATK = dataProvider.User.MATK, NGAYLAP = DateTime.Today };
            dataProvider.Create<PHIEUNHAPSACH>(p);
            foreach (ImportItem item in ImportList)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    CTSACH c = new CTSACH() { MASACH = item.BookId, MAPNS = p.MAPNS, GIABIA = item.CoverPrice, GIANHAP = item.ImportPrice };
                    dataProvider.Create<CTSACH>(c);
                }
            }
            HistoryList = dataProvider.GetImportBillList(x => true);
            ImportList = new BindableCollection<ImportItem>();
            MessageBox.Show("Đã nhập thành công");
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
        public void Remove(ImportItem item)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Bạn có muốn xóa phần tử này hay không?","Cảnh báo",MessageBoxButton.OKCancel,MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                ImportList.Remove(item);
                UpdateValue();
            }
        }
        #endregion
        #region History Methods
        public void HistorySearch()
        {
            /*foreach (CTSACH c in SelectedImportBill.CTSACH)
            {
                dataProvider.Delete<CTSACH>(x => x.MAPNS == SelectedImportBill.MAPNS);
            }
            dataProvider.Delete<PHIEUNHAPSACH>(x => x.MAPNS == SelectedImportBill.MAPNS);*/
            if (string.IsNullOrEmpty(HistorySearchString))
            {
                HistoryList = dataProvider.GetImportBillList(x => x.NGAYLAP >= BeginDate && x.NGAYLAP <= EndDate);
                return;
            }
            HistoryList = dataProvider.GetImportBillList(x => (x.TAIKHOAN.TAIKHOAN1.ToUpper().Contains(HistorySearchString.ToUpper()) || x.TAIKHOAN.HOTEN.ToUpper().Contains(HistorySearchString)) && 
                                                               x.NGAYLAP >= BeginDate && x.NGAYLAP <= EndDate);
        }
        public void GetDetailList()
        {
            
        }
        #endregion
        private void ShowMessage(Models.Message mess)
        {
            MessageBox.Show(mess.Note, mess.Title);
        }
    }
}
