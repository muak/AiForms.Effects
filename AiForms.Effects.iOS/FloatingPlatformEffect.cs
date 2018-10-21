using System;
using Xamarin.Forms;
using AiForms.Effects;
using AiForms.Effects.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.ComponentModel;
using System.Collections.Generic;

[assembly: ExportEffect(typeof(FloatingPlatformEffect), nameof(Floating))]
namespace AiForms.Effects.iOS
{
    public class FloatingPlatformEffect:PlatformEffect
    {
        Page _page;
        UIView _nativePage;
        UIView _layoutView;
        FloatingLayout _formsLayout;
        List<IVisualElementRenderer> _renderers = new List<IVisualElementRenderer>();
        Action OnceInitializeAction;
        List<UIView> _children;

        protected override void OnAttached()
        {
            if (!(Element is Page)) return;

            OnceInitializeAction = Initialize;

            _formsLayout = Floating.GetContent(Element);
            _formsLayout.Parent = Element;

            _page = Element as Page;
            _page.SizeChanged += _page_SizeChanged;

        }

        protected override void OnDetached()
        {

        }

        void _page_SizeChanged(object sender, EventArgs e)
        {
            if(OnceInitializeAction == null)
            {
                _formsLayout.Layout(_nativePage.Bounds.ToRectangle());
                _formsLayout.LayoutChildren();
            }
            else
            {
                OnceInitializeAction?.Invoke();
            }
        }


        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
        }

        void Initialize()
        {
            OnceInitializeAction = null;

            _nativePage = Container.Subviews[0];

            _formsLayout.Layout(_nativePage.Bounds.ToRectangle());

            foreach (var child in _formsLayout)
            {
                var renderer = GetOrCreateNativeView(child);
                _formsLayout.LayoutChild(child);
                SetLayoutAlignment(renderer.NativeView, _nativePage, child);
                _renderers.Add(renderer);
            }
        }

        internal static IVisualElementRenderer GetOrCreateNativeView(VisualElement view)
        {
            IVisualElementRenderer renderer = Platform.GetRenderer(view);
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(view);
                Platform.SetRenderer(view, renderer);
            }

            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

            return renderer;
        }

        internal static void SetLayoutAlignment(UIView targetView, UIView parentView, FloatingView floating)
        {
            targetView.TranslatesAutoresizingMaskIntoConstraints = false;
            parentView.AddSubview(targetView);

            if(floating.HorizontalLayoutAlignment != LayoutAlignment.Fill)
            {
                targetView.WidthAnchor.ConstraintEqualTo((System.nfloat)floating.Bounds.Width).Active = true;
            }
            if(floating.VerticalLayoutAlignment != LayoutAlignment.Fill)
            {
                targetView.HeightAnchor.ConstraintEqualTo((System.nfloat)floating.Bounds.Height).Active = true;
            }

            switch (floating.VerticalLayoutAlignment)
            {
                case Xamarin.Forms.LayoutAlignment.Start:
                    targetView.TopAnchor.ConstraintEqualTo(parentView.TopAnchor, floating.OffsetY).Active = true;
                    break;
                case Xamarin.Forms.LayoutAlignment.End:
                    targetView.BottomAnchor.ConstraintEqualTo(parentView.BottomAnchor, floating.OffsetY).Active = true;
                    break;
                case Xamarin.Forms.LayoutAlignment.Center:
                    targetView.CenterYAnchor.ConstraintEqualTo(parentView.CenterYAnchor, floating.OffsetY).Active = true;
                    break;
                default:
                    targetView.HeightAnchor.ConstraintEqualTo(parentView.HeightAnchor).Active = true;
                    break;
            }

            switch (floating.HorizontalLayoutAlignment)
            {
                case Xamarin.Forms.LayoutAlignment.Start:
                    targetView.LeftAnchor.ConstraintEqualTo(parentView.LeftAnchor, floating.OffsetX).Active = true;
                    break;
                case Xamarin.Forms.LayoutAlignment.End:
                    targetView.RightAnchor.ConstraintEqualTo(parentView.RightAnchor, floating.OffsetX).Active = true;
                    break;
                case Xamarin.Forms.LayoutAlignment.Center:
                    targetView.CenterXAnchor.ConstraintEqualTo(parentView.CenterXAnchor, floating.OffsetX).Active = true;
                    break;
                default:
                    targetView.WidthAnchor.ConstraintEqualTo(parentView.WidthAnchor).Active = true;
                    break;
            }
        }
    }
}
