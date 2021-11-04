using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DataAccess
{
    /// <summary>
    /// Data Access Util Function
    /// </summary>
    public static class ReflectionFuncs
    {


        /// <summary>
        /// Compares properties of 2 objects of the same class. Returns a list of changed properties
        /// </summary>
        public static List<Property> GetChangedProperties(object obj1, object obj2)
        {
            List<Property> result = new List<Property>();

            if (obj1 == null || obj2 == null)
                // just return empty result
                return result; 

            if (obj1.GetType() != obj2.GetType())
                throw new InvalidOperationException("Two objects should be from the same type");

            Type objectType = obj1.GetType();
            // check if the objects are primitive types
            if (objectType.IsPrimitive || objectType == typeof(Decimal) || objectType == typeof(String))
            {
                return result;
            }

            var properties = objectType.GetProperties();
            foreach (var property in properties)
            {
                if (!object.Equals(property.GetValue(obj1), property.GetValue(obj2)))
                {
                    Property newProperty = new Property();
                    newProperty.Name = property.Name;
                    newProperty.Value = property.GetValue(obj1);
                    newProperty.OldValue = property.GetValue(obj2);

                    result.Add(newProperty);
                }
            }
           
            return result;
        }

        /// <summary>
        /// Returns a list of all valid properties
        /// </summary>
        public static List<Property> GetAllValidProperties(object obj1, string containing = "")
        {
            List<Property> result = new List<Property>();

            if (obj1 == null)
                // just return empty result
                return result;

            Type objectType = obj1.GetType();
            var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var property in properties)
            {
                Property newProperty = new Property();
                newProperty.Name = property.Name;
                newProperty.Value = property.GetValue(obj1);

                if (property.PropertyType == typeof(DateTime))
                    if (!ValidDateTime(newProperty.Value)) continue; 

                if(containing != "")
                {
                    if (!property.Name.ToLower().Contains(containing.ToLower()))
                    {
                        continue;
                    }
                }


                result.Add(newProperty);
            }

            return result;
        }

        public static bool ValidDateTime(object value)
        {
            string nullString = value as string;
            if (nullString?.ToLower() == "null")
                return false;

            DateTime date = (DateTime)value;
            if (date.Year == 1) return false;  // Datetime default value is "1/1/0001", year 1 means it was never set

            return true;
        }

        /// <summary>
        /// Gets a single object property based off the property Name
        /// </summary>
        public static Property GetPropertyInfo(object obj, string name)
        {
            Type objectType = obj.GetType();
            if (objectType.IsPrimitive || objectType == typeof(Decimal) || objectType == typeof(String))
                return null;

            var properties = objectType.GetProperties();
            PropertyInfo prop = properties.SingleOrDefault(x => x.Name == name);

            Property newProperty = new Property();
            newProperty.Name = prop.Name;
            newProperty.Value = prop.GetValue(obj);
            return newProperty;

        }


        
        

    }
}
