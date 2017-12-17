using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Support.V7.Widget;
using Android.Widget;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(AlterColorPlatformEffect), nameof(AlterColor))]
namespace AiForms.Effects.Droid
{
    public class AlterColorPlatformEffect : AiEffectBase
    {
        IAiEffectDroid _effect;

        protected override void OnAttached()
        {
            if (Element is Slider) {
                _effect = new AlterColorSlider(Control as SeekBar, Element);
            }
            else if (Element is Xamarin.Forms.Switch) {
                _effect = new AlterColorSwitch(Control as SwitchCompat, Element);
            }
            else if (Element is Entry || Element is Editor) {
                _effect = new AlterColorTextView(Control as TextView, Element);
            }
            else if (Element is Page) {
                _effect = new AlterColorStatusbar(Element,(Control ?? Container).Context);
            }
            else {
                Device.BeginInvokeOnMainThread(() => AlterColor.SetOn(Element, false));
                return;
            }

            _effect?.Update();
        }

        protected override void OnDetached()
        {
            if (!IsDisposed) {
                _effect?.OnDetachedIfNotDisposed();
            }
            _effect?.OnDetached();
            _effect = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (IsDisposed) {
                return;
            }

            if (args.PropertyName == AlterColor.AccentProperty.PropertyName) {
                _effect?.Update();
            }
        }
    }
}
