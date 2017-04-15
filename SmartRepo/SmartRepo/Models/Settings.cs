using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Softentertainer.SmartRepo.Models
{
    using Utils;

    static class Settings
    {
        static readonly IDictionary<string, object> AppProps = Application.Current.Properties;

        public static string ToName
        {
            get { return (string)AppProps.GetValueOrDefault("ToName"); }
            set { AppProps["ToName"] = value; }
        }

        public static string ToMailAddress
        {
            get { return (string)AppProps.GetValueOrDefault("ToMailAddress"); }
            set { AppProps["ToMailAddress"] = value; }
        }
    }
}
