using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(AlterColorPlatformEffect), nameof(AlterColor))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AlterColorPlatformEffect : AiEffectBase
    {
        IAiEffectDroid _effect;

        protected override void OnAttachedOverride()
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

        protected override void OnDetachedOverride()
        {
            if (!IsDisposed) {
                _effect?.OnDetachedIfNotDisposed();
                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }
            _effect?.OnDetached();
            _effect = null;
            System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached completely");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (!IsSupportedByApi)
                return;

            if (IsNullOrDisposed) {
                return;
            }

            if (args.PropertyName == AlterColor.AccentProperty.PropertyName) {
                _effect?.Update();
            }
        }
    }
}
