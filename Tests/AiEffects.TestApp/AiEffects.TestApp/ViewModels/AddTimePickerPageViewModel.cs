using System;
using Reactive.Bindings;
using Prism.Services;

namespace AiEffects.TestApp.ViewModels
{
    public class AddTimePickerPageViewModel
    {
        public ReactiveProperty<bool> EffectOn { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<TimeSpan> LabelTime { get; } = new ReactiveProperty<TimeSpan>();
        public ReactiveProperty<TimeSpan> BoxTime { get; } = new ReactiveProperty<TimeSpan>();
        public ReactiveProperty<TimeSpan> StackTime { get; } = new ReactiveProperty<TimeSpan>();
        public ReactiveProperty<TimeSpan> ButtonTime { get; } = new ReactiveProperty<TimeSpan>();
        public ReactiveProperty<TimeSpan> ImageTime { get; } = new ReactiveProperty<TimeSpan>();

        public ReactiveCommand<TimeSpan> SelectedCommand{get;} = new ReactiveCommand<TimeSpan>();

        public AddTimePickerPageViewModel(IPageDialogService pageDlg)
        {
            EffectOn.Value = true;

            LabelTime.Value = TimeSpan.FromHours(3.5);
            BoxTime.Value = TimeSpan.FromHours(0);
            StackTime.Value = TimeSpan.FromHours(12.2);
            ButtonTime.Value = TimeSpan.FromHours(23.0);
            ImageTime.Value = TimeSpan.FromHours(6.5);

            SelectedCommand.Subscribe(t=>{
                pageDlg.DisplayAlertAsync("",t.ToString("c"),"OK");
            });
        }
    }
}
