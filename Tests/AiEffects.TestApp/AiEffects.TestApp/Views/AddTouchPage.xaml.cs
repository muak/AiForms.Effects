using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AiForms.Effects;

namespace AiEffects.TestApp.Views
{
    public partial class AddTouchPage : ContentPage
    {
        public AddTouchPage()
        {
            InitializeComponent();
            BindingContext = this;
            var recognizer = AddTouch.GetRecognizer(container);

            recognizer.TouchBegin += (sender, e) => {
                eventLabel.Text = "TouchBegin";
                posXLabel.Text = $"X: {e.Location.X}";
                posYLabel.Text = $"Y: {e.Location.Y}";
            };

            recognizer.TouchMove += (sender, e) =>  {
                eventLabel.Text = "TouchMove";
                posXLabel.Text = $"X: {e.Location.X}";
                posYLabel.Text = $"Y: {e.Location.Y}";  
            };

            recognizer.TouchEnd += (sender, e) => {
                eventLabel.Text = "TouchEnd";
                posXLabel.Text = $"X: {e.Location.X}";
                posYLabel.Text = $"Y: {e.Location.Y}";
            };

            recognizer.TouchCancel += (sender, e) => {
                eventLabel.Text = "TouchCancel";
                posXLabel.Text = $"X: {e.Location.X}";
                posYLabel.Text = $"Y: {e.Location.Y}";
            };
        }

    }
}
