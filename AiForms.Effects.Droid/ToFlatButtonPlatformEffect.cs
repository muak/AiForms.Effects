using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Animation;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(ToFlatButtonPlatformEffect), nameof(ToFlatButton))]
namespace AiForms.Effects.Droid
{
    public class ToFlatButtonPlatformEffect : PlatformEffect
    {
        private ColorStateList Colors;
        private GradientDrawable Shape;
        private InsetDrawable Inset;
        private RippleDrawable Ripple;

        private Button FormsButton;
        private AppCompatButton NativeButton;

        private Drawable OrgBackground;
        private StateListAnimator OrgStateListAnimator;
        private ColorStateList OrgBackgroundTint;


        protected override void OnAttached()
        {
            NativeButton = Control as AppCompatButton;
            if (NativeButton == null)
                return;

            FormsButton = Element as Button;

            OrgBackground = NativeButton.Background;
            OrgStateListAnimator = NativeButton.StateListAnimator;
            OrgBackgroundTint = NativeButton.SupportBackgroundTintList;

            //shadow off
            NativeButton.StateListAnimator = null;
            //disabled default background
            NativeButton.SupportBackgroundTintList = null;

            Shape = new GradientDrawable();
            Shape.SetShape(ShapeType.Rectangle);

            UpdateBackgroundColor();
            UpdateBorderRadius();
            UpdateBorder();
            UpdateRippleColor();

            NativeButton.Background = Ripple;
        }

        protected override void OnDetached()
        {
            var renderer = Container as IVisualElementRenderer;
            if (renderer?.Element != null) {
                NativeButton.Background = OrgBackground;
                NativeButton.StateListAnimator = OrgStateListAnimator;
                NativeButton.SupportBackgroundTintList = OrgBackgroundTint;
            }
            Colors.Dispose();
            Shape.Dispose();
            Inset.Dispose();
            Ripple.Dispose();

            FormsButton = null;
            NativeButton = null;
            OrgBackground = null;
            OrgStateListAnimator = null;
            OrgBackgroundTint = null;

            Colors = null;
            Shape = null;
            Ripple = null;
            Inset = null;

        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);
            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == Button.BorderRadiusProperty.PropertyName) {
                UpdateBorderRadius();
            }
            else if (e.PropertyName == Button.BorderWidthProperty.PropertyName) {
                UpdateBorder();
            }
            else if (e.PropertyName == Button.BorderColorProperty.PropertyName) {
                UpdateBorder();
            }
            else if (e.PropertyName == ToFlatButton.RippleColorProperty.PropertyName) {
                UpdateRippleColor();
            }

        }

        void UpdateBackgroundColor()
        {
            if (Colors != null) {
                Colors.Dispose();
            }

            var color = Android.Graphics.Color.White;
            var disabledColor = Android.Graphics.Color.White;

            if (FormsButton.BackgroundColor != Xamarin.Forms.Color.Default) {
                color = FormsButton.BackgroundColor.ToAndroid();
                disabledColor = FormsButton.BackgroundColor.MultiplyAlpha(0.5).ToAndroid();
            }

            Colors = new ColorStateList(new int[][]
                            {
                                new int[]{global::Android.Resource.Attribute.StateEnabled},
                                new int[]{-global::Android.Resource.Attribute.StateEnabled},//disabled
                            },
                           new int[] {
                                color,
                                disabledColor
                            });
            Shape.SetColor(Colors);
        }
        void UpdateBorderRadius()
        {
            var size = (float)Container.Context.ToPixels(FormsButton.BorderRadius);
            Shape.SetCornerRadius(size);
        }
        void UpdateBorder()
        {
            var borderColor = FormsButton.BorderColor == Xamarin.Forms.Color.Default ? 
                                         Xamarin.Forms.Color.Transparent : FormsButton.BorderColor;
            var size = (int)Container.Context.ToPixels(FormsButton.BorderWidth);
            Shape.SetStroke(size,borderColor.ToAndroid());
        }

        void UpdateRippleColor()
        {
            var rippleColor = ToFlatButton.GetRippleColor(Element).ToAndroid();
            if (Ripple == null) {
                Inset = new InsetDrawable(Shape,0,1,0,1);
                Ripple = new RippleDrawable(getPressedColorSelector(rippleColor.ToArgb()), Inset, null);
            }
            else {
                Ripple.SetColor(getPressedColorSelector(rippleColor.ToArgb()));
            }

        }

        ColorStateList getPressedColorSelector(int pressedColor)
        {
            return new ColorStateList(
                new int[][]
                {
                    new int[]{}
                },
                new int[]
                {
                    pressedColor,
                });
        }

    }
}
