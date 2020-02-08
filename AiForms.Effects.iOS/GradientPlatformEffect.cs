using System;
using System.ComponentModel;
using System.Linq;
using AiForms.Effects;
using AiForms.Effects.iOS;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(GradientPlatformEffect), nameof(Gradient))]
namespace AiForms.Effects.iOS
{
    public class GradientPlatformEffect:PlatformEffect
    {
        UIView _view;
        VisualElement _visualElement;
        CAGradientLayer _layer;
        bool _clipsToBounds;

        protected override void OnAttached()
        {
            _view = Control ?? Container;
            _visualElement = Element as VisualElement;
            if(_visualElement == null)
            {
                Device.BeginInvokeOnMainThread(() => Gradient.SetOn(Element, false));
                return;
            }

            if (Element is Label)
            {
                _view = Container;
            }

            _clipsToBounds = _view.ClipsToBounds;
            _visualElement.SizeChanged += OnSizeChanged;
            UpdateGradient();
        }

        protected override void OnDetached()
        {
            if(_visualElement != null)
            {
                _visualElement.SizeChanged -= OnSizeChanged;
            }

            if(_view != null)
            {
                _view.ClipsToBounds = _clipsToBounds;
            }

            _layer?.RemoveFromSuperLayer();
            _layer?.Dispose();
            _layer = null;

            _view = null;
            _visualElement = null;

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if(args.PropertyName == Gradient.ColorsProperty.PropertyName ||
                args.PropertyName == Gradient.OrientationProperty.PropertyName)
            {
                UpdateGradient();
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            UpdateGradient();
        }

        void UpdateGradient()
        {
            var colors = Gradient.GetColors(Element);
            if(colors == null)
            {
                return;
            }

            _layer?.RemoveFromSuperLayer();
            _layer?.Dispose();

            _layer = new CAGradientLayer();
            _layer.Frame = new CGRect(0, 0, _visualElement.Width, _visualElement.Height);      
            _layer.Colors = colors.Select(x => x.ToCGColor()).ToArray();
            (_layer.StartPoint,_layer.EndPoint) = ConvertPoint();            
            _view.Layer.InsertSublayer(_layer, 0);
            _view.ClipsToBounds = true;
        }

        (CGPoint start,CGPoint end) ConvertPoint()
        {
            var orientation = Gradient.GetOrientation(Element);

            switch(orientation)
            {
                case GradientOrientation.LeftRight:
                    return (new CGPoint(0, 0.5), new CGPoint(1, 0.5));
                case GradientOrientation.BlTr:
                    return (new CGPoint(0, 1.0), new CGPoint(1, 0));
                case GradientOrientation.BottomTop:
                    return (new CGPoint(0.5, 1), new CGPoint(0.5, 0));
                case GradientOrientation.BrTl:
                    return (new CGPoint(1, 1), new CGPoint(0, 0));
                case GradientOrientation.RightLeft:
                    return (new CGPoint(1, 0.5), new CGPoint(0, 0.5));
                case GradientOrientation.TrBl:
                    return (new CGPoint(1, 0), new CGPoint(0, 1));
                case GradientOrientation.TopBottom:
                    return (new CGPoint(0.5, 0), new CGPoint(0.5, 1));
                case GradientOrientation.TlBr:
                    return (new CGPoint(0, 0), new CGPoint(1, 1));
                default:
                    return (new CGPoint(0, 0.5), new CGPoint(1, 0.5));
            }
        }
    }
}
