using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AiEffects.TestApp.Views
{
    public partial class FloatingPage : ContentPage
    {
        public FloatingPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("", "Tap", "OK");
        }

        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            DisplayAlert("", "FloatingTap", "OK");
        }
    }
}
