using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Softentertainer.SmartRepo.Utils
{
    /// <summary>
    /// デリゲートを呼び出す汎用値コンバータ。
    /// </summary>
    public class DelegateValueConverter : IValueConverter
    {
        private Func<object, Type, object, CultureInfo, object> convertFunc;
        private Func<object, Type, object, CultureInfo, object> convertBackFunc;

        public DelegateValueConverter(Func<object, Type, object, CultureInfo, object> convertFunc, Func<object, Type, object, CultureInfo, object> convertBackFunc = null)
        {
            this.convertFunc = convertFunc;
            this.convertBackFunc = convertBackFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.convertFunc != null 
                ? this.convertFunc(value, targetType, parameter, culture) 
                : throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.convertBackFunc != null
                ? this.convertBackFunc(value, targetType, parameter, culture)
                : throw new NotImplementedException();
        }
    }
}
