using System;
using System.Diagnostics;

namespace ViewModelUtils.Enums
{
    public static class EnumExtensions
    {
        public static T[] GetAttributeValues<T>(this Enum value, Type valueType) where T : class
        {
            var fieldInfo = valueType.GetField(value.ToString());
            return (T[])fieldInfo.GetCustomAttributes(typeof(T), false);
        }

        public static Axle GetAxleByTyrePlacement(this TyrePlacement tyrePlacement)
        {
            var attributes = tyrePlacement.GetAttributeValues<AxleAttribute>(typeof(TyrePlacement));
            if (attributes.Length > 0)
            {
                return attributes[0].Axle;
            }
            throw new Exception($"TyrePlacement {tyrePlacement} has not been given an Axle attribute setting.");
        }

        /// <summary>
        /// Extension method for a string - tries to parse said string as an enum entry for the specified enum type.
        /// </summary>
        public static T ToEnum<T>(this string enumValueAsString) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("ToEnum Error | T must be an enumerated type.");
            }
            if (string.IsNullOrEmpty(enumValueAsString))
            {
                throw new ArgumentException("ToEnum Error | enumValueAsString cannot be null or an empty string.");
            }

            T enumValue;
            var success = Enum.TryParse<T>(enumValueAsString, out enumValue);
            if (!success)
            {
                throw new InvalidCastException($"ToEnum Error | String '{enumValueAsString}' could not be parsed as a {typeof(T)}.");
            }
            Debug.Assert(success);
            return enumValue;
        }
    }
}