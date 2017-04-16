using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace Softentertainer.SmartRepo.ViewModels
{
    using Views;
    using Models;

    /// <summary>
    /// 日報選択画面のビューモデル
    /// </summary>
    public class DailyReportSelectPageViewModel : BindableBase
    {
        private CalendarView.YearMonth viewMonth = CalendarView.YearMonth.ThisMonth;
        public CalendarView.YearMonth ViewMonth
        {
            get { return this.viewMonth;  }
            set { SetProperty(ref this.viewMonth, value, new Action(this.LoadReportExists)); }
        }

        private IDictionary<DateTime, object> reportExists;
        public IDictionary<DateTime, object> ReportExists
        {
            get { return this.reportExists; }
            set { SetProperty(ref this.reportExists, value); }
        }

        public DelegateCommand<DateTime?> CalendarItemCommand { get; }

        public DailyReportSelectPageViewModel(INavigationService navigationService)
        {
            this.CalendarItemCommand = new DelegateCommand<DateTime?>(async date =>
            {
                await navigationService.NavigateAsync(
                    nameof(DailyReportInputPage), 
                    new NavigationParameters()
                    {
                        { "Date", date }
                    });
            });
            this.LoadReportExists();
        }

        private void LoadReportExists()
        {
            //this.ReportExists = new Dictionary<DateTime, object>() { { new DateTime(2017, 4, 17), "有" } };
            this.ReportExists = Models.DailyReport.GetReportExistsDateInMonth(this.viewMonth.Year, this.viewMonth.Month).ToDictionary(x => x, x => (object)"有");
        }
    }
}
