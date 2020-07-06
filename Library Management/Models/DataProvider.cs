using Caliburn.Micro;
using Library_Management.Resources.Sercurity;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;

namespace Library_Management.Models
{
    public class DataProvider
    {
        public BindableCollection<TAIKHOAN> Users { get; set; }
        public TAIKHOAN User { get; set; }
        public BindableCollection<LOAIDOCGIA> ClientTypeList { get; set; }
        public QUYDINH LibraryRules { get; set; }
        public BindableCollection<TACGIA> AuthorList { get; set; }
        public BindableCollection<THELOAI> CategoryList { get; set; }
        public BindableCollection<NHAXUATBAN> PublisherList { get; set; }
        public SACH SelectedBook { get; set; }
        public DOCGIA SelectedClient { get; set; }
        public int WhereToGoBack { get; set; }

        public DataProvider()
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                ClientTypeList = new BindableCollection<LOAIDOCGIA>(db.LOAIDOCGIA.ToList());
                LibraryRules = db.QUYDINH.OrderByDescending(x => x.NGAYSUA).FirstOrDefault();
                AuthorList = new BindableCollection<TACGIA>(db.TACGIA.ToList());
                CategoryList = new BindableCollection<THELOAI>(db.THELOAI.ToList());
                PublisherList = new BindableCollection<NHAXUATBAN>(db.NHAXUATBAN.ToList());
            }
        }

        public async Task FindUser(string username, string pass)
        {
            pass = MD5Sercurity.MD5Hash(pass);
            using (QLTVEntities db = new QLTVEntities())
            {
                User = await db.TAIKHOAN.FirstOrDefaultAsync(x => x.TAIKHOAN1 == username && x.MATKHAU == pass);
            }
        }

        public bool ChangeAvartar(string path)
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                var d = db.TAIKHOAN.FirstOrDefault(x => x.MATK == User.MATK);
                if (d != null)
                {
                    d.HINHANH = path;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool Create<T>(T dto) where T : class
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                try
                {
                    db.Set<T>().Add(dto);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }
        }

        public bool Delete<T>(Expression<Func<T, bool>> currentEntityFilter) where T : class
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                try
                {
                    var current = db.Set<T>().FirstOrDefault(currentEntityFilter);
                    if (current is null)
                    {
                        return false;
                    }
                    db.Set<T>().Remove(current);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }
        }

        public bool Update<T>(T dto, Expression<Func<T, bool>> currentEntityFilter) where T : class
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                try
                {
                    var current = db.Set<T>().FirstOrDefault(currentEntityFilter);
                    if (current is null)
                    {
                        return false;
                    }
                    db.Entry(current).CurrentValues.SetValues(dto);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public T GetItem<T>(Expression<Func<T, bool>> currentEntityFilter) where T : class
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                var current = db.Set<T>().FirstOrDefault(currentEntityFilter);
                return current;
            }
        }

        public T GetItemInclude<T>(Expression<Func<T, object>>[] includes) where T : class
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                var query = db.Set<T>().AsQueryable();
                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }
                return query.FirstOrDefault();
            }
        }
        public DOCGIA GetClient(Expression<Func<DOCGIA, bool>> currentEntityFilter)
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                return db.DOCGIA.Include(x => x.LOAIDOCGIA).FirstOrDefault(currentEntityFilter);
            }
        }
        public BindableCollection<T> GetList<T>() where T : class
        {
            using (QLTVEntities db = new QLTVEntities())
            {
                return new BindableCollection<T>(db.Set<T>().ToList());
            }
        }

        public BindableCollection<SACH> GetBookList(Expression<Func<SACH, bool>> currentEntityFilter)
        {
            BindableCollection<SACH> BookList = new BindableCollection<SACH>();
            using (QLTVEntities db = new QLTVEntities())
            {
                BookList = new BindableCollection<SACH>(db.SACH.Include(x => x.TACGIA).Include(x => x.THELOAI).Include(x => x.NHAXUATBAN).Include(x => x.CTSACH).Where(currentEntityFilter));
                if (BookList.Any())
                {
                    foreach (SACH s in BookList)
                    {
                        int c = 0;
                        if (s.CTSACH is null)
                            continue;
                        foreach (CTSACH cts in s.CTSACH)
                        {
                            CTSACH t = db.CTSACH.Include(x => x.PHIEUMUONSACH).FirstOrDefault(p => p.MACTS == cts.MACTS);
                            if (t is null)
                                continue;
                            c += t.PHIEUMUONSACH.Count();
                        }
                        s.LUOTMUON = c;
                    }
                }
            }
            return BookList;
        }

        public BindableCollection<SACH> GetNewBookList()
        {
            BindableCollection<SACH> BookList;
            BindableCollection<CTSACH> ctsList;
            using (QLTVEntities db = new QLTVEntities())
            {
                BookList = new BindableCollection<SACH>(db.SACH.Include(x => x.CTSACH).Where(y => true));
                ctsList = new BindableCollection<CTSACH>(db.CTSACH.Include(x => x.SACH.TACGIA).Include(x => x.SACH.THELOAI).Include(x => x.SACH.NHAXUATBAN).Include(x => x.PHIEUNHAPSACH).OrderByDescending(x => x.MACTS));
                BookList = new BindableCollection<SACH>(ctsList.GroupBy(x => x.SACH).Select(p => p.Key));
                if (BookList.Any())
                {
                    foreach (SACH s in BookList)
                    {
                        int c = 0;
                        if (s.CTSACH is null)
                            continue;
                        foreach (CTSACH cts in s.CTSACH)
                        {
                            CTSACH t = db.CTSACH.Include(x => x.PHIEUMUONSACH).FirstOrDefault(p => p.MACTS == cts.MACTS);
                            if (t is null)
                                continue;
                            c += t.PHIEUMUONSACH.Count();
                        }
                        s.LUOTMUON = c;
                    }
                }
            }
            return BookList;
        }

        public BindableCollection<PHIEUNHAPSACH> GetImportBillList(Expression<Func<PHIEUNHAPSACH, bool>> currentEntityFilter)
        {
            BindableCollection<PHIEUNHAPSACH> temp;
            using (QLTVEntities db = new QLTVEntities())
            {
                temp = new BindableCollection<PHIEUNHAPSACH>(db.PHIEUNHAPSACH.Include(x => x.CTSACH).Include(x => x.TAIKHOAN).Where(currentEntityFilter));
                if (temp.Any())
                {
                    foreach (PHIEUNHAPSACH item in temp)
                    {
                        if (item.CTSACH != null)
                        {
                            item.SOLUONG = item.CTSACH.Count;
                            decimal c = 0;
                            foreach (CTSACH cs in item.CTSACH)
                            {
                                c += cs.GIANHAP ?? 0;
                            }
                            item.TONGTIEN = c;
                        }
                    }
                }
            }
            return temp;
        }

        public BindableCollection<CTSACH> GetBookDetailList(Expression<Func<CTSACH, bool>> currentEntityFilter)
        {
            BindableCollection<CTSACH> temp;
            using (QLTVEntities db = new QLTVEntities())
            {
                temp = new BindableCollection<CTSACH>(db.CTSACH.Include(x => x.TINHTRANG).Include(p => p.PHIEUNHAPSACH.TAIKHOAN).Include(y => y.PHIEUMUONSACH).Include(y => y.SACH).Where(currentEntityFilter));
            }
            return temp;
        }

        public BindableCollection<DOCGIA> GetClientList(Expression<Func<DOCGIA, bool>> currentEntityFilter)
        {
            BindableCollection<DOCGIA> ds;
            using (QLTVEntities db = new QLTVEntities())
            {
                ds =  new BindableCollection<DOCGIA>(db.DOCGIA.Include(x => x.LOAIDOCGIA).Where(currentEntityFilter));
            }
            return ds;
        }
        public BindableCollection<PHIEUMUONSACH> GetPMS(Expression<Func<PHIEUMUONSACH, bool>> currentEntityFilter)
        {
            BindableCollection<PHIEUMUONSACH> ps;
            using (QLTVEntities db = new QLTVEntities())
            {
                ps = new BindableCollection<PHIEUMUONSACH>(db.PHIEUMUONSACH.Include(x => x.CTPTS).Include(y => y.CTSACH).Where(currentEntityFilter));              
            }
            return ps;
        }
        public void UpdateCTS(CTSACH cs, PHIEUMUONSACH pms)
        {
            using(QLTVEntities db = new QLTVEntities())
            {
                CTSACH c = db.CTSACH.FirstOrDefault(x => x.MACTS == cs.MACTS);
                PHIEUMUONSACH d = db.PHIEUMUONSACH.FirstOrDefault(x => x.MAPMS == pms.MAPMS);
                c.MATT = cs.MATT;
                c.PHIEUMUONSACH.Add(d);
                d.CTSACH.Add(c);
                db.SaveChanges();
            }
        }
        public BindableCollection<CTPTS> ReturnBookList(DOCGIA dg)
        {
            BindableCollection<CTSACH> temp;
            BindableCollection<CTPTS> temp2;
            using (QLTVEntities db = new QLTVEntities())
            {
                temp = new BindableCollection<CTSACH>(db.CTSACH.Include(Y => Y.SACH).Where(x => x.PHIEUMUONSACH.OrderByDescending(z => z.NGMUON).FirstOrDefault().MADG == dg.MADG && x.MATT == 3));
                temp2 = new BindableCollection<CTPTS>(temp.Select(x => new CTPTS
                {
                    MACTS = x.MACTS,
                    MAPMS = x.PHIEUMUONSACH.OrderByDescending(z => z.NGMUON).FirstOrDefault(y => y.MADG == dg.MADG).MAPMS,
                    MASACH = x.MASACH ?? 0,
                    TENSACH = x.SACH.TENSACH,
                    NGMUON = x.PHIEUMUONSACH.OrderByDescending(z => z.NGMUON).FirstOrDefault(y => y.MADG == dg.MADG).NGMUON ?? DateTime.Today,
                }));
                foreach(CTPTS item in temp2)
                {
                    if((DateTime.Today-item.NGMUON).Days > LibraryRules.NGAYMUON)
                    {
                        item.TIENPHAT = ((DateTime.Today - item.NGMUON).Days - LibraryRules.NGAYMUON) * LibraryRules.TIENPHAT;
                    }
                    else
                    {
                        item.TIENPHAT = 0;
                    }    
                }    
            }
            return temp2;
        }
        public BindableCollection<ReportItem1> Report1(DateTime d1, DateTime d2)
        {
            BindableCollection<ReportItem1> temp;
            using (QLTVEntities db = new QLTVEntities())
            {
                BindableCollection<PHIEUMUONSACH> pms = new BindableCollection<PHIEUMUONSACH>(db.PHIEUMUONSACH.Where(y => y.NGMUON >= d1 && y.NGMUON <= d2));
                BindableCollection<CTSACH> cts = new BindableCollection<CTSACH>();
                foreach (var item in pms)
                {
                    cts.AddRange(item.CTSACH);
                }

                temp = new BindableCollection<ReportItem1>(cts.GroupBy(x => x.SACH.MATL).Select(y => new ReportItem1()
                {
                    TENTL = GetItem<THELOAI>(u => u.MATL == y.Key).THELOAI1,
                    SOLUOT = y.Count()
                })) ;
            }
            return temp;
        }
        public BindableCollection<ReportItem2> Report2(DateTime d1, DateTime d2)
        {
            BindableCollection<ReportItem2> temp;
            using (QLTVEntities db = new QLTVEntities())
            {
                
                temp = new BindableCollection<ReportItem2>(db.CTPTS.Where(x => x.TIENPHAT > 0).Select(y => new ReportItem2()
                {
                    TENSACH = y.CTSACH.SACH.TENSACH,
                    NGAYMUON = y.PHIEUMUONSACH.NGMUON ?? DateTime.Today,
                    NGAYTRA = y.PHIEUTRASACH.NGTRASACH ?? DateTime.Today
                })) ;              
            }
            return temp;
        }
    }
}