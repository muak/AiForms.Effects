using System;
using Reactive.Bindings;
using Prism.Services;
using Prism.Navigation;
namespace AiEffects.TestApp.ViewModels
{
    public class TriggerTestViewModel
    {
        public ReactiveCommand TapCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand LongTapCommand { get; set; } = new ReactiveCommand();

        public TriggerTestViewModel(IPageDialogService pageDialog)
        {
            TapCommand.Subscribe(async _ =>
            {
                await pageDialog.DisplayAlertAsync("", "Tap", "OK");
            });

            LongTapCommand.Subscribe(async _ =>
            {
                await pageDialog.DisplayAlertAsync("", "LongTap", "OK");
            });
        }

    }
}
