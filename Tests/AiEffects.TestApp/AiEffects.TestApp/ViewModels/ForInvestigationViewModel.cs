using System;
using Reactive.Bindings;
using Xamarin.Forms;
using Prism.Navigation;
namespace AiEffects.TestApp.ViewModels
{
    public class ForInvestigationViewModel
    {
        public ReactivePropertySlim<Color> BackColor { get; } = new ReactivePropertySlim<Color>();
        public ReactiveCommand GoCommand { get; set; } = new ReactiveCommand();


        public ForInvestigationViewModel(INavigationService navigationService)
        {
            BackColor.Value = Color.Blue;

            var toggle = false;
            GoCommand.Subscribe(async _ =>
            {
                //BackColor.Value = toggle ? Color.Blue : Color.Green;
                //toggle = !toggle;
                await navigationService.NavigateAsync("MainPage",null,true);
            });
        }
    }
}
