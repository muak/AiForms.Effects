using System;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(SizeToFitPlatformEffect), nameof(SizeToFit))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class SizeToFitPlatformEffect : AiEffectBase
    {
        FormsTextView _view;
        float _orgFontSize;

        protected override void OnAttachedOverride()
        {
            _view = Control as FormsTextView;
            _orgFontSize = _view.TextSize;

            UpdateFitFont();
        }

        protected override void OnDetachedOverride()
        {
            if (!IsDisposed){
                _view.SetTextSize(ComplexUnitType.Px, _orgFontSize);
                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }
            _view = null;
            System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached completely");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (!IsSupportedByApi)
                return;

            if (args.PropertyName == VisualElement.HeightProperty.PropertyName ||
                args.PropertyName == VisualElement.WidthProperty.PropertyName ||
                args.PropertyName == Label.TextProperty.PropertyName ||
                args.PropertyName == SizeToFit.CanExpandProperty.PropertyName)
            {
                UpdateFitFont();
            }
            else if (args.PropertyName == Label.FontProperty.PropertyName)
            {
                _orgFontSize = _view.TextSize;
                UpdateFitFont();
            }
            //else if (args.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName){
            //    var label = Element as Label;
            //    _view.Gravity = label.HorizontalTextAlignment.ToHorizontalGravityFlags() | label.VerticalTextAlignment.ToVerticalGravityFlags();
            //}
        }

        void UpdateFitFont()
        {
            var formsView = Element as Xamarin.Forms.View;
            if (formsView.Width < 0 || formsView.Height < 0)
            {
                return;
            }

            var nativeHeight = _view.Context.ToPixels(formsView.Height);
            var nativeWidth = _view.Context.ToPixels(formsView.Width);

            var height = MeasureTextSize(_view.Text, nativeWidth, _orgFontSize, _view.Typeface);

            var fontSize = _orgFontSize;

            if (SizeToFit.GetCanExpand(Element) && height < nativeHeight)
            {
                while (height < nativeHeight)
                {
                    fontSize += 1f;
                    height = MeasureTextSize(_view.Text, nativeWidth, fontSize, _view.Typeface);
                }
            }

            while (height > nativeHeight && fontSize > 0)
            {
                fontSize -= 1f;
                height = MeasureTextSize(_view.Text, nativeWidth, fontSize, _view.Typeface);
            }

            _view.SetTextSize(ComplexUnitType.Px, fontSize);

            if (!IsFastRenderer)
            {
                _view.SetHeight(Container.Height);
            }
        }


        public double MeasureTextSize(string text, double width, double fontSize, Typeface typeface = null)
        {
            var textView = new TextView(_view.Context);
            textView.Typeface = typeface ?? Typeface.Default;
            textView.SetText(text, TextView.BufferType.Normal);
            textView.SetTextSize(ComplexUnitType.Px, (float)fontSize);

            int widthMeasureSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec(
                (int)width, MeasureSpecMode.AtMost);
            int heightMeasureSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec(
                0, MeasureSpecMode.Unspecified);

            textView.Measure(widthMeasureSpec, heightMeasureSpec);
            var height = (double)textView.MeasuredHeight;

            textView.Dispose();

            return height;
        }

    }
}