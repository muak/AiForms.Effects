using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Reactive.Bindings;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;

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

        [Required(ErrorMessage = "Required")]
        [StringLength(20, ErrorMessage = "less than 20")]
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<string> ErrorTitle { get; }

        public ReactiveProperty<string> Description { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> DescriptionCount { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<Color> DescriptionColor { get; } = new ReactiveProperty<Color>();

        public ReactiveCommand ChangeHAlign { get; } = new ReactiveCommand();
        public ReactiveCommand ChangeVAlign { get; set; } = new ReactiveCommand();

        public AddTextPageViewModel()
        {
            Title.SetValidateAttribute(() => this.Title);


            this.ErrorTitle = this.Title
                .ObserveErrorChanged
                .Select(x => x?.Cast<string>()?.FirstOrDefault())
                .ToReadOnlyReactiveProperty();

            DescriptionColor.Value = Color.Silver;
            Description.Subscribe(x => {
                var cnt = string.IsNullOrEmpty(x) ? 0 : x.Length;
                DescriptionCount.Value = $"{cnt}/140";
                if (cnt > 140) {
                    DescriptionColor.Value = Color.Red;
                }
                else {
                    DescriptionColor.Value = Color.Silver;
                }
            });


            HAlign.Value = TextAlignment.Start;
            VAlign.Value = TextAlignment.Start;

            ChangeHAlign.Subscribe(x => {
                if (HAlign.Value == TextAlignment.End) {
                    HAlign.Value = TextAlignment.Start;
                }
                else {
                    HAlign.Value = TextAlignment.End;
                }
            });

            ChangeVAlign.Subscribe(_ => {
                if (VAlign.Value == TextAlignment.End) {
                    VAlign.Value = TextAlignment.Start;
                }
                else {
                    VAlign.Value = TextAlignment.End;
                }
            });
        }
    }
}
