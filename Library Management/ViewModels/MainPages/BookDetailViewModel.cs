using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Library_Management.Models;
using Library_Management.Views.UserControls;

namespace Library_Management.ViewModels.MainPages
{
    public class BookDetailViewModel:ViewBaseModel
    {
        #region Properties
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

        private bool _IsNotLogin;
        public bool IsNotLogin
        {
            get { return _IsNotLogin; }
            set { _IsNotLogin = value; NotifyOfPropertyChange("IsNotLogin"); }
        }

        private bool _IsLogin;
        public bool IsLogin
        {
            get { return _IsLogin; }
            set { _IsLogin = value; IsNotLogin = !value; NotifyOfPropertyChange("IsLogin"); }
        }

        private int _PublishRange;
        public int PublishRange
        {
            get { return _PublishRange; }
            set { _PublishRange = value; NotifyOfPropertyChange("PublishRange"); }
        }

        private BindableCollection<CTSACH> _DetailList;
        public BindableCollection<CTSACH> DetailList
        {
            get { return _DetailList; }
            set { _DetailList = value; NotifyOfPropertyChange("DetailList"); }
        }

        #endregion
        #region Dialog Properties

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
        #region Dialog Methods
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
        #endregion
        public BookDetailViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            AuthorList = dataProvider.AuthorList;
            CategoryList = dataProvider.CategoryList;
            PublisherList = dataProvider.PublisherList;           
            Book = dataProvider.SelectedBook;
            PublishRange = dataProvider.LibraryRules.KHOANGCACHXB ?? 0;
            eventAggregator.Subscribe(this);
            Author = AuthorList.FirstOrDefault(x => x.MATG == Book.MATG);
            Category = CategoryList.FirstOrDefault(x => x.MATL == Book.MATL);
            Publisher = PublisherList.FirstOrDefault(x => x.MANXB == Book.MANXB);
            DetailList = dataProvider.GetBookDetailList(x => x.MASACH == Book.MASACH);
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
        public void Save()
        {
            Book.MANXB = Publisher.MANXB;
            Book.MATL = Category.MATL;
            Book.MATG = Author.MATG;
            dataProvider.Update<SACH>(Book, x => x.MASACH == Book.MASACH);
        }
        public void Cancel()
        {
            Book.TENSACH = dataProvider.GetItem<SACH>(x => x.MASACH == Book.MASACH).TENSACH;
            Book.NAMXB = dataProvider.GetItem<SACH>(x => x.MASACH == Book.MASACH).NAMXB;
            Book.ANHBIA = dataProvider.GetItem<SACH>(x => x.MASACH == Book.MASACH).ANHBIA;
            Author = AuthorList.FirstOrDefault(x => x.MATG == Book.MATG);
            Category = CategoryList.FirstOrDefault(x => x.MATL == Book.MATL);
            Publisher = PublisherList.FirstOrDefault(x => x.MANXB == Book.MANXB);         
        }
        public void GoBack()
        {
            //dataProvider.up(Book);
            eventAggregator.PublishOnCurrentThread("GoBackSearchBook");
        }
        private void ShowMessage(Models.Message mess)
        {
            MessageBox.Show(mess.Note, mess.Title);
        }
    }
}
