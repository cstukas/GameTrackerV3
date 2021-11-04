using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DesktopUI.Converters
{
    class SubstringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            int wordIndex = System.Convert.ToInt32((string)parameter);
            var sentence = (string)value;

            var split = sentence.Split(' ');
            var count = split.Length;
            if (wordIndex >= split.Length) return "";
            return split[wordIndex].Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
