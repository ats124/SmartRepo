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
    /// 設定画面のビューモデル
    /// </summary>
    public class SettingsPageViewModel : BindableBase
    {
        private string toName;
        public string ToName
        {
            get { return this.toName; }
            set { SetProperty(ref toName, value, () => Settings.ToName = value);  }
        }

        private string toMailAddress;
        public string ToMailAddress
        {
            get { return this.toMailAddress; }
            set { SetProperty(ref this.toMailAddress, value, () => Settings.ToMailAddress = value);  }
        }

        public SettingsPageViewModel(INavigationService navigationService)
        {
            this.ToName = Settings.ToName;
            this.ToMailAddress = Settings.ToMailAddress;
        }
    }
}
