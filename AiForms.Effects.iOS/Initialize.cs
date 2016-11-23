using System;
namespace AiForms.Effects.iOS
{
    
    public class MyAppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

    }

    public static class Effects
    {
        public static void Init() {
#pragma warning disable RECS0026 // Possible unassigned object created by 'new'
            new MyAppDelegate();
#pragma warning restore RECS0026 // Possible unassigned object created by 'new'
        }
    }
}
