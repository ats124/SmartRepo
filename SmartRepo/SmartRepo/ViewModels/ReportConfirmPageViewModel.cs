using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Plugin.Messaging;

namespace Softentertainer.SmartRepo.ViewModels
{
    using Models;
    using Views;

    /// <summary>
    /// 日報確認画面のビューモデル
    /// </summary>
    public class ReportConfirmPageViewModel : BindableBase, INavigationAware
    {
        private string subject;
        public string Subject
        {
            get { return this.subject; }
            set { SetProperty(ref this.subject, value); }
        }

        private string message;
        public string Message
        {
            get { return this.message; }
            set { SetProperty(ref this.message, value); }
        }

        private DailyReport DailyReport { get; set; }

        public DelegateCommand SendReportCommand { get; }

        public ReportConfirmPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            this.SendReportCommand = new DelegateCommand(async () =>
            {
                var emailTask = CrossMessaging.Current.EmailMessenger;
                if (emailTask.CanSendEmail)
                {
                    var email = this.DailyReport.CreateEmail(emailTask, Settings.ToName, Settings.ToMailAddress);
                    emailTask.SendEmail(email);
                }
                else
                {
                    await pageDialogService.DisplayAlertAsync("送信エラー", "メールが送信できません。", "OK");
                }
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            this.DailyReport = (DailyReport)parameters["DailyReport"];
            this.Subject = this.DailyReport.CreateMailTitle();
            this.Message = this.DailyReport.CreateMailBody();
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
