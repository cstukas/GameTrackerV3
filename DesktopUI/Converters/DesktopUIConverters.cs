﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GameBL;

namespace DesktopUI.Converters
{
    class UserKeyVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userKeyShowing = (int)value;
            if (userKeyShowing == Utilities.UserUtils.CurrentUser.UserKey)
                return "Visible";
            else
                return "Collapsed";


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    
    class OnlyOwnedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var showOnlyOwn = (bool)value;
            if (showOnlyOwn) return "Collapsed";
            else return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    class NoMemoVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var memo = (string)value;
            if (memo != "") return "Hidden";
            else return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    class UserKeyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userKey = (int)value;
            return Utilities.UserUtils.ConvertUserKeyToName(userKey);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    
 
    class GenreKeyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gKey = (int)value;
            var name = GameBL.Globals.GenreList.FirstOrDefault(x => x.GenreKey == gKey)?.GenreName;
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    class RemakeTypeKeyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gKey = (int)value;
            var name = LoadedData.RemakeTypeList.FirstOrDefault(x => x.Key == gKey)?.Type;
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    class GameKeyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gKey = (int)value;
            var name = LoadedData.AllGames.FirstOrDefault(x => x.GameKey == gKey)?.Name;
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


    class RatingKeyToName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rating = (int)value;
            return Utilities.General.GetRating(rating);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class RatingKeyToNameOrNone : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rating = (int)value;
            return Utilities.General.GetRatingOrNone(rating);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class FriendButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userKey = (int)value;
            if (userKey == Utilities.UserUtils.CurrentUser.UserKey)
            {
                return "Hidden";
            }
            else
            {
                return "Visible";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class DimensionTypeToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dimType = (int)value;
            if (dimType == 1)
                return "2D";
            else if (dimType == 2)
                return "3D";
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    class PercentBeatenToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
               var val = (int)value;
            if (val == 0) return System.Windows.Media.Brushes.White;
            else if (val == 1) return System.Windows.Media.Brushes.LightGreen;
            else if (val == 2) return System.Windows.Media.Brushes.MediumSeaGreen;
            else if (val == 3) return System.Windows.Media.Brushes.Green;
            else return System.Windows.Media.Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




}
