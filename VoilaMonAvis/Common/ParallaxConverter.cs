using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace VoilaMonAvis.Common
{
    public class ParallaxConverter : IValueConverter
    {
        const double _factor = -0.10;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double factor;
            if (!Double.TryParse(parameter as string, out factor))
            {
                factor = _factor;
            }

            if (value is double)
            {
                return (double)value * factor;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double factor;
            if (!Double.TryParse(parameter as string, out factor))
            {
                factor = _factor;
            }

            if (value is double)
            {
                return (double)value / factor;
            }
            return 0;
        }
    } 
}
