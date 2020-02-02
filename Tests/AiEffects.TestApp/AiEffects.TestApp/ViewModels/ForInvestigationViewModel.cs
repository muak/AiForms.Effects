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
        public List<string> ItemsSource { get; set; } = new List<string>();

        public ReactiveCommand GoCommand { get; set; } = new ReactiveCommand();

        public ReactiveCommand CellCommand { get; set; } = new ReactiveCommand();

        public ForInvestigationViewModel(INavigationService navigationService)
        {
            BackColor.Value = Color.Blue;

            ItemsSource.Add("ABC");
            ItemsSource.Add("CC");
            ItemsSource.Add("DD");
            ItemsSource.Add("EEE");
            ItemsSource.Add("FFF");
            ItemsSource.Add("GADSFS");
            ItemsSource.Add("HDSGDG");
            ItemsSource.Add("IGDG");
            ItemsSource.Add("YYUY");
            ItemsSource.Add("XXX");

            var toggle = false;
            GoCommand.Subscribe(async _ =>
            {
                //BackColor.Value = toggle ? Color.Blue : Color.Green;
                //toggle = !toggle;
                //await navigationService.NavigateAsync("MainPage",null,true);
                await navigationService.GoBackAsync();
            });

            CellCommand.Subscribe(_ =>
            {
                Debug.WriteLine("Cell Tap!");
            });
        }
    }
}
