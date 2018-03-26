using System;
using Reactive.Bindings;
namespace AiEffects.TestApp.ViewModels
{
    public class ToFlatButtonPageViewModel
    {
        public ReactiveProperty<bool> On { get; set; } = new ReactiveProperty<bool>(true);

        public ToFlatButtonPageViewModel()
        {
        }
    }
}
