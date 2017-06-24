using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Support.V7.Widget;
using AiForms.Effects.Droid;
using AiForms.Effects;

[assembly: ExportEffect(typeof(AlterColorPlatformEffect), nameof(AlterColor))]
namespace AiForms.Effects.Droid
{
    public class AlterColorPlatformEffect : PlatformEffect
    {
        IAlterColorEffect _effect;

        protected override void OnAttached()
        {
            var renderer = Container as IVisualElementRenderer;

            if (Element is Slider) {
                _effect = new AlterColorSlider(Control as SeekBar, Element, renderer);
            }
            else if (Element is Xamarin.Forms.Switch) {
                _effect = new AlterColorSwitch(Control as SwitchCompat, Element, renderer);
            }
            else if (Element is Entry || Element is Editor) {
                _effect = new AlterColorTextView(Control as TextView, Element, renderer);
            }
            else if (Element is Page) {
                _effect = new AlterColorStatusbar(Element, renderer);
            }
            else {
                return;
            }

            _effect?.Update();
        }

        protected override void OnDetached()
        {
            _effect?.OnDetached();
            _effect = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == AlterColor.AccentProperty.PropertyName) {
                _effect?.Update();
            }
        }
    }
}
