﻿using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class Border
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool),
                declaringType: typeof(Border),
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

            if ((bool)newValue) {
                view.Effects.Add(new BorderRoutingEffect());
            }
            else {
                var toRemove = view.Effects.FirstOrDefault(e => e is BorderRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        public static readonly BindableProperty RadiusProperty =
            BindableProperty.CreateAttached(
                "Radius",
                typeof(double),
                typeof(Border),
                default(double)
            );

        public static void SetRadius(BindableObject view, double value)
        {
            view.SetValue(RadiusProperty, value);
        }

        public static double GetRadius(BindableObject view)
        {
            return (double)view.GetValue(RadiusProperty);
        }

        public static readonly BindableProperty WidthProperty =
            BindableProperty.CreateAttached(
                "Width",
                typeof(double),
                typeof(Border),
                default(double)
            );

        public static void SetWidth(BindableObject view, double value)
        {
            view.SetValue(WidthProperty, value);
        }

        public static double GetWidth(BindableObject view)
        {
            return (double)view.GetValue(WidthProperty);
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.CreateAttached(
                "Color",
                typeof(Color),
                typeof(Border),
                Color.Transparent
            );

        public static void SetColor(BindableObject view, Color value)
        {
            view.SetValue(ColorProperty, value);
        }

        public static Color GetColor(BindableObject view)
        {
            return (Color)view.GetValue(ColorProperty);
        }

        class BorderRoutingEffect : RoutingEffect
        {
            public BorderRoutingEffect() : base("AiForms." + nameof(Border)) { }
        }


    }
}
