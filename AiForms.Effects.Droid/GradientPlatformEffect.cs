using System;
using System.ComponentModel;
using System.Linq;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(GradientPlatformEffect), nameof(Gradient))]
namespace AiForms.Effects.Droid
{
    public class GradientPlatformEffect:AiEffectBase
    {
        Android.Views.View _view;
        GradientDrawable _gradient;
        Drawable _orgDrawable;

        protected override void OnAttachedOverride()
        {
            _view = Container ?? Control;

            _gradient = new GradientDrawable();
            _orgDrawable = _view.Background;

            UpdateGradient();
        }

        protected override void OnDetachedOverride()
        {
            if(!IsDisposed)
            {
                _view.Background = _orgDrawable;
                _view.ClipToOutline = false;
                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }

            _gradient?.Dispose();
            _gradient = null;
            _view = null;
            System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached completely");
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (!IsSupportedByApi)
                return;

            if (IsDisposed)
            {
                return;
            }

            if (args.PropertyName == Gradient.ColorsProperty.PropertyName ||
                args.PropertyName == Gradient.OrientationProperty.PropertyName)
            {
                UpdateGradient();
            }
        }

        void UpdateGradient()
        {
            var colors = Gradient.GetColors(Element);
            if(colors == null)
            {
                return;
            }

            _gradient.SetColors(colors.Select(x => (int)x.ToAndroid()).ToArray());
            _gradient.SetOrientation(ConvertOrientation());

            _view.ClipToOutline = true; //not to overflow children
            _view.SetBackground(_gradient);
        }

        GradientDrawable.Orientation ConvertOrientation()
        {
            var orientation = Gradient.GetOrientation(Element);

            switch (orientation)
            {
                case GradientOrientation.LeftRight:
                    return GradientDrawable.Orientation.LeftRight;
                case GradientOrientation.BlTr:
                    return GradientDrawable.Orientation.BlTr;
                case GradientOrientation.BottomTop:
                    return GradientDrawable.Orientation.BottomTop;
                case GradientOrientation.BrTl:
                    return GradientDrawable.Orientation.BrTl;
                case GradientOrientation.RightLeft:
                    return GradientDrawable.Orientation.RightLeft;
                case GradientOrientation.TrBl:
                    return GradientDrawable.Orientation.TrBl;
                case GradientOrientation.TopBottom:
                    return GradientDrawable.Orientation.TopBottom;
                case GradientOrientation.TlBr:
                    return GradientDrawable.Orientation.TlBr;
                default:
                    return GradientDrawable.Orientation.LeftRight;
            }
        }
    }
}
