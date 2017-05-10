using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class AddText
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    "On",
                    typeof(bool),
                    typeof(AddText),
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

            if ((bool)newValue) {
                view.Effects.Add(new AddTextRoutingEffect());
            }
            else {
                var toRemove = view.Effects.FirstOrDefault(e => e is AddTextRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }


        class AddTextRoutingEffect : RoutingEffect
        {
            public AddTextRoutingEffect() : base("AiForms." + nameof(AddText)) { }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.CreateAttached(
                    "Text",
                    typeof(string),
                    typeof(AddText),
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

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.CreateAttached(
                    "FontSize",
                    typeof(double),
                    typeof(AddTextRoutingEffect),
                    8.0d
                );


        public static void SetFontSize(BindableObject view, double value)
        {
            view.SetValue(FontSizeProperty, value);
        }
       
        public static double GetFontSize(BindableObject view)
        {
            return (double)view.GetValue(FontSizeProperty);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.CreateAttached(
                    "TextColor",
                    typeof(Color),
                    typeof(AddText),
                    default(Color)
                );

        public static void SetTextColor(BindableObject view, Color value)
        {
            view.SetValue(TextColorProperty, value);
        }

        public static Color GetTextColor(BindableObject view)
        {
            return (Color)view.GetValue(TextColorProperty);
        }

        public static readonly BindableProperty MarginProperty =
            BindableProperty.CreateAttached(
                    "Margin",
                    typeof(int),
                    typeof(AddText),
                    0
                );

        public static void SetMargin(BindableObject view, int value)
        {
            view.SetValue(MarginProperty, value);
        }

        public static int GetMargin(BindableObject view)
        {
            return (int)view.GetValue(MarginProperty);
        }

        public static readonly BindableProperty HorizontalAlignProperty =
            BindableProperty.CreateAttached(
                    "HorizontalAlign",
                    typeof(TextAlignment),
                    typeof(AddText),
                    default(TextAlignment)
                );

        public static void SetHorizontalAlign(BindableObject view, TextAlignment value)
        {
            view.SetValue(HorizontalAlignProperty, value);
        }

        public static TextAlignment GetHorizontalAlign(BindableObject view)
        {
            return (TextAlignment)view.GetValue(HorizontalAlignProperty);
        }

        public static readonly BindableProperty VerticalAlignProperty =
            BindableProperty.CreateAttached(
                    "VerticalAlign",
                    typeof(TextAlignment),
                    typeof(AddText),
                    default(TextAlignment)
                );

        public static void SetVerticalAlign(BindableObject view, TextAlignment value)
        {
            view.SetValue(VerticalAlignProperty, value);
        }

        public static TextAlignment GetVerticalAlign(BindableObject view)
        {
            return (TextAlignment)view.GetValue(VerticalAlignProperty);
        }
    }
}
