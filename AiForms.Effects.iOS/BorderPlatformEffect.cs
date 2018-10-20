using System;
using AiForms.Effects;
using AiForms.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Linq;

[assembly: ExportEffect(typeof(BorderPlatformEffect), nameof(Border))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class BorderPlatformEffect : PlatformEffect
    {
        UIView _view;
        Type[] hasBorderTypes = new Type[]{
            typeof(Entry),
            typeof(DatePicker),
            typeof(TimePicker),
            typeof(Picker),
        };
        bool _clipsToBounds;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            _clipsToBounds = _view.ClipsToBounds;
            if (hasBorderTypes.Any(x => x == Element.GetType())) {
                var textfield = _view as UITextField;
                textfield.BorderStyle = UITextBorderStyle.None;
            }

            UpdateRadius();
            UpdateWidth();
            UpdateColor();
        }

        protected override void OnDetached()
        {
            if (hasBorderTypes.Any(x => x == Element.GetType())) {
                var textfield = _view as UITextField;
                textfield.BorderStyle = UITextBorderStyle.RoundedRect;
            }
            _view.ClipsToBounds = _clipsToBounds;
            _view.Layer.CornerRadius = 0f;
            _view.Layer.BorderWidth = 0;
            _view = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == Border.RadiusProperty.PropertyName) {
                UpdateRadius();
            }
            else if (args.PropertyName == Border.WidthProperty.PropertyName) {
                UpdateWidth();
            }
            else if (args.PropertyName == Border.ColorProperty.PropertyName) {
                UpdateColor();
            }
        }

        void UpdateRadius()
        {
            var r = Border.GetRadius(Element);
            _view.Layer.CornerRadius = (nfloat)r;
            _view.ClipsToBounds = true;
        }

        void UpdateWidth()
        {
            _view.Layer.BorderWidth = (float)Border.GetWidth(Element);
        }

        void UpdateColor()
        {
            _view.Layer.BorderColor = Border.GetColor(Element).ToCGColor();
        }
    }
}
