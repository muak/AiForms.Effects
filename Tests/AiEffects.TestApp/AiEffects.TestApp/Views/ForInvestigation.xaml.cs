using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AiEffects.TestApp.Views
{
    public partial class ForInvestigation : ContentPage
    {
        public ForInvestigation()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
