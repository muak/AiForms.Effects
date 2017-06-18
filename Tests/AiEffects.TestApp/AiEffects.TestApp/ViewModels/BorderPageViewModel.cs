using System;
using Prism.Mvvm;
using Reactive.Bindings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AiEffects.TestApp.ViewModels
{
    public class BorderPageViewModel : BindableBase
    {
        public ReactiveProperty<bool> EffectOn { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<double> Radius { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<double> Width { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<bool> WidthToggle { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> RadiusToggle { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<Color> BorderColor { get; } = new ReactiveProperty<Xamarin.Forms.Color>();

        public BorderPageViewModel()
        {
            BorderColor.Value = Color.Blue;

            WidthToggle.Subscribe(x => {
                Width.Value = x ? 2.0d : 0.0d;
            });

            RadiusToggle.Subscribe(x => {
                Radius.Value = x ? 8.0d : 0.0d;
            });
        }
    }
}
