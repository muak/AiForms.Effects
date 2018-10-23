using System;
using Reactive.Bindings;
using Prism.Services;
using System.Collections.ObjectModel;
using Prism.Navigation;
namespace AiEffects.TestApp.ViewModels
{
    public class ViewCellPageViewModel
    {
        public ReactiveCommand TestCommand { get; } = new ReactiveCommand();
        public ObservableCollection<string> ItemsSource { get; } = new ObservableCollection<string>();

        public ViewCellPageViewModel(INavigationService navigationService, IPageDialogService pageDlg)
        {
            for (var i = 0; i < 20;i++){
                ItemsSource.Add("Name");
            }

            TestCommand.Subscribe(async _=>{
                await navigationService.GoBackAsync(null);
                //await pageDlg.DisplayAlertAsync("", "Tap", "OK");

            });
        }
    }
}
