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
    public class MainPageViewModel : BindableBase
    {
        public DelegateCommand<string> NavigationCommand { get; }

        public MainPageViewModel(INavigationService navigationService)
        {
            this.NavigationCommand = new DelegateCommand<string>(async param =>
            {
                await navigationService.NavigateAsync(param);
            });
        }
    }
}
