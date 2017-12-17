using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class SizeToFit
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                "On",
                typeof(bool),
                typeof(SizeToFit),
                default(bool),
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
            if (!(view is Label))
                return;

            if ((bool)newValue)
            {
                view.Effects.Add(new SizeToFitRoutingEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is SizeToFitRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        public static readonly BindableProperty CanExpandProperty =
            BindableProperty.CreateAttached(
                    "CanExpand",
                    typeof(bool),
                    typeof(SizeToFit),
                    true
                );

        public static void SetCanExpand(BindableObject view, bool value)
        {
            view.SetValue(CanExpandProperty, value);
        }

        public static bool GetCanExpand(BindableObject view)
        {
            return (bool)view.GetValue(CanExpandProperty);
        }

        class SizeToFitRoutingEffect : RoutingEffect
        {
            public SizeToFitRoutingEffect() : base("AiForms." + nameof(SizeToFit)) { }
        }
    }
}
