using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using Caliburn.Micro;
using Library_Management.Models;

namespace Library_Management.Resources.Sercurity
{
    class SelectedItemValidationRule : ValidationRule
    {
        public string Tag { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((value ?? " ").ToString()))
                return new ValidationResult(false, "Không được để trống hoặc toàn khoảng trắng.");
            switch(Tag)
            {
                case "ClientAccountList":
                    if (Wrapper.ClientAccountList is null)
                        return new ValidationResult(false, "Danh sách trống.");
                    var check = Wrapper.ClientAccountList.FirstOrDefault(x => x == value);
                    if (check == null)
                        return new ValidationResult(false, "Lựa chọn không tồn tại.");
                    break;
                default:
                    break;
            }
            
            return ValidationResult.ValidResult;
        }
        public Wrapper Wrapper { get; set; }
    }
}
