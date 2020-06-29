using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Caliburn.Micro;
using Library_Management.Resources.Sercurity;
using System.Windows;
using System.Data.Entity.Migrations;
using System.IO;
using ControlzEx.Standard;
using System.Linq.Expressions;

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
        public int WhereToGoBack { get; set; }       
        public DataProvider()
        {
            using(QLTVEntities db = new QLTVEntities())
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
 
      
        public async Task<bool> ChangeAvartar(string path)
        {
            string imgPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\LibrarianAccount\" + User.MATK.ToString() + ".png";            
            using (QLTVEntities db = new QLTVEntities())
            {
                User.HINHANH = imgPath;
                try
                {
                    File.Copy(path, imgPath, true);
                    db.Set<TAIKHOAN>().AddOrUpdate(User);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString());
                    return false;
                }
            }
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
                catch(Exception e)
                {
                    return false;
                }
            }            
        }
        public T GetItem<T>(Expression<Func<T, bool>> currentEntityFilter) where T : class
        {
            using(QLTVEntities db = new QLTVEntities())
            {
                var current = db.Set<T>().FirstOrDefault(currentEntityFilter);
                return current;
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
        public BindableCollection<PHIEUNHAPSACH> GetImportBillList(Expression<Func<PHIEUNHAPSACH, bool>> currentEntityFilter)
        {
            BindableCollection<PHIEUNHAPSACH> temp;
            using(QLTVEntities db = new QLTVEntities())
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
                temp = new BindableCollection<CTSACH>(db.CTSACH.Include(x => x.TINHTRANG).Include(p => p.PHIEUNHAPSACH.TAIKHOAN).Where(currentEntityFilter));
            }
            return temp;
        }
        public void up(SACH s)
        {
            using(QLTVEntities db = new QLTVEntities())
            {
                BindableCollection<CTSACH> c = new BindableCollection<CTSACH>(db.CTSACH.Where(x => x.MASACH == s.MASACH));
                foreach (CTSACH item in c)
                {
                    item.MATT = 1;
                }
                db.SaveChanges();
            }
        }
    }
}
