using System;
using Xamarin.Forms;
namespace AiEffects.Sample.Views
{
    public class MyTabbed:TabbedPage
    {
        public MyTabbed() {
            Children.Add(new AddCommandPage { Title = "Tab1" });
            Children.Add(new ContentPage { Title = "Tab2",BackgroundColor=Color.Blue});
            Children.Add(new ContentPage { Title = "Tab3", BackgroundColor = Color.Green});
        }
    }
}
