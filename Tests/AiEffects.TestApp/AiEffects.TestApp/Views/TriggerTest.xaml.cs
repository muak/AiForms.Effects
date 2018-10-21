using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Linq;
using AiForms.Effects;

namespace AiEffects.TestApp.Views
{
    public partial class TriggerTest : ContentPage
    {
        public TriggerTest()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var result = (
                FeedbackColor.Effects.OfType<FeedbackRoutingEffect>().Any() &&
                FeedbackSound.Effects.OfType<FeedbackRoutingEffect>().Any() &&
                BorderWidth.Effects.OfType<BorderRoutingEffect>().Any() &&
                BorderRadius.Effects.OfType<BorderRoutingEffect>().Any() &&
                AddTextText.Effects.OfType<AddTextRoutingEffect>().Any() &&
                ToFlatButtonRippleColor.Effects.OfType<ToFlatButtonRoutingEffect>().Any() &&
                AddCommandCommand.Effects.OfType<AddCommandRoutingEffect>().Any() &&
                AddCommandLongCommand.Effects.OfType<AddCommandRoutingEffect>().Any() &&
                AddNumberPickerNumber.Effects.OfType<AddNumberPickerRoutingEffect>().Any() &&
                AddTimePickerTime.Effects.OfType<AddTimePickerRoutingEffect>().Any() &&
                AddDatePickerDate.Effects.OfType<AddDatePickerRoutingEffect>().Any() &&
                AlterLineHeightMultiple.Effects.OfType<AlterLineHeightRoutingEffect>().Any() &&
                AlterColorAccent.Effects.OfType<AlterColorRoutingEffect>().Any() &&
                PlaceholderText.Effects.OfType<PlaceholderRoutingEffect>().Any()
            );

            ResultLabel.Text = result ? "All Passed" : "Failed";
            ResultLabel.TextColor = result ? Color.Green : Color.Red;
        }
    }
}
