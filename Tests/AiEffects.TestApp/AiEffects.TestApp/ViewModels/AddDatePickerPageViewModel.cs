using System;
using Reactive.Bindings;
using Prism.Services;

namespace AiEffects.TestApp.ViewModels
{
    public class AddDatePickerPageViewModel
    {
        public ReactiveProperty<bool> EffectOn { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<DateTime> LabelTime { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<DateTime> BoxTime { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<DateTime> StackTime { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<DateTime> ButtonTime { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<DateTime> ImageTime { get; } = new ReactiveProperty<DateTime>();

        public ReactiveCommand<DateTime> SelectedCommand{get;} = new ReactiveCommand<DateTime>();

        public AddDatePickerPageViewModel(IPageDialogService pageDlg)
        {
            EffectOn.Value = true;

            LabelTime.Value = DateTime.Today;
            BoxTime.Value = new DateTime(2016,12,15);
            StackTime.Value = new DateTime(2018,3,1);
            ButtonTime.Value = new DateTime(2020,8,15);
            ImageTime.Value = new DateTime(2015,12,20);

            SelectedCommand.Subscribe(d=>{
                pageDlg.DisplayAlertAsync("",d.ToString("d"),"OK");
            });
        }
    }
}
