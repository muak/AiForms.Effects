using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Placeholder.
    /// </summary>
    public static class Placeholder
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool?),
                declaringType: typeof(Placeholder),
                defaultValue: null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<PlaceholderRoutingEffect>
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
                    typeof(Placeholder),
                    default(string),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<PlaceholderRoutingEffect>
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
        /// The color property.
        /// </summary>
        public static readonly BindableProperty ColorProperty =
            BindableProperty.CreateAttached(
                    "Color",
                    typeof(Color),
                    typeof(Placeholder),
                    default(Color)
                );

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetColor(BindableObject view, Color value)
        {
            view.SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <returns>The color.</returns>
        /// <param name="view">View.</param>
        public static Color GetColor(BindableObject view)
        {
            return (Color)view.GetValue(ColorProperty);
        }
    }

    internal class PlaceholderRoutingEffect : AiRoutingEffectBase
    {
        public PlaceholderRoutingEffect() : base("AiForms." + nameof(Placeholder)) { }
    }
}
