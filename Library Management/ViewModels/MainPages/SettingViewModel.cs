using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Library_Management.Models;

namespace Library_Management.ViewModels.MainPages
{
    public class SettingViewModel:ViewBaseModel
    {
        #region Properties

        private BindableCollection<QUYDINH> _Rule;
        public BindableCollection<QUYDINH> Rule
        {
            get { return _Rule; }
            set { _Rule = value; NotifyOfPropertyChange("Rule"); }
        }

        private BindableCollection<QUYDINH> _RuleList;
        public BindableCollection<QUYDINH> RuleList
        {
            get { return _RuleList; }
            set { _RuleList = value; NotifyOfPropertyChange("RuleList"); }
        }

        #endregion
        public SettingViewModel(DataProvider dataProvider, IEventAggregator eventAggregator) : base(dataProvider, eventAggregator)
        {
            Rule = new BindableCollection<QUYDINH>();
            QUYDINH q = dataProvider.GetList<QUYDINH>().OrderByDescending(x => x.ID).FirstOrDefault();          
            Rule.Add(q);
            RuleList = dataProvider.GetList<QUYDINH>();
        }
        public void Cancel()
        {
            Rule = new BindableCollection<QUYDINH>();
            QUYDINH q = dataProvider.GetList<QUYDINH>().OrderByDescending(x => x.ID).FirstOrDefault();
            Rule.Add(q);
        }
        public void Update()
        {
            QUYDINH q = new QUYDINH()
            {
                KHOANGCACHXB = Rule.FirstOrDefault().KHOANGCACHXB,
                NGAYMUON = Rule.FirstOrDefault().NGAYMUON,
                NGUOISUA = dataProvider.User.MATK,
                NGAYSUA = DateTime.Today,
                SACHMUONTOIDA = Rule.FirstOrDefault().SACHMUONTOIDA,
                THOIHANTHE = Rule.FirstOrDefault().THOIHANTHE,
                TIENPHAT = Rule.FirstOrDefault().TIENPHAT,
                TIENPHATTOIDA = Rule.FirstOrDefault().TIENPHATTOIDA,
                TUOITOIDA = Rule.FirstOrDefault().TUOITOIDA,
                TUOITOITHIEU = Rule.FirstOrDefault().TUOITOITHIEU
            };
            if(dataProvider.Create<QUYDINH>(q))
            {
                eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Cập nhật thành công"));
                RuleList = dataProvider.GetList<QUYDINH>();
                dataProvider.LibraryRules = RuleList.LastOrDefault();
            }    
            else
            {
                eventAggregator.PublishOnCurrentThread(new Models.Message("Thông báo", "Cập nhật thất bại"));
            }    
                  
        }
    }
}
