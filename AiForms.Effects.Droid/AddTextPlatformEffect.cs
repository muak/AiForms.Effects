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
        private LinearLayout _layout;
        private ContainerOnLayoutChangeListener _listener;

        protected override void OnAttached()
        {
            _layout = new LinearLayout(Container.Context);
            _layout.Orientation = Orientation.Vertical;

            using (var lparam = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)) {

                _textView = new TextView(Container.Context);
                _textView.SetMaxLines(1);
                _textView.SetMinLines(1);
                _textView.Ellipsize = Android.Text.TextUtils.TruncateAt.End;

                _layout.AddView(_textView, lparam);
            }

            using (var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)) {
                Container.AddView(_layout, param);
            }

            _listener = new ContainerOnLayoutChangeListener(_layout, _textView, Element);
            Container.AddOnLayoutChangeListener(_listener);

            UpdateText();
            UpdateFontSize();
            UpdateTextColor();
            UpdateMargin();
            UpdateHorizontalAlign();
            UpdateVerticalAlign();
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

            _layout.Dispose();
            _layout = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == AddText.TextProperty.PropertyName) {
                UpdateText();
            }
            else if (args.PropertyName == AddText.FontSizeProperty.PropertyName) {
                UpdateFontSize();
                Container.RequestFocus();
            }
            else if (args.PropertyName == AddText.TextColorProperty.PropertyName) {
                UpdateTextColor();
            }
            else if (args.PropertyName == AddText.MarginProperty.PropertyName) {
                UpdateMargin();
                Container.RequestFocus();
            }
            else if (args.PropertyName == AddText.HorizontalAlignProperty.PropertyName) {
                UpdateHorizontalAlign();
            }
            else if (args.PropertyName == AddText.VerticalAlignProperty.PropertyName) {
                UpdateVerticalAlign();
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

        void UpdateMargin()
        {
            var margin = AddText.GetMargin(Element);
            _textView.SetPadding(
                (int)Container.Context.ToPixels(margin.Left),
                (int)Container.Context.ToPixels(margin.Top),
                (int)Container.Context.ToPixels(margin.Right),
                (int)Container.Context.ToPixels(margin.Bottom)
            );
        }

        void UpdateHorizontalAlign()
        {
            _textView.Gravity = AddText.GetHorizontalAlign(Element).ToHorizontalGravityFlags();
        }

        void UpdateVerticalAlign()
        {
            Container.RequestLayout();
        }

        internal class ContainerOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
        {
            private ViewGroup _layout;
            private TextView _textview;
            private Element _element;

            public ContainerOnLayoutChangeListener(ViewGroup layout, TextView textview, Element element)
            {
                _layout = layout;
                _textview = textview;
                _element = element;
            }

            // In OnLayoutChange, decide size and position of child element.
            // For some reason, in layout that was added to container, it does not work all gravity options and all layout options.
            public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
            {
                _layout.Right = v.Width;
                _layout.Bottom = v.Height;

                _textview.Right = v.Width;

                var margin = AddText.GetMargin(_element);
                var height = (int)Forms.Context.ToPixels(margin.Top) + (int)Forms.Context.ToPixels(margin.Bottom) + _textview.LineHeight;
                var yPos = AddText.GetVerticalAlign(_element) == Xamarin.Forms.TextAlignment.Start ? 0 : v.Height - height;

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

