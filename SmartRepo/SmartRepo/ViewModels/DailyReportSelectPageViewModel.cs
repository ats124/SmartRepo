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

    /// <summary>
    /// 日報選択画面のビューモデル
    /// </summary>
    public class DailyReportSelectPageViewModel : BindableBase
    {
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
        }
    }
}
