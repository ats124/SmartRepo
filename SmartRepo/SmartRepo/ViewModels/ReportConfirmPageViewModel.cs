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

        public DelegateCommand SendReportCommand { get; }

        public ReportConfirmPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            this.SendReportCommand = new DelegateCommand(async () =>
            {
                var emailMessanger = MessagingPlugin.EmailMessenger;
                if (emailMessanger.CanSendEmail)
                {
                    var email = new EmailMessageBuilder()
                        .To("test@contoso.com")
                        .Subject(this.Subject)
                        .Body(this.Message)
                        .Build();
                    emailMessanger.SendEmail(email);
                }
                else
                {
                    await pageDialogService.DisplayAlertAsync("送信エラー", "メールが設定されていません。", "OK");
                }
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            this.Message = (string)parameters["Message"];
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
