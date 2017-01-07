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
    /// 日報確認画面のビューモデル
    /// </summary>
    public class ReportConfirmPageViewModel : BindableBase, INavigationAware
    {
        private string message;
        public string Message
        {
            get { return this.message; }
            set { SetProperty(ref this.message, value); }
        }

        public DelegateCommand SendReport { get; }

        public ReportConfirmPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            this.SendReport = new DelegateCommand(async () =>
            {
                await pageDialogService.DisplayAlertAsync("報告書", "報告書を送信しました", null);
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            this.Message = (string)parameters["Message"];
        }
    }
}
