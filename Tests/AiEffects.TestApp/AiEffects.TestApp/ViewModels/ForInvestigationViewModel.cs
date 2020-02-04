using System;
using Reactive.Bindings;
using Xamarin.Forms;
using Prism.Navigation;
using System.Diagnostics;
using System.Collections.Generic;

namespace AiEffects.TestApp.ViewModels
{
    public class ForInvestigationViewModel
    {
        public ReactivePropertySlim<Color> BackColor { get; } = new ReactivePropertySlim<Color>();
        
        public ReactiveCommand GoCommand { get; set; } = new ReactiveCommand();

        public ReactiveCommand HogeCommand { get; set; } 

        public ReactivePropertySlim<bool> CanExecute { get; } = new ReactivePropertySlim<bool>();

        public ForInvestigationViewModel(INavigationService navigationService)
        {
            BackColor.Value = Color.Blue;
          

            var toggle = false;
            GoCommand.Subscribe(async _ =>
            {
                CanExecute.Value = !CanExecute.Value;
            });

            HogeCommand = CanExecute.ToReactiveCommand();

            HogeCommand.Subscribe(_ =>
            {
                Debug.WriteLine("Cell Tap!");
            });
        }
    }
}
