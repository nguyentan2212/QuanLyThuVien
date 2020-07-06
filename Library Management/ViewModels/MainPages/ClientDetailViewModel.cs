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
    public class ClientDetailViewModel : ViewBaseModel
    {
        #region Properties

        private DOCGIA _Client;

        public DOCGIA Client
        {
            get { return _Client; }
            set { _Client = value; NotifyOfPropertyChange("Client"); }
        }

        private BindableCollection<LOAIDOCGIA> _ClientTypeList;

        public BindableCollection<LOAIDOCGIA> ClientTypeList
        {
            get { return _ClientTypeList; }
            set { _ClientTypeList = value; NotifyOfPropertyChange("ClientTypeList"); }
        }

        private string _BookDetailSearchString;

        public string BookDetailSearchString
        {
            get { return _BookDetailSearchString; }
            set { _BookDetailSearchString = value; NotifyOfPropertyChange("BookDetailSearchString"); }
        }

        private int _Amount;

        public int Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                if (_Amount < 0)
                {
                    _Amount = 0;
                }
                NotifyOfPropertyChange("Amount");
            }
        }

        private BindableCollection<CTSACH> _BorrowList;

        public BindableCollection<CTSACH> BorrowList
        {
            get { return _BorrowList; }
            set { _BorrowList = value; NotifyOfPropertyChange("BorrowList"); }
        }

        private BindableCollection<PHIEUMUONSACH> _BorrowBillList;

        public BindableCollection<PHIEUMUONSACH> BorrowBillList
        {
            get { return _BorrowBillList; }
            set { _BorrowBillList = value; NotifyOfPropertyChange("BorrowBillList"); }
        }

        private BindableCollection<CTPTS> _ReturnBookBill;
        public BindableCollection<CTPTS> ReturnBookBill
        {
            get { return _ReturnBookBill; }
            set { _ReturnBookBill = value; NotifyOfPropertyChange("ReturnBookBill"); }
        }
       
        private BindableCollection<CTPTS> _MyList;
        public BindableCollection<CTPTS> MyList
        {
            get { return _MyList; }
            set { _MyList = value; NotifyOfPropertyChange("MyList"); }
        }

        private decimal _SumMoney;
        public decimal SumMoney
        {
            get { return _SumMoney; }
            set { _SumMoney = value; NotifyOfPropertyChange("SumMoney"); }
        }

        private decimal _Money;
        public decimal Money
        {
            get { return _Money; }
            set { _Money = value; NotifyOfPropertyChange("Money"); }
        }

        private LOAIDOCGIA _LoaiDocGia;
        public LOAIDOCGIA LoaiDocGia
        {
            get { return _LoaiDocGia; }
            set { _LoaiDocGia = value; NotifyOfPropertyChange("LoaiDocGia"); }
        }
        #endregion Properties

        public ClientDetailViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            Client = dataProvider.SelectedClient;
            if (string.IsNullOrEmpty(Client.HINHANH) == true || File.Exists(Client.HINHANH) == false)
                Client.HINHANH = @"/Resources/Images/ClientAccount/DefaultAccount.png";
            ClientTypeList = dataProvider.GetList<LOAIDOCGIA>();
            LoaiDocGia = ClientTypeList.FirstOrDefault(x => x.MALDG == Client.MALDG);
            Client.LOAIDOCGIA = ClientTypeList.FirstOrDefault(x => x.MALDG == Client.MALDG);
            BorrowList = new BindableCollection<CTSACH>();
            BorrowBillList = dataProvider.GetPMS(x => x.MADG == Client.MADG);
            Amount = dataProvider.LibraryRules.SACHMUONTOIDA ?? 5;
            foreach (PHIEUMUONSACH item in BorrowBillList)
            {
                Amount -= item.CTSACH.Count - item.CTPTS.Count;
            }
            MyList = dataProvider.ReturnBookList(Client);
            ReturnBookBill = new BindableCollection<CTPTS>();
        }

        #region Borrow Book Method
        public void Delete(CTSACH cs)
        {
            BorrowList.Remove(cs);
        }
        public void AddBook()
        {
            AddToBorrow(BookDetailSearchString);
        }
        public void AddToBorrow(string s)
        {
            if (Client.NGLAPTHE?.AddMonths(dataProvider.LibraryRules.THOIHANTHE ?? 0) < DateTime.Today)
            {
                MessageBox.Show("Thẻ độc giả quá hạn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Client.TIENNO > dataProvider.LibraryRules.TIENPHATTOIDA)
            {
                eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tiền nợ vượt mức tối đa! Hãy thanh toán trước khi mượn sách mới."));
                return;
            }
            CTSACH temp = dataProvider.GetBookDetailList(x => x.MACTS.ToString() == s).FirstOrDefault();
            if (temp is null)
            {
                eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Không tìm thấy!"));
                return;
            }
            if (Amount == BorrowList.Count)
            {
                eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Không thể mượn thêm được nữa!"));
                return;
            }
            if (temp.MATT != 1)
            {
                eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Không thể mượn!"));
                return;
            }
            if (BorrowList.FirstOrDefault(x => x.MACTS == temp.MACTS) is null)
            {
                BorrowList.Add(temp);
            }
            else
            {
                eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Sách đã có trong đơn!"));
            }
        }
        public void CreateBorrowBookBill()
        {
            PHIEUMUONSACH temp = new PHIEUMUONSACH() { MADG = Client.MADG, NGMUON = DateTime.Today, NGUOILAP = dataProvider.User.MATK };
            if (dataProvider.Create<PHIEUMUONSACH>(temp))
            {
                foreach (CTSACH item in BorrowList)
                {
                    item.MATT = 3;
                    dataProvider.UpdateCTS(item, temp);
                }
            }
            BorrowList = new BindableCollection<CTSACH>();
            BorrowBillList = dataProvider.GetPMS(x => x.MADG == Client.MADG);
            Amount = dataProvider.LibraryRules.SACHMUONTOIDA ?? 5;
            foreach (PHIEUMUONSACH item in BorrowBillList)
            {
                Amount -= item.CTSACH.Count - item.CTPTS.Count;
            }
            MyList = dataProvider.ReturnBookList(Client);
            ReturnBookBill = new BindableCollection<CTPTS>();
            eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo phiếu mượn sách thành công"));
        }

        #endregion Borrow And Return Book Method
        #region Return Book
        public void Add(CTPTS t)
        {
            ReturnBookBill.Add(t);
            MyList.Remove(t);
            SumMoney += t.TIENPHAT ?? 0;
        }
        public void Remove(CTPTS t)
        {
            MyList.Add(t);
            ReturnBookBill.Remove(t);
            SumMoney -= t.TIENPHAT ?? 0;
        }
        public void CreateReturnBill()
        {
            PHIEUTRASACH t = new PHIEUTRASACH() { MADG = Client.MADG, NGTRASACH = DateTime.Today, NGUOILAP = dataProvider.User.MATK };
            dataProvider.Create<PHIEUTRASACH>(t);
            foreach(CTPTS item in ReturnBookBill)
            {
                item.MAPTS = t.MAPTS;
                if ((dataProvider.GetItem<PHIEUMUONSACH>(x => x.MAPMS == item.MAPMS).NGMUON - DateTime.Today)?.Days > dataProvider.LibraryRules.NGAYMUON)
                    item.TIENPHAT = ((dataProvider.GetItem<PHIEUMUONSACH>(x => x.MAPMS == item.MAPMS).NGMUON - DateTime.Today)?.Days - dataProvider.LibraryRules.NGAYMUON) * dataProvider.LibraryRules.TIENPHAT;
                CTSACH d = dataProvider.GetItem<CTSACH>(x => x.MACTS == item.MACTS);
                d.MATT = 1;
                dataProvider.Update<CTSACH>(d, x => x.MACTS == d.MACTS);
                dataProvider.Create<CTPTS>(item);
            }
            ReturnBookBill = new BindableCollection<CTPTS>();
            BorrowBillList = dataProvider.GetPMS(x => x.MADG == Client.MADG);
            Amount = dataProvider.LibraryRules.SACHMUONTOIDA ?? 5;
            foreach (PHIEUMUONSACH item in BorrowBillList)
            {
                Amount -= item.CTSACH.Count - item.CTPTS.Count;
            }
            eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo phiếu trả sách thành công!"));
        }
        #endregion
        #region Client Info Method
        private CollectMoneyDialog moneyDialog;
        public void CollectMoney()
        {
            moneyDialog = new CollectMoneyDialog();
            this.DialogContent = moneyDialog;
            moneyDialog.DataContext = this;
            IsDialogOpen = true;
        }
        public void Collect()
        {
            if (Money > Client.TIENNO)
            {                
                MessageBox.Show("Không được thu vượt quá số tiền nợ", "Thông báo");
                return;
            }    
            Client.TIENNO -= Money;
            dataProvider.Update<DOCGIA>(Client, x => x.MADG == Client.MADG);
            PHIEUTHUTIENPHAT p = new PHIEUTHUTIENPHAT() { MADG = Client.MADG, NGTHU = DateTime.Today, SOTIENTHU = Money, NGUOILAP = dataProvider.User.MATK };
            dataProvider.Create<PHIEUTHUTIENPHAT>(p);
            IsDialogOpen = false;
            eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Thu tiền phạt thành công."));
        }
        public void Renew()
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn gia hạn thẻ không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Client.NGLAPTHE = DateTime.Today;
                if (dataProvider.Update<DOCGIA>(Client, x => x.MADG == Client.MADG))
                {
                    this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Gia hạn thành công."));                   
                }
                else
                {
                    this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Gia hạn không thành công."));                  
                }
            }
        }

        public void Save()
        {
            Client.MALDG = LoaiDocGia.MALDG;
            bool IsSuccess = dataProvider.Update<DOCGIA>(Client, x => x.MADG == Client.MADG);
            if (IsSuccess)
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Cập nhật thành công"));                
                Cancel();
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo mới thất bại"));
            }
        }

        public void Cancel()
        {
            Client = dataProvider.GetClient(x => x.MADG == Client.MADG);
            LoaiDocGia = ClientTypeList.FirstOrDefault(x => x.MALDG == Client.MALDG);
            if (string.IsNullOrEmpty(Client.HINHANH) == true || File.Exists(Client.HINHANH) == false)
                Client.HINHANH = @"/Resources/Images/ClientAccount/DefaultAccount.png";
        }

        public void ChangeAvartar()
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.InitialDirectory = @"C:\";
            openFile.RestoreDirectory = true;
            openFile.Title = "Đổi ảnh đại diện";
            openFile.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg";
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.Multiselect = false;
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Client.HINHANH = openFile.FileName;
            }
        }

        #endregion Client Info Method

        public void GoBack()
        {
            eventAggregator.PublishOnCurrentThread("GoBack");
        }

        #region ClientTypeDialog

        private ClientTypeDialog ClientTypeForm = new ClientTypeDialog();
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

        private LOAIDOCGIA _SelectedClientType;

        public LOAIDOCGIA SelectedClientType
        {
            get { return _SelectedClientType; }
            set
            {
                _SelectedClientType = value;
                TypeString = (value is null) ? null : value.LOAIDOCGIA1;
                NotifyOfPropertyChange("SelectedClientType");
            }
        }

        private string _TypeString;

        public string TypeString
        {
            get { return _TypeString; }
            set { _TypeString = value; NotifyOfPropertyChange("TypeString"); }
        }

        public void OpenEditClientType()
        {
            this.DialogContent = ClientTypeForm;
            ClientTypeForm.DataContext = this;
            IsDialogOpen = true;
            SelectedClientType = new LOAIDOCGIA();
        }

        public void Exit()
        {
            IsDialogOpen = false;
        }

        public void UpdateClientType()
        {
            SelectedClientType.LOAIDOCGIA1 = TypeString;
            bool IsSuccess = dataProvider.Update<LOAIDOCGIA>(SelectedClientType, g => g.MALDG == SelectedClientType.MALDG);
            if (IsSuccess)
            {
                ShowMessage(new Models.Message("Thông báo", "Cập nhật thành công"));
            }
            else
            {
                ShowMessage(new Models.Message("Thông báo", "Cập nhật không thành công"));
            }
        }

        public void NewClientType()
        {
            bool IsSuccess = dataProvider.Create<LOAIDOCGIA>(new LOAIDOCGIA() { LOAIDOCGIA1 = TypeString });
            if (IsSuccess)
            {
                ShowMessage(new Models.Message("Thông báo", "Tạo mới thành công"));
                ClientTypeList = dataProvider.GetList<LOAIDOCGIA>();
            }
            else
            {
                ShowMessage(new Models.Message("Thông báo", "Tạo mới không thành công"));
            }
        }

        public void DeleteClientType()
        {
            bool IsSuccess = dataProvider.Delete<LOAIDOCGIA>(g => g.MALDG == SelectedClientType.MALDG);
            if (IsSuccess)
            {
                ShowMessage(new Models.Message("Thông báo", "Xóa thành công"));
                ClientTypeList = dataProvider.GetList<LOAIDOCGIA>();
            }
            else
            {
                ShowMessage(new Models.Message("Thông báo", "Xóa không thành công"));
            }
        }

        private void ShowMessage(Models.Message mess)
        {
            MessageBox.Show(mess.Note, mess.Title);
        }

        #endregion ClientTypeDialog

        #region QRCode properties and methods

        private BitmapImage _BitImage;
        private Bitmap _bitmap;
        private FilterInfo _currentDevice;
        private string _QRString;
        private BindableCollection<FilterInfo> _videoDevices;
        private IVideoSource _videoSource;
        private DispatcherTimer timer;
        private QRScanDialog scanDialog;
        private int _ActionCode;
        public BitmapImage BitImage
        {
            get { return _BitImage; }
            set { _BitImage = value; NotifyOfPropertyChange("BitImage"); }
        }
        public string QRString
        {
            get { return _QRString; }
            set { _QRString = value; NotifyOfPropertyChange("QRString"); }
        }

        public void CloseQRDialog()
        {
            StopCamera();
            timer.Stop();
            IsDialogOpen = false;
        }

        public void ScanQRCode(int t)
        {
            _ActionCode = t;
            scanDialog = new QRScanDialog();
            DialogContent = scanDialog;
            scanDialog.DataContext = this;
            IsDialogOpen = true;
            GetVideoDevice();
            StartCamera();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();
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
                CloseQRDialog();
                if (_ActionCode == 1)
                {
                    AddToBorrow(QRString);
                }   
                else
                {
                    CTPTS t = MyList.FirstOrDefault(x => x.MACTS.ToString() == QRString);
                    if (t is null)
                    {
                        this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Không tìm thấy"));
                        return;
                    }    
                    ReturnBookBill.Add(t);
                    MyList.Remove(t);
                    SumMoney += t.TIENPHAT ?? 0;
                }    
                
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
        #endregion QRCode properties and methods
    }
}