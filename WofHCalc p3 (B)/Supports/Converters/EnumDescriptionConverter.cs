using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WofHCalc.Supports.ForEnumBinds;

namespace WofHCalc.Supports.Converters
{
    internal class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.GetType().IsEnum) throw new Exception("error enum to description");
            else
            {
                //Type t = value.GetType();
                return EnumHelper.Description((Enum)value);
                //return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
