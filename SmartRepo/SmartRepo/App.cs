using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Prism.Unity;

namespace Softentertainer.SmartRepo
{
    using Views;

    public class App : PrismApplication
    {
        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<ReportSelectPage>();
            Container.RegisterTypeForNavigation<DailyReportInputPage>();
        }

        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync(nameof(ReportSelectPage));
        }
    }
}
