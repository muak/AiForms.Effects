using System;
using Reactive.Bindings;
using Xamarin.Forms;

namespace AiEffects.TestApp.ViewModels
{
    public class PlaceholderPageViewModel
    {
        public ReactiveProperty<bool> EffectOn { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> PlaceText { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<Color> PlaceColor { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<bool> ColorToggle { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> TextToggle { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> EditText { get; } = new ReactiveProperty<string>();

        public ReactiveCommand ChangeTextCommand { get; } = new ReactiveCommand();

        public PlaceholderPageViewModel()
        {
            EffectOn.Value = true;


            ColorToggle.Subscribe(x=>{
                PlaceColor.Value = x ? Color.Silver : Color.Red;
            });

            TextToggle.Subscribe(x=>{
                PlaceText.Value = x ? "Placeholder Text" :
                    "Placeholder Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text Text End";
            });

            ColorToggle.Value = true;
            TextToggle.Value = true; 

            ChangeTextCommand.Subscribe(_=>{
                if(string.IsNullOrEmpty(EditText.Value)){
                    EditText.Value = "Abcdef";
                }
                else{
                    EditText.Value = "";
                }

            });
        }
    }
}
