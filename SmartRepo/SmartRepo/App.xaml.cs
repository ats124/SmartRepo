using Prism.Unity;
using Xamarin.Forms;

namespace Softentertainer.SmartRepo
{
    using Views;

    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync(nameof(MainPage));
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<DailyReportSelectPage>();
            Container.RegisterTypeForNavigation<DailyReportInputPage>();
            Container.RegisterTypeForNavigation<ReportConfirmPage>();
            Container.RegisterTypeForNavigation<WeeklyReportSelectPage>();
            Container.RegisterTypeForNavigation<SettingsPage>();
        }
    }
}
