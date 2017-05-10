using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Reactive.Bindings;

namespace AiEffects.TestApp.ViewModels
{
    public class AddTextPageViewModel : BindableBase
    {
        public ReactiveProperty<TextAlignment> HAlign { get; } = new ReactiveProperty<TextAlignment>();
        public ReactiveProperty<TextAlignment> VAlign { get; } = new ReactiveProperty<TextAlignment>();
        public ReactiveProperty<int> Margin { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<double> FontSize { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<Color> TextColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<string> Text { get; } = new ReactiveProperty<string>();

        public ReactiveCommand ChangeHAlign{get;} = new ReactiveCommand();
        public ReactiveCommand ChangeVAlign { get; set; } = new ReactiveCommand();

        public AddTextPageViewModel()
        {
            HAlign.Value = TextAlignment.Start;
            VAlign.Value = TextAlignment.Start;

            ChangeHAlign.Subscribe(x=>{
                if(HAlign.Value == TextAlignment.End){
                    HAlign.Value = TextAlignment.Start;
                }
                else{
                    HAlign.Value = TextAlignment.End;
                }
            });

            ChangeVAlign.Subscribe(_=>{
                if(VAlign.Value == TextAlignment.End){
                    VAlign.Value = TextAlignment.Start;
                }
                else{
                    VAlign.Value = TextAlignment.End;
                }
            });
        }
    }
}
