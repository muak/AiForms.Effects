using System;
using System.Collections.Generic;
using Reactive.Bindings;
using System.Linq;
using Prism.Navigation;

namespace AiEffects.TestApp.ViewModels
{
    public class FloatingPageViewModel:INavigatedAware
    {
        public ReactiveCommand NextCommand { get; set; } = new ReactiveCommand();
        public ReactivePropertySlim<bool> Visible1 { get; set; } = new ReactivePropertySlim<bool>(false);
        public ReactivePropertySlim<bool> Visible2 { get; set; } = new ReactivePropertySlim<bool>(true);
        public ReactivePropertySlim<bool> Visible3 { get; set; } = new ReactivePropertySlim<bool>(true);
        public ReactivePropertySlim<bool> Visible4 { get; set; } = new ReactivePropertySlim<bool>(true);

        List<ReactivePropertySlim<bool>> _visibles = new List<ReactivePropertySlim<bool>>();

        public FloatingPageViewModel()
        {
            _visibles.Add(Visible1);
            _visibles.Add(Visible2);
            _visibles.Add(Visible3);
            _visibles.Add(Visible4);

            NextCommand.Subscribe(_ =>
            {
                var index = _visibles.FindIndex(x => x.Value == false);
                _visibles[index++].Value = true;

                if(index == _visibles.Count){
                    index = 0;
                }

                _visibles[index].Value = false;
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            //Visible1.Value = true;
            //Visible2.Value = false;
            //Visible3.Value = false;
            //Visible4.Value = false;
        }
    }
}
