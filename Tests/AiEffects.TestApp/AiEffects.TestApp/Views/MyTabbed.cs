using System;
using Xamarin.Forms;
namespace AiEffects.TestApp.Views
{
    public class MyTabbed:TabbedPage
    {
        public MyTabbed() {
           Children.Add(new AlterLineHeightPage { Title = "Tab2" });
           Children.Add(new AddCommandPage { Title = "Tab1" });
           Children.Add(new ContentPage { Title = "Tab3", BackgroundColor = Color.Green });
        }


    }

    public class LineHeightTabbed : TabbedPage
    {
        public LineHeightTabbed()
        {
            if (Device.OS == TargetPlatform.iOS) {
                Children.Add(new AlterLineHeightPage { Title = "Tab2" });
                Children.Add(new ContentPage{ Title = "Tab1" });
                Children.Add(new ContentPage { Title = "Tab3", BackgroundColor = Color.Green });
            }
            else {
                
            }
        }
    }
}
