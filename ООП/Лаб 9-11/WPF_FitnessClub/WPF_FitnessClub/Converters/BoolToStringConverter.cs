using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_FitnessClub.Converters
{
    /// <summary>
    /// Конвертер, который преобразует значение bool в строку.
    /// Может использоваться двумя способами:
    /// 1. С параметром в формате "ЗначениеДляTrue;ЗначениеДляFalse"
    /// 2. С установленными свойствами TrueValue и FalseValue
    /// </summary>
    public class BoolToStringConverter : IValueConverter
    {
        /// <summary>
        /// Значение, возвращаемое когда входное значение равно true
        /// </summary>
        public string TrueValue { get; set; } = "Да";

        /// <summary>
        /// Значение, возвращаемое когда входное значение равно false
        /// </summary>
        public string FalseValue { get; set; } = "Нет";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? "Выполнено" : "Не выполнено";
            }
            return "Не выполнено";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 