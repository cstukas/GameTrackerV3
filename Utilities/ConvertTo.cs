using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ConvertTo
    {

        /// <summary>
        /// Returns the key with leading 0s
        /// </summary>
        static string ConvertToKey(int key, int digits)
        {
            StringBuilder displayKey = new StringBuilder();
            displayKey.Append(key.ToString());
            for (int i = displayKey.Length; i < digits; i++)
            {
                displayKey.Insert(0, "0");
            }
            return displayKey.ToString();
        }


        /// <summary>
        /// Converts Key to Normal length
        /// </summary>
        public static string ConvertTo6DigitKey(int key)
        {
            return ConvertToKey(key, 6);
        }

        /// <summary>
        /// Converts Key to Short length
        /// </summary>
        public static string ConvertTo2DigitKey(int key)
        {
            return ConvertToKey(key, 2);
        }


    }
}
