using System;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(PlaceholderPlatformEffect), nameof(Placeholder))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class PlaceholderPlatformEffect : AiEffectBase
    {
        EditText _editText;

        protected override void OnAttachedOverride()
        {
            _editText = Control as EditText;

            UpdateText();
            UpdateColor();
        }

        protected override void OnDetachedOverride()
        {
            if (!IsDisposed) {
                _editText.Hint = string.Empty;
                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }
            _editText = null;
            System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached completely");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (!IsSupportedByApi)
                return;

            if (IsDisposed) {
                return;
            }

            if (e.PropertyName == Placeholder.TextProperty.PropertyName) {
                UpdateText();
            }
            else if (e.PropertyName == Placeholder.ColorProperty.PropertyName) {
                UpdateColor();
            }
        }

        void UpdateText()
        {
            _editText.Hint = Placeholder.GetText(Element);
        }

        void UpdateColor()
        {
            _editText.SetHintTextColor(Placeholder.GetColor(Element).ToAndroid());
        }
    }
}
