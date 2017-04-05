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
    /// 設定画面のビューモデル
    /// </summary>
    public class SettingPageViewModel : BindableBase
    {
        public SettingPageViewModel(INavigationService navigationService)
        {
        }
    }
}
