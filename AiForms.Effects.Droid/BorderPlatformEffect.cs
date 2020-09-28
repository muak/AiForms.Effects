using System;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(BorderPlatformEffect), nameof(Border))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class BorderPlatformEffect : AiEffectBase
    {
        Android.Views.View _view;
        GradientDrawable _border;
        Android.Graphics.Color _color;
        int _width;
        float _radius;
        Drawable _orgDrawable;
        Drawable _orgTextBackground;

        protected override void OnAttachedOverride()
        {
            _view = Container ?? Control;
            
            _border = new GradientDrawable();
            _orgDrawable = _view.Background;
            if(Control is FormsEditTextBase editText)
            {
                _orgTextBackground = editText.Background;
                // hide underline.
                editText.Background = null;
            }

            UpdateRadius();
            UpdateWidth();
            UpdateColor();
            UpdateBorder();
        }

        protected override void OnDetachedOverride()
        {
            if (!IsDisposed) {    // Check disposed
                if(Control is FormsEditTextBase editText)
                {
                    editText.Background = _orgTextBackground;                    
                }

                if(Element is Label label)
                {
                    (_view as FormsTextView).SetBackgroundColor(label.BackgroundColor.ToAndroid());
                }
                else if(Element is Image image)
                {
                    (_view as ImageView).SetBackgroundColor(image.BackgroundColor.ToAndroid());
                }
                else
                {
                    _view.Background = _orgDrawable;
                }                

                _view.SetPadding(0, 0, 0, 0);
                _view.ClipToOutline = false;

                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }
            _border?.Dispose();
            _border = null;
            _orgDrawable = null;
            _orgTextBackground = null;
            _view = null;
            System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached completely");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (!IsSupportedByApi)
                return;

            if (IsDisposed) {
                return;
            }

            if (args.PropertyName == Border.RadiusProperty.PropertyName) {
                UpdateRadius();
                UpdateBorder();
            }
            else if (args.PropertyName == Border.WidthProperty.PropertyName) {
                UpdateWidth();
                UpdateBorder();
            }
            else if (args.PropertyName == Border.ColorProperty.PropertyName) {
                UpdateColor();
                UpdateBorder();
            }
            else if (args.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
        }

        void UpdateRadius()
        {
            _radius = _view.Context.ToPixels(Border.GetRadius(Element));
        }

        void UpdateWidth()
        {
            _width = (int)_view.Context.ToPixels(Border.GetWidth(Element) ?? 0);
        }

        void UpdateColor()
        {
            _color = Border.GetColor(Element).ToAndroid();
        }

        void UpdateBorder()
        {
            _border.SetStroke(_width, _color);
            _border.SetCornerRadius(_radius);

            var formsBack = (Element as Xamarin.Forms.View).BackgroundColor;
            if (formsBack != Xamarin.Forms.Color.Default) {
                _border.SetColor(formsBack.ToAndroid());
            }

            if (Element is BoxView) {
                var foreColor = ((BoxView)Element).Color;
                if (foreColor != Xamarin.Forms.Color.Default) {
                    _border.SetColor(foreColor.ToAndroid());
                }
            }

            _view.SetPadding(_width, _width, _width, _width);
            _view.ClipToOutline = true; //not to overflow children

            _view.SetBackground(_border);            
        }

        void UpdateBackgroundColor()
        {
            _orgDrawable = _view.Background;
            UpdateBorder();
        }
    }
}
