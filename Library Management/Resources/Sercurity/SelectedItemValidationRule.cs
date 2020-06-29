using Library_Management.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;

namespace Library_Management.Resources.Sercurity
{
    class SelectedItemValidationRule : ValidationRule
    {       
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is null)
                return new ValidationResult(false, "Không được để trống hoặc toàn khoảng trắng.");                      
            return ValidationResult.ValidResult;
        }       
    }
}
