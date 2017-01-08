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
    /// 報告書入力画面のビューモデル
    /// </summary>
    public class DailyReportInputPageViewModel : BindableBase
    {
        private string mailAddress;
        public string MailAddress
        {
            get { return this.mailAddress; }
            set { SetProperty(ref this.mailAddress, value); }
        }

        private string comment;
        public string Comment
        {
            get { return this.comment; }
            set { SetProperty(ref this.comment, value); }
        }

        private string nextSchedule;
        public string NextSchedule
        {
            get { return this.nextSchedule; }
            set { SetProperty(ref this.nextSchedule, value); }
        }

        public DelegateCommand ConfirmButton { get; }

        public DailyReportInputPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            this.ConfirmButton = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync(nameof(ReportConfirmPage), new NavigationParameters()
                {
                    { "Message", $"お疲れ様です。hogehogeです。{Environment.NewLine}{this.Comment}" }
                });
            });
        }
    }
}
