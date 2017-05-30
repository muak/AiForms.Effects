using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;

[assembly: ExportEffect(typeof(AddTextPlatformEffect), nameof(AddText))]
namespace AiForms.Effects.Droid
{
    public class AddTextPlatformEffect : PlatformEffect
    {
        private TextView _textView;
        private ContainerOnLayoutChangeListener _listener;

        protected override void OnAttached()
        {
            _textView = new TextView(Container.Context);
            _textView.SetMaxLines(1);
            _textView.SetMinLines(1);
            _textView.Ellipsize = Android.Text.TextUtils.TruncateAt.End;

            Container.AddView(_textView);

            _listener = new ContainerOnLayoutChangeListener(_textView, Element);
            Container.AddOnLayoutChangeListener(_listener);

            UpdateText();
            UpdateFontSize();
            UpdateTextColor();
            UpdateBackgroundColor();
            UpdatePadding();
            Container.RequestLayout();
        }

        protected override void OnDetached()
        {
            var renderer = Container as IVisualElementRenderer;
            if (renderer?.Element != null) {    // check is disposed
                Container.RemoveOnLayoutChangeListener(_listener);
            }

            _listener.Dispose();
            _listener = null;

            _textView.Dispose();
            _textView = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == AddText.TextProperty.PropertyName) {
                UpdateText();
                Container.RequestLayout();
            }
            else if (args.PropertyName == AddText.FontSizeProperty.PropertyName) {
                UpdateFontSize();
                Container.RequestLayout();
            }
            else if (args.PropertyName == AddText.TextColorProperty.PropertyName) {
                UpdateTextColor();
            }
            else if (args.PropertyName == AddText.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (args.PropertyName == AddText.PaddingProperty.PropertyName) {
                UpdatePadding();
                Container.RequestLayout();
            }
            else if (args.PropertyName == AddText.MarginProperty.PropertyName) {
                Container.RequestLayout();
            }
            else if (args.PropertyName == AddText.HorizontalAlignProperty.PropertyName) {
                Container.RequestLayout();
            }
            else if (args.PropertyName == AddText.VerticalAlignProperty.PropertyName) {
                Container.RequestLayout();
            }
        }

        void UpdateText()
        {
            var text = AddText.GetText(Element);
            _textView.Text = text;
            _textView.Visibility = string.IsNullOrEmpty(text) ? ViewStates.Invisible : ViewStates.Visible;
        }

        void UpdateFontSize()
        {
            var size = (float)AddText.GetFontSize(Element);
            _textView.SetTextSize(Android.Util.ComplexUnitType.Sp, size);
        }

        void UpdateTextColor()
        {
            _textView.SetTextColor(AddText.GetTextColor(Element).ToAndroid());
        }

        void UpdateBackgroundColor()
        {
            _textView.SetBackgroundColor(AddText.GetBackgroundColor(Element).ToAndroid());
        }

        void UpdatePadding()
        {
            var padding = AddText.GetPadding(Element);
            _textView.SetPadding(
                (int)Container.Context.ToPixels(padding.Left),
                (int)Container.Context.ToPixels(padding.Top),
                (int)Container.Context.ToPixels(padding.Right),
                (int)Container.Context.ToPixels(padding.Bottom)
            );
        }


        internal class ContainerOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
        {
            private TextView _textview;
            private Element _element;

            public ContainerOnLayoutChangeListener(TextView textview, Element element)
            {
                _textview = textview;
                _element = element;
            }

            // In OnLayoutChange, decide size and position of child element.
            // For some reason, in layout that was added to container, it does not work all gravity options and all layout options.
            public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
            {
                if (string.IsNullOrEmpty(_textview.Text)) {
                    return;
                }

                var margin = AddText.GetMargin(_element);
                margin.Left = (int)Forms.Context.ToPixels(margin.Left);
                margin.Top = (int)Forms.Context.ToPixels(margin.Top);
                margin.Right = (int)Forms.Context.ToPixels(margin.Right);
                margin.Bottom = (int)Forms.Context.ToPixels(margin.Bottom);

                var textpaint = _textview.Paint;
                var rect = new Android.Graphics.Rect();
                textpaint.GetTextBounds(_textview.Text, 0, _textview.Text.Length, rect);

                var xPos = 0;
                if (AddText.GetHorizontalAlign(_element) == Xamarin.Forms.TextAlignment.End) {
                    xPos = v.Width - rect.Width() - _textview.PaddingLeft - _textview.PaddingRight - (int)margin.Right - 4;
                    if (xPos < (int)margin.Left) {
                        xPos = (int)margin.Left;
                    }
                    _textview.Right = v.Width - (int)margin.Right;
                }
                else {
                    xPos = (int)margin.Left;
                    _textview.Right = (int)margin.Left + rect.Width() + _textview.PaddingLeft + _textview.PaddingRight + 4;
                    if (_textview.Right >= v.Width) {
                        _textview.Right = v.Width - (int)margin.Right;
                    }
                }

                _textview.Left = xPos;


                var fm = textpaint.GetFontMetrics();
                var height = (int)(Math.Abs(fm.Top) + fm.Bottom + _textview.PaddingTop + _textview.PaddingEnd);
                var yPos = AddText.GetVerticalAlign(_element) == Xamarin.Forms.TextAlignment.Start ? 0 + (int)margin.Top : v.Height - height - (int)margin.Bottom;

                _textview.Top = yPos;
                _textview.Bottom = yPos + height;
            }
        }


    }

    internal static class AlignmentExtensions
    {
        internal static GravityFlags ToHorizontalGravityFlags(this Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment) {
                case Xamarin.Forms.TextAlignment.End:
                    return GravityFlags.Right;
                case Xamarin.Forms.TextAlignment.Center:
                    return GravityFlags.Center;
                default:
                    return GravityFlags.Left;
            }
        }

        internal static GravityFlags ToVerticalGravityFlags(this Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment) {
                case Xamarin.Forms.TextAlignment.Start:
                    return GravityFlags.Top;
                case Xamarin.Forms.TextAlignment.End:
                    return GravityFlags.Bottom;
                default:
                    return GravityFlags.CenterVertical;
            }
        }
    }
}

