using System;
using Reactive.Bindings;
using Prism.Services;
using System.Collections.ObjectModel;
namespace AiEffects.TestApp.ViewModels
{
    public class ViewCellPageViewModel
    {
        public ReactiveCommand TestCommand { get; } = new ReactiveCommand();
        public ObservableCollection<string> ItemsSource { get; } = new ObservableCollection<string>();

        public ViewCellPageViewModel(IPageDialogService pageDlg)
        {
            for (var i = 0; i < 20;i++){
                ItemsSource.Add("Name");
            }

            TestCommand.Subscribe(async _=>{
                await pageDlg.DisplayAlertAsync("", "Tap", "OK");
            });
        }
    }
}
