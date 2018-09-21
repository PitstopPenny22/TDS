using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ViewModelUtils.Enums;

namespace Views.Converters
{
    public sealed class EnumDescriptionConverter : IValueConverter
    {
        private static EnumDescriptionConverter _instance;
        public static EnumDescriptionConverter Instance => _instance ?? (_instance = new EnumDescriptionConverter());

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("EnumDescriptionConverter Error | value argument cannot be null.");
            }
            var valueType = value.GetType();
            if (!valueType.IsEnum)
            {
                throw new ArgumentException("EnumDescriptionConverter Error | value must be of an enumerated type.");
            }
            if (parameter != null)
            {
                Debug.WriteLine("EnumDescriptionConverter Warning | parameter argument is not used.");
            }
            return GetEnumDescription(value as Enum, valueType);
        }

        private static string GetEnumDescription(Enum value, Type valueType)
        {
            var attributes = value.GetAttributeValues<DescriptionAttribute>(valueType);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine("EnumDescriptionConverter Warning | ConvertBack is not supported.");
            return DependencyProperty.UnsetValue;
        }
    }
}