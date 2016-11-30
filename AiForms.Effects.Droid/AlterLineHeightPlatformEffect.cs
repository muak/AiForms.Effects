using System;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Xamarin.Forms;
using AiForms.Effects;
using AiForms.Effects.Droid;

[assembly: ExportEffect(typeof(AlterLineHeightPlatformEffect), nameof(AlterLineHeight))]
namespace AiForms.Effects.Droid
{
    public class AlterLineHeightPlatformEffect : PlatformEffect
    {
        private ILineHeightEffect _effect;

        protected override void OnAttached()
        {
            if (Element is Label) {
                _effect = new LineHeightForTextView(Container,Control, Element);
            }
            else if (Element is Editor) {
                _effect = new LineHeightForEditText(Container,Control, Element);
            }

            _effect?.Update();

        }

        protected override void OnDetached()
        {
            _effect?.OnDetached();
            _effect = null;
        }


        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AlterLineHeight.MultipleProperty.PropertyName) {
                _effect?.Update();
            }

            else if (e.PropertyName == Label.FontSizeProperty.PropertyName) {
                _effect?.Update();
            }

        }
    }
}

