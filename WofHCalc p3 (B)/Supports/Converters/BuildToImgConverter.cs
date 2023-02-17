using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WofHCalc.Supports.Converters
{
#pragma warning disable CS8603 // Possible null reference return.
    internal class BuildToImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //source->target
            if (value is BuildName)
            {
                return $"/DataSourses/img/builds/{(int)value}_1.png";
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {   //target->source
            return null;
            //return (BuildName)(int.Parse(((string)value).Substring(12, ((string)value).Length - 18)));            
        }
    }
}
#pragma warning restore CS8603 // Possible null reference return.
