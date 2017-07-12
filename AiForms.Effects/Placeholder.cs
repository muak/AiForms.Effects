using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class Placeholder
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool),
                declaringType: typeof(Placeholder),
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
            var view = bindable as View;
            if (view == null)
                return;
            if(!(view is Editor))
                return;

            if ((bool)newValue) {
                view.Effects.Add(new PlaceholderRoutingEffect());
            }
            else {
                var toRemove = view.Effects.FirstOrDefault(e => e is PlaceholderRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.CreateAttached(
                    "Text",
                    typeof(string),
                    typeof(Placeholder),
                    default(string)
                );

        public static void SetText(BindableObject view, string value)
        {
            view.SetValue(TextProperty, value);
        }

        public static string GetText(BindableObject view)
        {
            return (string)view.GetValue(TextProperty);
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.CreateAttached(
                    "Color",
                    typeof(Color),
                    typeof(Placeholder),
                    default(Color)
                );

        public static void SetColor(BindableObject view, Color value)
        {
            view.SetValue(ColorProperty, value);
        }

        public static Color GetColor(BindableObject view)
        {
            return (Color)view.GetValue(ColorProperty);
        }

        class PlaceholderRoutingEffect : RoutingEffect
        {
            public PlaceholderRoutingEffect() : base("AiForms." + nameof(Placeholder)) { }
        }
    }
}
