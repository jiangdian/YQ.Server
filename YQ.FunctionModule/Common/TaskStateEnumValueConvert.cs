

namespace YQ.FunctionModule.Common
{
    public class TaskStateEnumValueConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is TaskStateEnum)
            //{
                return ((SerialPoartEnum)value).GetDescription();
            //}
            //else
            //{
            //    return value;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString()!.GetEnumName<SerialPoartEnum>();
        }
    }
}
