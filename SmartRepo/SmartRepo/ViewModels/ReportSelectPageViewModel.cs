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
    /// 報告書選択画面のビューモデル
    /// </summary>
    public class ReportSelectPageViewModel : BindableBase
    {
        public DelegateCommand DailyCommand { get; }

        public DelegateCommand WeeklyCommand { get; }

        public ReportSelectPageViewModel(INavigationService navigationService)
        {
            this.DailyCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync(nameof(DailyReportInputPage));
            });
        }
    }
}
