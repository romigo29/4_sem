using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_FitnessClub.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string parameterString = parameter.ToString();
            
            // Если параметр - это число
            if (int.TryParse(parameterString, out int parameterValue))
            {
                // Если значение также число, сравниваем напрямую
                if (value is int valueInt)
                {
                    return valueInt == parameterValue;
                }
                
                // Если значение - перечисление, конвертируем его в число и сравниваем
                if (value is Enum)
                {
                    return System.Convert.ToInt32(value) == parameterValue;
                }
            }
            
            // Если параметр - строка, пробуем конвертировать ее в перечисление
            if (value is Enum && Enum.IsDefined(value.GetType(), value))
            {
                return value.ToString().Equals(parameterString, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue && parameter != null)
            {
                if (targetType.IsEnum)
                {
                    // Если целевой тип - перечисление и параметр - строка, пробуем преобразовать в перечисление
                    if (parameter is string parameterString)
                    {
                        // Если параметр - число, пробуем конвертировать его в перечисление
                        if (int.TryParse(parameterString, out int intValue))
                        {
                            return Enum.ToObject(targetType, intValue);
                        }
                        
                    }
                    
                    // Если параметр - число, пробуем преобразовать его в перечисление
                    if (parameter is int intParam)
                    {
                        return Enum.ToObject(targetType, intParam);
                    }
                }
            }

            return Binding.DoNothing;
        }
    }
} 