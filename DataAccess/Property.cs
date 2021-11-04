using System;

namespace DataAccess
{
    public class Property
    {
        public string Name { get; set; }

        private object _value;

        public object Value
        {
            get
            {
                // Handles Datetimes we want to set as null
                DateTime? date = _value as DateTime?;
                if(date != null)
                {
                    if(date.Value.Year < 1800)
                    {
                        return "NULL";
                    }
                }


                return _value;
            }
            set { _value = value; }
        }


        public object OldValue { get; set; }
    }
}
