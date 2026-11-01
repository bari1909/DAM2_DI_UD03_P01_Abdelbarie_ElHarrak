using System;
using System.Globalization;
using System.Windows.Data;

namespace DAM2_DI_UD03_P01.Converters
{
    // RadioButton: Selector de prioridad exclusivo (Baja, Media, Alta) con GroupName="GrpPrioridad"
    public class PrioridadToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? prioridad = value as string;
            string? parametro = parameter as string;
            return prioridad == parametro;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool marcado && marcado)
            {
                return parameter;
            }

            return Binding.DoNothing;
        }
    }
}
