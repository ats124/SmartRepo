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

        private string forward;
        public string Forward
        {
            get { return this.forward; }
            set { SetProperty(ref this.forward, value); }
        }

        public DailyReportInputPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
        }
    }
}
