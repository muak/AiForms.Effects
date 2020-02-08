using System;
using System.Collections.Generic;
using AiForms.Effects;
using Prism.Mvvm;
using Reactive.Bindings;
using Xamarin.Forms;

namespace AiEffects.TestApp.ViewModels
{
    public class GradientPageViewModel:BindableBase
    {
        public ReactiveProperty<bool> EffectOn { get; } = new ReactiveProperty<bool>(false);
        public ReactivePropertySlim<GradientColors> Colors { get; } = new ReactivePropertySlim<GradientColors>();
        public ReactivePropertySlim<GradientOrientation> Orientation { get; } = new ReactivePropertySlim<GradientOrientation>();

        public ReactiveCommand<string> ChangeColorsCommand { get; } = new ReactiveCommand<string>();
        public ReactiveCommand<string> ChangeOrientationCommand { get; } = new ReactiveCommand<string>();

        GradientColors _colors1 = new GradientColors(new List<Color>
        {
            Color.Blue,Color.LightGray
        });
        GradientColors _colors2 = new GradientColors(new List<Color>
        {
            Color.Red, Color.Green,Color.LightGray
        });

        public GradientPageViewModel()
        {
            Colors.Value = _colors1;
            Orientation.Value = GradientOrientation.LeftRight;

            ChangeColorsCommand.Subscribe(x =>
            {
                var p = int.Parse(x);
                Colors.Value = p == 2 ? _colors1 : _colors2;
            });

            ChangeOrientationCommand.Subscribe(x =>
            {
                var p = int.Parse(x);
                Orientation.Value = (GradientOrientation)p;
            });
        }
    }
}
