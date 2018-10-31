using System;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Xamarin.Forms;
using Android.Content;

namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AlterColorStatusbar : IAiEffectDroid
    {
        Window _window;
        Element _element;
        int _orgColor;

        public AlterColorStatusbar(Element element,Context context)
        {
            System.Diagnostics.Debug.WriteLine("Constructor Start");

            _window = (context as FormsAppCompatActivity).Window;
            _element = element;
            _orgColor = _window.StatusBarColor;

            System.Diagnostics.Debug.WriteLine("Constructor Completed");
        }

        public void OnDetachedIfNotDisposed() { }

        public void OnDetached()
        {
            System.Diagnostics.Debug.WriteLine("OnDetached Start");
            var color = new Android.Graphics.Color(_orgColor);
            _window.SetStatusBarColor(color);

            _window = null;
            _element = null;

            System.Diagnostics.Debug.WriteLine("OnDetached Completed");
        }

        public void Update()
        {
            System.Diagnostics.Debug.WriteLine("Update Start");
            var color = AlterColor.GetAccent(_element).ToAndroid();
            _window.SetStatusBarColor(color);

            System.Diagnostics.Debug.WriteLine("Update Completed");
        }
    }
}
