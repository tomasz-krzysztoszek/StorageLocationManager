using Application.Services;
using Model;
using Model.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace ViewModel.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        private static Statuscolors _statusColorsInstance;
        public StatusToColorConverter() { }
        public StatusToColorConverter(AppProfile appProfile)
        {
            _statusColorsInstance = appProfile.StatusColors;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = Enum.Parse(typeof(StatusMscSkl), value.ToString());
            switch (status)
            {
                case StatusMscSkl.Empty:
                    return _statusColorsInstance.Empty;
                case StatusMscSkl.Busy:
                    return _statusColorsInstance.Busy;
                case StatusMscSkl.TpsLessThan6Month:
                    return _statusColorsInstance.TpsLessThan6Month;
                case StatusMscSkl.TpsLessThan3Month:
                    return _statusColorsInstance.TpsLessThan3Month;
                default:
                    return _statusColorsInstance.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
