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
            _window = (context as FormsAppCompatActivity).Window;
            _element = element;
            _orgColor = _window.StatusBarColor;
        }

        public void OnDetachedIfNotDisposed() { }

        public void OnDetached()
        {
            var color = new Android.Graphics.Color(_orgColor);
            _window.SetStatusBarColor(color);

            _window = null;
            _element = null;
        }

        public void Update()
        {
            var color = AlterColor.GetAccent(_element).ToAndroid();
            _window.SetStatusBarColor(color);
        }
    }
}
