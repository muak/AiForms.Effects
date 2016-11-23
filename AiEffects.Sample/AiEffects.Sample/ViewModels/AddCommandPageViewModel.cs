using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Services;
using Prism.Navigation;

namespace AiEffects.Sample.ViewModels
{
    public class AddCommandPageViewModel : BindableBase,IDestructible,INavigationAware
    {
        private bool _EffectOn;
        public bool EffectOn {
            get { return _EffectOn; }
            set { SetProperty(ref _EffectOn, value); }
        }

        private DelegateCommand<object> _EffectCommand;
        public DelegateCommand<object> EffectCommand {
            get { return _EffectCommand = _EffectCommand ?? new DelegateCommand<object>((p) => {
                PageDialog.DisplayAlertAsync("情報",p.ToString()+"のコマンドが実行されました。","OK");
            }); }
        }

        private DelegateCommand _GoNextCommand;
        public DelegateCommand GoNextCommand {
            get { return _GoNextCommand = _GoNextCommand ?? new DelegateCommand(async() => {
                await Navigation.NavigateAsync("AddCommandPage");
            }); }
        }

        private IPageDialogService PageDialog;
        private INavigationService Navigation;

        public AddCommandPageViewModel(INavigationService navigationService, IPageDialogService dlg) {
            PageDialog = dlg;
            Navigation = navigationService;
            EffectOn = true;
        }

        public void Destroy() {
           
        }

        public void OnNavigatedFrom(NavigationParameters parameters) {
           
        }

        public void OnNavigatedTo(NavigationParameters parameters) {
           
        }

        public void OnNavigatingTo(NavigationParameters parameters) {
            
        }
    }
}
