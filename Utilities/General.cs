using AutoMapper;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Utilities
{
    public static class General
    {
        public static bool IntToBool(int i)
        {
            if (i == 0) return false;
            else return true;
        }

        public static int BoolToInt(bool b)
        {
            if (b) return 1;
            else return 0;
        }

        public static string GetRating(int rating)
        {
            if (rating == 1)
                return "Don't Recommend";
            else if (rating == 2)
                return "Nuetral";
            else if (rating == 3)
                return "Recommend";
            else
                return "";
        }

        public static string GetRatingOrNone(int rating)
        {
            if (rating == 0)
                return "None";
            else
                return GetRating(rating);
        }

        public static string GetMonthNameFromNumber(int num)
        {
            switch (num)
            {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
            }

            return "";
        }

        public static string GetShortMonthNameFromNumber(int num)
        {
            switch (num)
            {
                case 1: return "Jan";
                case 2: return "Feb";
                case 3: return "Mar";
                case 4: return "Apr";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "Aug";
                case 9: return "Sep";
                case 10: return "Oct";
                case 11: return "Nov";
                case 12: return "Dec";
            }

            return "";
        }

        public static string CalculatePercentString(int num1, int num2, int places)
        {
            float perc = (float)num1 / (float)num2;
            perc = perc * 100;
            var percent = perc.ToString();

            if (percent.Length > places) percent = percent.Substring(0, places);

            return percent;
        }


        public static int GetNextKey(int count = 1, string keyType = "")
        {
           return DBFunctions.GetNextKey(count, keyType);
        }

        public static T Map<W, T>(W source)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<W, T>(); });
            IMapper mapper = config.CreateMapper();
            var dest = mapper.Map<W, T>(source);
            return dest;
        }

        /// <summary>
        /// Returns a Cloned list
        /// </summary>
        public static T CloneList<T>(T oldList)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, oldList);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);

        }





    }
}
