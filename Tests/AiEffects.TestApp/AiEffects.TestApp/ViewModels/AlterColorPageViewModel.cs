using System;
using Prism.Mvvm;
using Reactive.Bindings;
using Xamarin.Forms;
using System.Diagnostics;

namespace AiEffects.TestApp.ViewModels
{
    public class AlterColorPageViewModel:BindableBase
    {
        public ReactiveProperty<bool> EffectOn { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> ColorToggle { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<Color> Accent { get; } = new ReactiveProperty<Color>();

        public AlterColorPageViewModel()
        {
            ColorToggle.Subscribe(x=>{
                Accent.Value = x ? Color.FromHex("#2455b3") : Color.FromHex("#CCA3B0");
            });
        }
    }
}
