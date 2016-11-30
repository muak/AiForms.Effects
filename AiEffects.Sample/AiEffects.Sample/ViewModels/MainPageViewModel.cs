using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using AiForms.Effects;

namespace AiEffects.Sample.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private string _title;
        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private DelegateCommand _GoAddCommand;
        public DelegateCommand GoAddCommand {
            get { return _GoAddCommand = _GoAddCommand ?? new DelegateCommand(async() => {
                await Navigation.NavigateAsync("AddCommandPage");
            }); }
        }

        private DelegateCommand _GoLineHeightCommand;
        public DelegateCommand GoLineHeightCommand {
            get { return _GoLineHeightCommand = _GoLineHeightCommand ?? new DelegateCommand(async() => {
                await Navigation.NavigateAsync("AlterLineHeightPage");
            }); }
        }

        private INavigationService Navigation;
        public MainPageViewModel(INavigationService navigationService) {
            Navigation = navigationService;
        }

        public void OnNavigatedFrom(NavigationParameters parameters) {

        }

        public void OnNavigatedTo(NavigationParameters parameters) {
            if (parameters.ContainsKey("title"))
                Title = (string)parameters["title"] + " and Prism";
        }

        public void OnNavigatingTo(NavigationParameters parameters) {
            //throw new NotImplementedException();
        }
    }
}

