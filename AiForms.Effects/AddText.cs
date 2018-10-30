using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Add text.
    /// </summary>
    public static class AddText
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    "On",
                    typeof(bool?),
                    typeof(AddText),
                    null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AddTextRoutingEffect>
                );

        /// <summary>
        /// Sets the on.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetOn(BindableObject view, bool? value)
        {
            view.SetValue(OnProperty, value);
        }

        /// <summary>
        /// Gets the on.
        /// </summary>
        /// <returns>The on.</returns>
        /// <param name="view">View.</param>
        public static bool? GetOn(BindableObject view)
        {
            return (bool?)view.GetValue(OnProperty);
        }

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.CreateAttached(
                "Text",
                typeof(string),
                typeof(AddText),
                default(string),
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<AddTextRoutingEffect>
            );

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetText(BindableObject view, string value)
        {
            view.SetValue(TextProperty, value);
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="view">View.</param>
        public static string GetText(BindableObject view)
        {
            return (string)view.GetValue(TextProperty);
        }

        /// <summary>
        /// The font size property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.CreateAttached(
                "FontSize",
                typeof(double),
                typeof(AddTextRoutingEffect),
                8.0d
            );

        /// <summary>
        /// Sets the size of the font.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetFontSize(BindableObject view, double value)
        {
            view.SetValue(FontSizeProperty, value);
        }

        /// <summary>
        /// Gets the size of the font.
        /// </summary>
        /// <returns>The font size.</returns>
        /// <param name="view">View.</param>
        public static double GetFontSize(BindableObject view)
        {
            return (double)view.GetValue(FontSizeProperty);
        }

        /// <summary>
        /// The text color property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.CreateAttached(
                "TextColor",
                typeof(Color),
                typeof(AddText),
                Color.Red
            );

        /// <summary>
        /// Sets the color of the text.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetTextColor(BindableObject view, Color value)
        {
            view.SetValue(TextColorProperty, value);
        }

        /// <summary>
        /// Gets the color of the text.
        /// </summary>
        /// <returns>The text color.</returns>
        /// <param name="view">View.</param>
        public static Color GetTextColor(BindableObject view)
        {
            return (Color)view.GetValue(TextColorProperty);
        }

        /// <summary>
        /// The background color property.
        /// </summary>
        public static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.CreateAttached(
                    "BackgroundColor",
                    typeof(Color),
                    typeof(AddText),
                    Color.Transparent
                );

        /// <summary>
        /// Sets the color of the background.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetBackgroundColor(BindableObject view, Color value)
        {
            view.SetValue(BackgroundColorProperty, value);
        }

        /// <summary>
        /// Gets the color of the background.
        /// </summary>
        /// <returns>The background color.</returns>
        /// <param name="view">View.</param>
        public static Color GetBackgroundColor(BindableObject view)
        {
            return (Color)view.GetValue(BackgroundColorProperty);
        }

        /// <summary>
        /// The padding property.
        /// </summary>
        public static readonly BindableProperty PaddingProperty =
            BindableProperty.CreateAttached(
                    "Padding",
                    typeof(Thickness),
                    typeof(AddText),
                    default(Thickness)
                );

        /// <summary>
        /// Sets the padding.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetPadding(BindableObject view, Thickness value)
        {
            view.SetValue(PaddingProperty, value);
        }

        /// <summary>
        /// Gets the padding.
        /// </summary>
        /// <returns>The padding.</returns>
        /// <param name="view">View.</param>
        public static Thickness GetPadding(BindableObject view)
        {
            return (Thickness)view.GetValue(PaddingProperty);
        }

        /// <summary>
        /// The margin property.
        /// </summary>
        public static readonly BindableProperty MarginProperty =
            BindableProperty.CreateAttached(
                "Margin",
                typeof(Thickness),
                typeof(AddText),
                default(Thickness)
            );

        /// <summary>
        /// Sets the margin.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetMargin(BindableObject view, Thickness value)
        {
            view.SetValue(MarginProperty, value);
        }

        /// <summary>
        /// Gets the margin.
        /// </summary>
        /// <returns>The margin.</returns>
        /// <param name="view">View.</param>
        public static Thickness GetMargin(BindableObject view)
        {
            return (Thickness)view.GetValue(MarginProperty);
        }

        /// <summary>
        /// The horizontal align property.
        /// </summary>
        public static readonly BindableProperty HorizontalAlignProperty =
            BindableProperty.CreateAttached(
                "HorizontalAlign",
                typeof(TextAlignment),
                typeof(AddText),
                TextAlignment.End
            );

        /// <summary>
        /// Sets the horizontal align.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetHorizontalAlign(BindableObject view, TextAlignment value)
        {
            view.SetValue(HorizontalAlignProperty, value);
        }

        /// <summary>
        /// Gets the horizontal align.
        /// </summary>
        /// <returns>The horizontal align.</returns>
        /// <param name="view">View.</param>
        public static TextAlignment GetHorizontalAlign(BindableObject view)
        {
            return (TextAlignment)view.GetValue(HorizontalAlignProperty);
        }

        /// <summary>
        /// The vertical align property.
        /// </summary>
        public static readonly BindableProperty VerticalAlignProperty =
            BindableProperty.CreateAttached(
                "VerticalAlign",
                typeof(TextAlignment),
                typeof(AddText),
                TextAlignment.Start
            );

        /// <summary>
        /// Sets the vertical align.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetVerticalAlign(BindableObject view, TextAlignment value)
        {
            view.SetValue(VerticalAlignProperty, value);
        }

        /// <summary>
        /// Gets the vertical align.
        /// </summary>
        /// <returns>The vertical align.</returns>
        /// <param name="view">View.</param>
        public static TextAlignment GetVerticalAlign(BindableObject view)
        {
            return (TextAlignment)view.GetValue(VerticalAlignProperty);
        }

    }


    internal class AddTextRoutingEffect : AiRoutingEffectBase
    {
        public AddTextRoutingEffect() : base("AiForms." + nameof(AddText)) { }
    }
}
