using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Library_Management.Resources.Sercurity
{
    class NumberValidationRule : ValidationRule
    {
        public bool IsYearValidation { get; set; }
        public bool IsGreaterThanZero { get; set; }
        public Wrapper Wrapper { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((value ?? " ").ToString()))
                return new ValidationResult(false, "Không được để trống hoặc toàn khoảng trắng.");
            string text = value as string;
            int num;
            decimal dec = 0;
            int.TryParse(text, out num);
            if (IsYearValidation)
            {
                if (num > DateTime.Now.Year)
                {
                    return new ValidationResult(false, "Năm không được vượt quá năm hiện tại.");
                }
                if (DateTime.Now.Year - num > Wrapper.PublishRange)
                {
                    return new ValidationResult(false, "Năm không được cách hiện tại quá " + Wrapper.PublishRange + " năm.");
                }
                return ValidationResult.ValidResult;
            }
            else if (!decimal.TryParse(text, out dec))
            {
                return new ValidationResult(false, "Không phải là định dạng số.");
            }     
            if (IsGreaterThanZero && dec == 0)
            {
                return new ValidationResult(false, "Phải lớn hơn 0.");
            }
            return ValidationResult.ValidResult;
        }     
    }
}
