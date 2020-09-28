using System;
using Reactive.Bindings;
using Xamarin.Forms;
using Prism.Navigation;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Prism.Mvvm;

namespace AiEffects.TestApp.ViewModels
{
    public class ForInvestigationViewModel:BindableBase, INavigatingAware
    {
        public ReactivePropertySlim<Color> BackColor { get; } = new ReactivePropertySlim<Color>();
        
        public ReactiveCommand GoCommand { get; set; } = new ReactiveCommand();

        public ReactiveCommand HogeCommand { get; set; }

        public ReactiveProperty<bool> On { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<double> Width { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<double> Radius { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<Color> Color { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<bool> WidthToggle { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> RadiusToggle { get; } = new ReactiveProperty<bool>(true);

        public ReactivePropertySlim<bool> CanExecute { get; } = new ReactivePropertySlim<bool>();

        public ForInvestigationViewModel(INavigationService navigationService)
        {
            //BackColor.Value = Color.Blue;            

            var toggle = false;
            GoCommand.Subscribe(async _ =>
            {
                On.Value = !On.Value;
            });

            HogeCommand = CanExecute.ToReactiveCommand();

            HogeCommand.Subscribe(_ =>
            {
                Debug.WriteLine("Cell Tap!");
            });

            Color.Value = Xamarin.Forms.Color.Blue;

            WidthToggle.Subscribe(x => {
                Width.Value = x ? 2.0d : 0.0d;
            });

            RadiusToggle.Subscribe(x => {
                Radius.Value = x ? 8.0d : 0.0d;
            });
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
