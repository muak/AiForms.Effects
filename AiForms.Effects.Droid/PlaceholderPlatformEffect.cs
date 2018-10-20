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

        protected override void OnAttached()
        {
            _editText = Control as EditText;

            UpdateText();
            UpdateColor();
        }

        protected override void OnDetached()
        {
            if (!IsDisposed) {
                _editText.Hint = string.Empty;
            }
            _editText = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

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
