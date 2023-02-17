using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WofHCalc.Supports.Converters
{
    internal class ResProdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var v = value as ObservableCollection<bool>;
                List<string> list = new List<string>();
                for (int i = 0; i < 23; i++)
                {
                    if (v[i]) list.Add($"/DataSourses/Img/icons/res/res{i}.png");
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
