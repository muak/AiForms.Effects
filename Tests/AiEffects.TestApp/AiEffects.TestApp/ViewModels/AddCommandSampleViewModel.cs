using System;
using Reactive.Bindings;
using Prism.Services;

namespace AiEffects.TestApp.ViewModels
{
    public class AddCommandSampleViewModel
    {
        public ReactiveCommand TapCommand { get; set; }
        public ReactiveCommand LongCommand { get; set; }
        public ReactiveProperty<bool> CanExecute { get; } = new ReactiveProperty<bool>(true);

        public AddCommandSampleViewModel(IPageDialogService pageDlg)
        {
            TapCommand = CanExecute.ToReactiveCommand();
            LongCommand = CanExecute.ToReactiveCommand();

            TapCommand.Subscribe(async _ =>{
                await pageDlg.DisplayAlertAsync("", $"Single Tap in {_}", "OK");
            });

            LongCommand.Subscribe(async _ => {
                await pageDlg.DisplayAlertAsync("", $"Long Tap in {_}", "OK");
            });
        }
    }
}
