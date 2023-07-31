using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WofHCalc.Supports.Converters
{
    public class NullableToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            List<string> ans = new();
            var vals = (IEnumerable)value;
            foreach (var v in vals)
            {
                if (v != null)
                    ans.Add(v.ToString()!);
                else
                    ans.Add("");
            }
            return ans;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
