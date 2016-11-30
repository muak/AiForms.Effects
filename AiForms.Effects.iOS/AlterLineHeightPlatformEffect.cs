using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using Foundation;
using AiForms.Effects;
using AiForms.Effects.iOS;
using CoreGraphics;

[assembly: ExportEffect(typeof(AlterLineHeightPlatformEffect), nameof(AlterLineHeight))]
namespace AiForms.Effects.iOS
{
    public class AlterLineHeightPlatformEffect : PlatformEffect
    {

        private ILineHeightEffect _effect;

        protected override void OnAttached()
        {
            if (Element is Label) {
                _effect = new LineHeightForLabel(Container, Control, Element);
            }
            else if (Element is Editor) {
                _effect = new LineHeightForTextView(Container, Control, Element);
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
            else if (e.PropertyName == Label.TextProperty.PropertyName && _effect is LineHeightForLabel) {
                _effect?.Update();
            }

        }


    }
}

