using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DesktopUI.Converters
{
    /// <summary>
    /// Converters a DateTime to WPF Visibility strings Base off if YEAR is 1
    /// Parameters: String1|String2
    /// Use a pipe | to seperate
    /// String1: Visibility string when value is True;
    /// String2: Visibility string when value is False;
    /// </summary>
    class DateTimeToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) throw new Exception("Parameter can't be null");
            var dt = (DateTime)value;

            string paras = (string)parameter;

            string[] words = paras.Split('|');
            string trueState = words[0].Trim();
            string falseState = words[1].Trim();

            if (!CheckIfVisibilityString(trueState) || !CheckIfVisibilityString(falseState))
            {
                throw new Exception("Parameters are spelled wrong. Need to be a Visibility string");
            }

            if (dt.Year != 1) return trueState;
            else return falseState;
        }

        public bool CheckIfVisibilityString(string word)
        {
            word = word.ToLower();
            if (word == "visible" || word == "collapsed" || word == "hidden")
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




    /// <summary>
    /// Converters a bool to WPF Visibility strings
    /// Parameters: String1|String2
    /// Use a pipe | to seperate
    /// String1: Visibility string when value is True;
    /// String2: Visibility string when value is False;
    /// </summary>
    class BoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) throw new Exception("Parameter can't be null");
            bool val = (bool)value;

            string paras = (string)parameter;

            string[] words = paras.Split('|');
            string trueState = words[0].Trim();
            string falseState = words[1].Trim();

            if (!CheckIfVisibilityString(trueState) || !CheckIfVisibilityString(falseState))
            {
                throw new Exception("Parameters are spelled wrong. Need to be a Visibility string");
            }

            if (val) return trueState;
            else return falseState;
        }

        public bool CheckIfVisibilityString(string word)
        {
            word = word.ToLower();
            if (word == "visible" || word == "collapsed" || word == "hidden")
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    
    class IsTodayToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            if (date.Date == DateTime.Now.Date)
                return "Visible";
            else
                return "Hidden";
        }

        public bool CheckIfVisibilityString(string word)
        {
            word = word.ToLower();
            if (word == "visible" || word == "collapsed" || word == "hidden")
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
