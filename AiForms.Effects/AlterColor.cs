using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class AlterColor
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool),
                declaringType: typeof(AlterColor),
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

        private static void OnOffChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
                return;

            if (!(view is Slider || view is Switch || view is Entry || view is Editor || view is Page)) {
                return;
            }

            if ((bool)newValue) {
                view.Effects.Add(new AlterColorRoutingEffect());
            }
            else {
                var toRemove = view.Effects.FirstOrDefault(e => e is AlterColorRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        public static readonly BindableProperty AccentProperty =
            BindableProperty.CreateAttached(
                "Accent",
                typeof(Color),
                typeof(AlterColor),
                default(Color)
            );

        public static void SetAccent(BindableObject view, Color value)
        {
            view.SetValue(AccentProperty, value);
        }

        public static Color GetAccent(BindableObject view)
        {
            return (Color)view.GetValue(AccentProperty);
        }

        class AlterColorRoutingEffect : RoutingEffect
        {
            public AlterColorRoutingEffect() : base("AiForms." + nameof(AlterColor)) { }
        }
    }
}
