using System;
using Xamarin.Forms;
using AiForms.Effects;
using AiForms.Effects.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Diagnostics;
using CoreGraphics;

[assembly: ExportEffect(typeof(FloatingPlatformEffect), nameof(Floating))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers =true)]
    public class FloatingPlatformEffect:PlatformEffect
    {
        Page _page;
        UIView _nativePage;
        FloatingLayout _formsLayout;
        Action OnceInitializeAction;
        UIDeviceOrientation _previousOrientation;

        protected override void OnAttached()
        {
            if (!(Element is Page)) return;

            OnceInitializeAction = Initialize;

            _formsLayout = Floating.GetContent(Element);
            _formsLayout.Parent = Element;

            _page = Element as Page;
            _page.SizeChanged += PageSizeChanged;
            _page.LayoutChanged += PageLayoutChanged;

            void BindingContextChanged(object sender,EventArgs e)
            {
                // When the target is a Page, since OnDetached method isn't automatically called,
                // manually make it call at this timing.
                if (_page.BindingContext != null && !IsAttached) return;

                _page.BindingContextChanged -= BindingContextChanged;

                var toRemove = Element.Effects.OfType<AiRoutingEffectBase>().FirstOrDefault(x => x.EffectId == ResolveId);
                Element.Effects.Remove(toRemove);
            }

            _page.BindingContextChanged += BindingContextChanged;
        }

        protected override void OnDetached()
        {
            _page.SizeChanged -= PageSizeChanged;
            _page.LayoutChanged -= PageLayoutChanged;
            _formsLayout.Parent = null;

            foreach(var child in _formsLayout)
            {
                PlatformUtility.DisposeModelAndChildrenRenderers(child);
            }

            _formsLayout = null;
            _nativePage = null;
            _page = null;

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }


        void PageLayoutChanged(object sender, EventArgs e)
        {
            if (OnceInitializeAction == null && _previousOrientation != UIDevice.CurrentDevice.Orientation)
            {
                _formsLayout.Layout(_nativePage.Bounds.ToRectangle());
                _formsLayout.LayoutChildren();
            }
            _previousOrientation = UIDevice.CurrentDevice.Orientation;
        }


        void PageSizeChanged(object sender, EventArgs e)
        {
            OnceInitializeAction?.Invoke();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
        }

        void Initialize()
        {
            OnceInitializeAction = null;

            _nativePage = Container;

            _formsLayout.Layout(_nativePage.Bounds.ToRectangle());

            foreach (var child in _formsLayout)
            {
                var renderer = PlatformUtility.GetOrCreateNativeView(child);
                _formsLayout.LayoutChild(child);
                SetLayoutAlignment(renderer.NativeView, _nativePage, child);
            }
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
