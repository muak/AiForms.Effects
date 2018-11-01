using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Size to fit.
    /// </summary>
    public static class SizeToFit
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                "On",
                typeof(bool?),
                typeof(SizeToFit),
                null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<SizeToFitRoutingEffect>
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
        /// The can expand property.
        /// </summary>
        public static readonly BindableProperty CanExpandProperty =
            BindableProperty.CreateAttached(
                    "CanExpand",
                    typeof(bool),
                    typeof(SizeToFit),
                    true
                );

        /// <summary>
        /// Sets the can expand.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void SetCanExpand(BindableObject view, bool value)
        {
            view.SetValue(CanExpandProperty, value);
        }

        /// <summary>
        /// Gets the can expand.
        /// </summary>
        /// <returns><c>true</c>, if can expand was gotten, <c>false</c> otherwise.</returns>
        /// <param name="view">View.</param>
        public static bool GetCanExpand(BindableObject view)
        {
            return (bool)view.GetValue(CanExpandProperty);
        }

    }

    internal class SizeToFitRoutingEffect : AiRoutingEffectBase
    {
        public SizeToFitRoutingEffect() : base("AiForms." + nameof(SizeToFit)) { }
    }
}
