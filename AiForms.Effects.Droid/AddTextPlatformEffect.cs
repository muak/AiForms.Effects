using System;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AGraphics = Android.Graphics;
using ARelativeLayout = Android.Widget.RelativeLayout;

[assembly: ExportEffect(typeof(AddTextPlatformEffect), nameof(AddText))]
namespace AiForms.Effects.Droid
{
    public class AddTextPlatformEffect : PlatformEffect
    {
        private TextView textView;
        private ARelativeLayout relative;
        private ContainerOnLayoutChangeListener listener;

        protected override void OnAttached()
        {
            relative = new ARelativeLayout(Container.Context);
            relative.SetBackgroundColor(Android.Graphics.Color.Red);

            using (var lparam = new ARelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)) {
                lparam.AddRule(LayoutRules.AlignParentTop);

                textView = new TextView(Container.Context);
                textView.SetMaxLines(1);
                textView.SetMinLines(1);

                relative.AddView(textView, lparam);
            }

            using ( var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)) {
                Container.AddView(relative, param);
            }

            listener = new ContainerOnLayoutChangeListener(relative,textView);
            Container.AddOnLayoutChangeListener(listener);

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
            if (renderer?.Element != null) {    // Disposeされているかの判定
                Container.RemoveOnLayoutChangeListener(listener);
            }

            listener.Dispose();
            listener = null;

            textView.Dispose();
            textView = null;

            relative.Dispose();
            relative = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if(args.PropertyName == AddText.TextProperty.PropertyName){
                UpdateText();
            }
            else if(args.PropertyName == AddText.FontSizeProperty.PropertyName){
                UpdateFontSize();
            }
            else if(args.PropertyName == AddText.TextColorProperty.PropertyName){
                UpdateTextColor();
            }
            else if(args.PropertyName == AddText.MarginProperty.PropertyName){
                UpdateMargin();
            }
            else if(args.PropertyName == AddText.HorizontalAlignProperty.PropertyName){
                UpdateHorizontalAlign();
            }
            else if(args.PropertyName == AddText.VerticalAlignProperty.PropertyName){
                UpdateVerticalAlign();
            }
        }

        void UpdateText() {
            var text = AddText.GetText(Element);
            textView.Text = text;
            textView.Visibility = string.IsNullOrEmpty(text) ? ViewStates.Invisible : ViewStates.Visible;
        }

        void UpdateFontSize(){
            var size = (float)AddText.GetFontSize(Element);
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, size);
        }

        void UpdateTextColor(){
            textView.SetTextColor(AddText.GetTextColor(Element).ToAndroid());
        }

        void UpdateMargin(){
            var margin = (int)Container.Context.ToPixels(AddText.GetMargin(Element));
            textView.SetPadding(margin,margin,margin,margin);
        }

        void UpdateHorizontalAlign(){
            textView.Gravity = AddText.GetHorizontalAlign(Element).ToHorizontalGravityFlags();
        }

        void UpdateVerticalAlign(){
            var align = AddText.GetVerticalAlign(Element);
            var param = new ARelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            param.AddRule(align == Xamarin.Forms.TextAlignment.Start ? LayoutRules.AlignParentTop:LayoutRules.AlignParentBottom);
            relative.LayoutParameters = param;

            relative.ForceLayout();
            relative.Invalidate();
            textView.ForceLayout();
            textView.Invalidate();
            //textView.Gravity = align.ToVerticalGravityFlags();
        }

        internal class ContainerOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
        {
            private Android.Widget.RelativeLayout _layout;
            private TextView _textview;

            public ContainerOnLayoutChangeListener(Android.Widget.RelativeLayout layout,TextView textview) {
                _layout = layout;
                _textview = textview;
            }

            //ContainerにAddViewした子要素のサイズを確定する必要があるため
            //ControlのOnLayoutChangeのタイミングでセットする
            public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom) {
                _layout.Right = v.Right;
                _layout.Bottom = v.Bottom;
                _textview.Right = v.Right;
                _textview.Bottom = 30;
            }
        }


    }

    internal static class AlignmentExtensions
    {
        internal static GravityFlags ToHorizontalGravityFlags(this Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    return GravityFlags.CenterHorizontal;
                case Xamarin.Forms.TextAlignment.End:
                    return GravityFlags.Right;
                default:
                    return GravityFlags.Left;
            }
        }

        internal static GravityFlags ToVerticalGravityFlags(this Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment)
            {
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

