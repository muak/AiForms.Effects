using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Alter color.
    /// </summary>
    public static class AlterColor
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool?),
                declaringType: typeof(AlterColor),
                defaultValue: null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AlterColorRoutingEffect>
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
        /// The accent property.
        /// </summary>
        public static readonly BindableProperty AccentProperty =
            BindableProperty.CreateAttached(
                "Accent",
                typeof(Color),
                typeof(AlterColor),
                default(Color),
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<AlterColorRoutingEffect>
            );

        /// <summary>
        /// Sets the accent.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetAccent(BindableObject view, Color value)
        {
            view.SetValue(AccentProperty, value);
        }

        /// <summary>
        /// Gets the accent.
        /// </summary>
        /// <returns>The accent.</returns>
        /// <param name="view">View.</param>
        public static Color GetAccent(BindableObject view)
        {
            return (Color)view.GetValue(AccentProperty);
        }

    }

    internal class AlterColorRoutingEffect : AiRoutingEffectBase
    {
        public AlterColorRoutingEffect() : base("AiForms." + nameof(AlterColor)) { }
    }
}
