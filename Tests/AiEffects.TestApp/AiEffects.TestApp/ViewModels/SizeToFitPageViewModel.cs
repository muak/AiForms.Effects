using System;
using Reactive.Bindings;
using Xamarin.Forms;

namespace AiEffects.TestApp.ViewModels
{
    public class SizeToFitPageViewModel
    {
        public ReactiveProperty<bool> EffectOn { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> LabelText { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<double> LabelHeight { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<double> LabelWidth { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<double> FontSize { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<bool> CanExpand { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<TextAlignment> TextAlign { get; } = new ReactiveProperty<TextAlignment>();
        public ReactiveProperty<TextAlignment> VTextAlign { get; } = new ReactiveProperty<TextAlignment>();
        public ReactiveProperty<bool> TextToggle { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> HeightToggle { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> WidthToggle { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> FontToggle { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> ExpandToggle { get; } = new ReactiveProperty<bool>();

        public ReactiveCommand HAlignCommand { get; } = new ReactiveCommand();
        public ReactiveCommand VAlignCommand { get; } = new ReactiveCommand();

        public SizeToFitPageViewModel()
        {
            EffectOn.Value = false;

            LabelHeight.Value = 40f;

            TextToggle.Subscribe(x=>{
                if (!x)
                {
                    LabelText.Value = "ShortTextEnd";
                }
                else{
                    LabelText.Value = "LongTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextTextEnd";
                }
            });

            HeightToggle.Subscribe(x=>{
                if(x){
                    LabelHeight.Value = 400f;
                }
                else{
                    LabelHeight.Value = 40f;
                }
            });

            WidthToggle.Subscribe(x=>{
                if(x){
                    LabelWidth.Value = 320f;
                }
                else{
                    LabelWidth.Value = 150f;
                }
            });

            FontToggle.Subscribe(x=>{
                if(x){
                    FontSize.Value = 28f;
                }
                else{
                    FontSize.Value = 14f;
                }
            });


            ExpandToggle.Subscribe(x=>{
                CanExpand.Value = x;
            });
            ExpandToggle.Value = true;

            HAlignCommand.Subscribe(_=>{
                if(TextAlign.Value == TextAlignment.Start){
                    TextAlign.Value = TextAlignment.Center;
                }
                else if(TextAlign.Value == TextAlignment.Center){
                    TextAlign.Value = TextAlignment.End;
                }
                else if(TextAlign.Value == TextAlignment.End){
                    TextAlign.Value = TextAlignment.Start;
                }
            });

            VAlignCommand.Subscribe(_ => {
                if (VTextAlign.Value == TextAlignment.Start)
                {
                    VTextAlign.Value = TextAlignment.Center;
                }
                else if (VTextAlign.Value == TextAlignment.Center)
                {
                    VTextAlign.Value = TextAlignment.End;
                }
                else if (VTextAlign.Value == TextAlignment.End)
                {
                    VTextAlign.Value = TextAlignment.Start;
                }
            });
        }
    }
}
