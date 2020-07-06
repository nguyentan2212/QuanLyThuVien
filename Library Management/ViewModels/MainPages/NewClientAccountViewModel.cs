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
    public class NewClientAccountViewModel : ViewBaseModel
    {
        #region Properties

        private BindableCollection<LOAIDOCGIA> _ClientTypeList;

        public BindableCollection<LOAIDOCGIA> ClientTypeList
        {
            get { return _ClientTypeList; }
            set { _ClientTypeList = value; NotifyOfPropertyChange("ClientTypeList"); }
        }

        private LOAIDOCGIA _ClienType;

        public LOAIDOCGIA ClienType
        {
            get { return _ClienType; }
            set { _ClienType = value; NotifyOfPropertyChange("ClienType"); }
        }

        private int _ClientMinAge;

        public int ClientMinAge
        {
            get { return _ClientMinAge; }
            set { _ClientMinAge = value; NotifyOfPropertyChange("ClientMinAge"); }
        }

        private int _ClientMaxAge;

        public int ClientMaxAge
        {
            get { return _ClientMaxAge; }
            set { _ClientMaxAge = value; NotifyOfPropertyChange("ClientMaxAge"); }
        }

        private DOCGIA _Client;

        public DOCGIA Client
        {
            get { return _Client; }
            set { _Client = value; NotifyOfPropertyChange("Client"); }
        }

        private BindableCollection<DOCGIA> _ClientList;

        public BindableCollection<DOCGIA> ClientList
        {
            get { return _ClientList; }
            set { _ClientList = value; NotifyOfPropertyChange("ClientList"); }
        }

        private string _ClientSearchString;

        public string ClientSearchString
        {
            get { return _ClientSearchString; }
            set
            {
                if (_ClientSearchString != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        ClientList = dataProvider.GetClientList(x => true);
                    }
                    else
                    {
                        ClientList = dataProvider.GetClientList(x => x.MADG.ToString().ToUpper().Contains(value) || x.HOTEN.ToUpper().Contains(value));
                    }
                }
                _ClientSearchString = value;
                NotifyOfPropertyChange("ClientSearchString");
            }
        }

        

        #endregion Properties

        public NewClientAccountViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            ClientTypeList = new BindableCollection<LOAIDOCGIA>();
            foreach (var item in dataProvider.ClientTypeList)
            {
                ClientTypeList.Add(item);
            }
            Client = new DOCGIA();
            if (string.IsNullOrEmpty(Client.HINHANH) == true || File.Exists(Client.HINHANH) == false)
                Client.HINHANH = @"/Resources/Images/ClientAccount/DefaultAccount.png";
            ClientMinAge = dataProvider.LibraryRules.TUOITOITHIEU ?? 18;
            ClientMaxAge = dataProvider.LibraryRules.TUOITOIDA ?? 55;
            eventAggregator.Subscribe(this);
            ClientList = dataProvider.GetClientList(x => true);
            dataProvider.WhereToGoBack = 3;
        }

        public void Save()
        {
            Client.MALDG = ClienType.MALDG;
            Client.NGLAPTHE = DateTime.Today;
            bool IsSuccess = dataProvider.Create<DOCGIA>(Client);
            if (IsSuccess)
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo mới thành công"));
                string imgPath = AppDomain.CurrentDomain.BaseDirectory + @"/Resources/Images/ClientAccount/" + Client.MADG.ToString() + ".png";
                File.Copy(Client.HINHANH, imgPath, true);
                Client.HINHANH = imgPath;
                IsSuccess = dataProvider.Update<DOCGIA>(Client, g => g.MADG == Client.MADG);
                Cancel();
                ClientList = dataProvider.GetClientList(x => true);
            }
            else
            {
                this.eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Tạo mới thất bại"));
            }
        }

        public void Cancel()
        {
            Client = new DOCGIA() { HINHANH = @"/Resources/Images/ClientAccount/DefaultAccount.png" };
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

        public void MoreInfo(DOCGIA dg)
        {
            dataProvider.SelectedClient = dg;
            eventAggregator.PublishOnCurrentThread("OpenClientView");
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

        public void ScanQRCode()
        {
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
                int t = int.Parse(QRString);
                DOCGIA dg = ClientList.FirstOrDefault(x => x.MADG == t);
                dataProvider.SelectedClient = dg;               
                StopCamera();
                IsDialogOpen = false;
                eventAggregator.PublishOnCurrentThread("OpenClientView");
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