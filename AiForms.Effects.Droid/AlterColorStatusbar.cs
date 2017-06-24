using System;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Xamarin.Forms;

namespace AiForms.Effects.Droid
{
    public class AlterColorStatusbar : AlterColorBase, IAlterColorEffect
    {
        Window _window;
        Element _element;
        int _orgColor;

        public AlterColorStatusbar(Element element, IVisualElementRenderer renderer) : base(renderer)
        {
            _window = (Xamarin.Forms.Forms.Context as FormsAppCompatActivity).Window;
            _element = element;
            _orgColor = _window.StatusBarColor;
        }

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
