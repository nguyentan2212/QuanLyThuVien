using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Library_Management.Resources.Sercurity
{
    public class DateValidationRule : ValidationRule
    {
        public bool IsAgeValidation { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime time;
            if (!DateTime.TryParse((value ?? "").ToString(),CultureInfo.CurrentCulture,DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,out time)) 
                return new ValidationResult(false, "Ngày không đúng.");
            if (time.Date > DateTime.Today)
                return new ValidationResult(false, "Ngày không được vượt qua hôm nay");   
            if (!IsAgeValidation)
            {
                return ValidationResult.ValidResult;
            }
            var today = DateTime.Today;
            int age = today.Year - time.Year;           
            if (age < Wrapper.MinAge || age > Wrapper.MaxAge)
                return new ValidationResult(false, "Tuổi hiện tại là " + age + ", không nằm trong khoảng " + Wrapper.MinAge + " tuổi đến " + Wrapper.MaxAge + " tuổi");
            return ValidationResult.ValidResult;
        }

        public Wrapper Wrapper { get; set; }
    }
}
