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

namespace Library_Management.Models
{
    public class DataProvider
    {
        public BindableCollection<TAIKHOAN> Users { get; set; }
        public TAIKHOAN User { get; set; }
       
        public DataProvider()
        {

           
        }
        public async Task FindUser(string username, string pass)
        {
            pass = MD5Sercurity.MD5Hash(pass);
            using (QLTVEntities db = new QLTVEntities())
            {
                User = await db.TAIKHOAN.FirstOrDefaultAsync(x => x.TAIKHOAN1 == username && x.MATKHAU == pass);               
            }
        }
        public async Task<bool> ChangePassword(string oldpass, string newpass)
        {
            if (string.IsNullOrWhiteSpace((oldpass ?? " ").ToString()) && string.IsNullOrWhiteSpace((newpass ?? " ").ToString()))
                return false;
            oldpass = MD5Sercurity.MD5Hash(oldpass);
            newpass = MD5Sercurity.MD5Hash(newpass);
            using (QLTVEntities db = new QLTVEntities())
            {

                User.MATKHAU = newpass;
                try
                {
                    db.Set<TAIKHOAN>().AddOrUpdate(User);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message.ToString());
                    return false;
                }
            }
        }

        public async Task<bool> UpdateUserAccount(TAIKHOAN UserAccount)
        {
            if (UserAccount == null)
                return false;
            using (QLTVEntities db = new QLTVEntities())
            {

                User = UserAccount;
                try
                {
                    db.Set<TAIKHOAN>().AddOrUpdate(UserAccount);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message.ToString());
                    return false;
                }               
            }                         
        }
        public async Task<bool> ChangeAvartar(string path)
        {
            string imgPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Images\LibrarianAccount\" + User.MATK.ToString() + ".png";
            File.Copy(path, imgPath);
            using (QLTVEntities db = new QLTVEntities())
            {
                User.HINHANH = imgPath;
                try
                {
                    db.Set<TAIKHOAN>().AddOrUpdate(User);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message.ToString());
                    return false;
                }
            }
        }
    }
}
