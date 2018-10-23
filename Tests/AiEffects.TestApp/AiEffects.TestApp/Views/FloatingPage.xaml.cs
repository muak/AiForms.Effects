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
            DisplayAlert("", "BaseTap", "OK");
        }

        void GreenTap(object sender, System.EventArgs e)
        {
            DisplayAlert("", "GreenTap", "OK");
        }

        void BlueTap(object sender, System.EventArgs e)
        {
            DisplayAlert("", "BlueTap", "OK");
        }

        void RedTap(object sender, System.EventArgs e)
        {
            DisplayAlert("", "RedTap", "OK");
        }

        void LimeTap(object sender, System.EventArgs e)
        {
            DisplayAlert("", "LimeTap", "OK");
        }

        void NavyTap(object sender, System.EventArgs e)
        {
            DisplayAlert("", "NavyTap", "OK");
        }

        void PinkTap(object sender, System.EventArgs e)
        {
            DisplayAlert("", "PinkTap", "OK");
        }
    }
}
