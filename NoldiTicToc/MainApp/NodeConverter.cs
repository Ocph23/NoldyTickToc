using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MainApp
{
    public class NodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double nilai = 0;
            double param =double.Parse(parameter.ToString());
            if(param==0)
                nilai = (double)value/2;
            else
                nilai = (double)value+((double)value / 2);

            return (double)(nilai - (40/2));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
