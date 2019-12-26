using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;
using Android.Content;
using System.Threading.Tasks;

[assembly: ExportEffect(typeof(AddTextPlatformEffect), nameof(AddText))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AddTextPlatformEffect : AiEffectBase
    {
        private TextView _textView;
        private ContainerOnLayoutChangeListener _listener;
        private Context _context => (Control ?? Container)?.Context;
        private ViewGroup _container;
        private FastRendererOnLayoutChangeListener _fastListener;

        protected override void OnAttachedOverride()
        {
            _container = Container;

            _textView = new TextView(_context);
            _textView.SetMaxLines(1);
            _textView.SetMinLines(1);
            _textView.Ellipsize = Android.Text.TextUtils.TruncateAt.End;

            if (IsFastRenderer) {
                _container = new FrameLayout(_context);

                _fastListener = new FastRendererOnLayoutChangeListener(Control, _container);
                Control.AddOnLayoutChangeListener(_fastListener);
            }

            _container.AddView(_textView);

            _listener = new ContainerOnLayoutChangeListener(_textView, Element);
            _container.AddOnLayoutChangeListener(_listener);

            UpdateText();
            UpdateFontSize();
            UpdateTextColor();
            UpdateBackgroundColor();
            UpdatePadding();
            UpdateLayout(_textView, Element, _container);
        }

        protected override void OnDetachedOverride()
        {
            System.Diagnostics.Debug.WriteLine(Element.GetType().FullName);

            if (!IsDisposed) {
                _container.RemoveView(_textView);
                _container.RemoveOnLayoutChangeListener(_listener);

                if (IsFastRenderer) {
                    Control.RemoveOnLayoutChangeListener(_fastListener);
                    _fastListener.CleanUp();
                }
                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }

            _listener?.Dispose();
            _listener = null;

            _textView.Dispose();
            _textView = null;

            _fastListener?.Dispose();
            _fastListener = null;
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

            if (args.PropertyName == AddText.TextProperty.PropertyName) {
                UpdateText();
                UpdateLayout(_textView, Element, _container);
            }
            else if (args.PropertyName == AddText.FontSizeProperty.PropertyName) {
                UpdateFontSize();
                UpdateLayout(_textView, Element, _container);
            }
            else if (args.PropertyName == AddText.TextColorProperty.PropertyName) {
                UpdateTextColor();
            }
            else if (args.PropertyName == AddText.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (args.PropertyName == AddText.PaddingProperty.PropertyName) {
                UpdatePadding();
                UpdateLayout(_textView, Element, _container);
            }
            else if (args.PropertyName == AddText.MarginProperty.PropertyName) {
                UpdateLayout(_textView, Element, _container);
            }
            else if (args.PropertyName == AddText.HorizontalAlignProperty.PropertyName) {
                UpdateLayout(_textView, Element, _container);
            }
            else if (args.PropertyName == AddText.VerticalAlignProperty.PropertyName) {
                UpdateLayout(_textView, Element, _container);
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
                (int)_context.ToPixels(padding.Left),
                (int)_context.ToPixels(padding.Top),
                (int)_context.ToPixels(padding.Right),
                (int)_context.ToPixels(padding.Bottom)
            );
        }

        static void UpdateLayout(TextView textview, Element element, Android.Views.View v)
        {
            if (string.IsNullOrEmpty(textview.Text)) {
                return;
            }

            var margin = AddText.GetMargin(element);
            margin.Left = (int)v.Context.ToPixels(margin.Left);
            margin.Top = (int)v.Context.ToPixels(margin.Top);
            margin.Right = (int)v.Context.ToPixels(margin.Right);
            margin.Bottom = (int)v.Context.ToPixels(margin.Bottom);

            var textpaint = textview.Paint;
            var rect = new Android.Graphics.Rect();
            //textpaint.GetTextBounds(textview.Text, 0, textview.Text.Length, rect); // GetTextBound.Width is sometimes a little small less than actual width.
            var textWidth = (int)textpaint.MeasureText(textview.Text);

            var xPos = 0;
            if (AddText.GetHorizontalAlign(element) == Xamarin.Forms.TextAlignment.End) {
                xPos = v.Width - textWidth - textview.PaddingLeft - textview.PaddingRight - (int)margin.Right - 4;
                if (xPos < (int)margin.Left) {
                    xPos = (int)margin.Left;
                }
                textview.Right = v.Width - (int)margin.Right;
            }
            else {
                xPos = (int)margin.Left;
                textview.Right = (int)margin.Left + textWidth + textview.PaddingLeft + textview.PaddingRight + 4;
                if (textview.Right >= v.Width) {
                    textview.Right = v.Width - (int)margin.Right;
                }
            }

            textview.Left = xPos;


            var fm = textpaint.GetFontMetrics();
            var height = (int)(Math.Abs(fm.Top) + fm.Bottom + textview.PaddingTop + textview.PaddingEnd);
            var yPos = AddText.GetVerticalAlign(element) == Xamarin.Forms.TextAlignment.Start ? 0 + (int)margin.Top : v.Height - height - (int)margin.Bottom;

            textview.Top = yPos;
            textview.Bottom = yPos + height;

            textview.Text = textview.Text; // HACK: For some reason, Invalidate is not work. Use reassign text instead of.
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
                UpdateLayout(_textview, _element, v);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) {
                    _textview = null;
                    _element = null;
                }
                base.Dispose(disposing);
            }
        }

        internal class FastRendererOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
        {
            bool _alreadyGotParent = false;
            Android.Views.ViewGroup _parent;
            Android.Views.View _view;
            ViewGroup _overlay;

            public FastRendererOnLayoutChangeListener(Android.Views.View view, ViewGroup container)
            {
                _view = view;
                _overlay = container;
            }

            // Because FastRenderer of Label or Image can't be set ClickListener, 
            // insert FrameLayout with same position and same size on the view.
            public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
            {
                _overlay.Layout(v.Left, v.Top, v.Right, v.Bottom);

                if (_alreadyGotParent) {
                    return;
                }

                _parent = _view.Parent as Android.Views.ViewGroup;
                _alreadyGotParent = true;

                _parent.AddView(_overlay);

                _overlay.BringToFront();
            }

            public void CleanUp()
            {
                _parent.RemoveView(_overlay);
                _overlay.Dispose();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) {
                    _parent = null;
                    _overlay = null;
                    _view = null;
                }
                base.Dispose(disposing);
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

