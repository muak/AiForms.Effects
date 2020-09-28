using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Border.
    /// </summary>
    public static class Border
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool?),
                declaringType: typeof(Border),
                defaultValue: null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<BorderRoutingEffect>
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
        /// The radius property.
        /// </summary>
        public static readonly BindableProperty RadiusProperty =
            BindableProperty.CreateAttached(
                "Radius",
                typeof(double),
                typeof(Border),
                default(double),
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<BorderRoutingEffect>
            );

        /// <summary>
        /// Sets the radius.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetRadius(BindableObject view, double value)
        {
            view.SetValue(RadiusProperty, value);
        }

        /// <summary>
        /// Gets the radius.
        /// </summary>
        /// <returns>The radius.</returns>
        /// <param name="view">View.</param>
        public static double GetRadius(BindableObject view)
        {
            return (double)view.GetValue(RadiusProperty);
        }

        /// <summary>
        /// The width property.
        /// </summary>
        public static readonly BindableProperty WidthProperty =
            BindableProperty.CreateAttached(
                "Width",
                typeof(double?),
                typeof(Border),
                default(double?),
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<BorderRoutingEffect>
            );

        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetWidth(BindableObject view, double? value)
        {
            view.SetValue(WidthProperty, value);
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <returns>The width.</returns>
        /// <param name="view">View.</param>
        public static double? GetWidth(BindableObject view)
        {
            return (double?)view.GetValue(WidthProperty);
        }

        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly BindableProperty ColorProperty =
            BindableProperty.CreateAttached(
                "Color",
                typeof(Color),
                typeof(Border),
                Color.Transparent
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

    internal class BorderRoutingEffect : AiRoutingEffectBase
    {
        public BorderRoutingEffect() : base("AiForms." + nameof(Border)) { }
    }
}
