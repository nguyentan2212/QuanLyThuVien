using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Library_Management.Resources.Sercurity
{
    public class NotEmptyValidationRule: ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return string.IsNullOrWhiteSpace((value ?? " ").ToString())
				? new ValidationResult(false, "Không được để trống hoặc toàn khoảng trắng.")
				: ValidationResult.ValidResult;
		}
	}
}
