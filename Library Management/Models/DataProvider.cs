using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using Caliburn.Micro;
using System.Security.Cryptography;
using Library_Management.Resources.Sercurity;

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
    }
}
