using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class ToFlatButton
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool),
                    declaringType: typeof(ToFlatButton),
                    defaultValue: false,
                    propertyChanged: OnOffChanged
                );

        public static void SetOn(BindableObject view, bool value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool GetOn(BindableObject view)
        {
            return (bool)view.GetValue(OnProperty);
        }


        public static readonly BindableProperty RippleColorProperty =
            BindableProperty.CreateAttached(
                    "RippleColor",
                    typeof(Color),
                    typeof(ToFlatButton),
                    default(Color)
                );

        public static void SetRippleColor(BindableObject view, Color value)
        {
            view.SetValue(RippleColorProperty, value);
        }

        public static Color GetRippleColor(BindableObject view)
        {
            return (Color)view.GetValue(RippleColorProperty);
        }

        private static void OnOffChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
                return;
            if (!(view is Button))
                return;

            if ((bool)newValue) {
                view.Effects.Add(new ToFlatButtonRoutingEffect());
            }
            else {
                var toRemove = view.Effects.FirstOrDefault(e => e is ToFlatButtonRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        class ToFlatButtonRoutingEffect : RoutingEffect
        {
            public ToFlatButtonRoutingEffect() : base("AiForms." + nameof(ToFlatButton)) { }
        }
    }
}
