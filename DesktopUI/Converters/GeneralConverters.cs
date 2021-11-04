using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DesktopUI.Converters
{
    class BoolFlipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (targetType != typeof(bool))
            //    throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt = (DateTime)value;
            if (dt.Year == 1) return ""; // Never going to want to display a date when the year is 1, meaning its never been set
            if (parameter == null) parameter = "0";

            switch (parameter.ToString())
            {
                // Returns only the Date
                case "1":
                    return dt.ToShortDateString();

                // Returns only the Time
                case "2":
                    return dt.ToShortTimeString();

                // Returns short datetime
                case "3":
                    return dt.ToShortDateString() + " " + dt.ToShortTimeString();

                // Returns year
                case "4":
                    return dt.Year;

                // Returns Date Time as normal
                default: 
                    return dt.ToShortDateString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class DateAddedToMonthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt = (DateTime)values[0];
            bool exact = (bool)values[1];

            if (dt.Year == 1) return ""; // Never going to want to display a date when the year is 1, meaning its never been set

            if (exact)
                return Utilities.General.GetMonthNameFromNumber(dt.Month);
            else
                return "";

        }

          public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ShowIntIfNotZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)value;
            if (val == 0) return "";
            else return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    class IntToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)value;
            if (val == 0) return "No";
            else return "Yes";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    class DecimalConverter : IValueConverter
    {
        /// Parameters: DecimalPlaces,ShowIfZero
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (decimal)value;
            var paras = parameter.ToString().Split(',');
            var places = System.Convert.ToInt32(paras[0]);
            var showIfZero = System.Convert.ToBoolean(paras[1]);

            if (!showIfZero && val == 0m)
                return "";

          return 0;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    // Test converter
    class BoolToEnabledColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;

            if (val) return "White";
            else return "LightGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    class PercentageKeyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)value;
            return GameBL.LoadedData.PercentageList.FirstOrDefault(x=>x.ItemKey == val)?.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }   
    
    class PlatformKeyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)value;
            return GameBL.Platform.KeyToName(val);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
