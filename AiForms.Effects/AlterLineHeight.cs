using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Alter line height.
    /// </summary>
    public static class AlterLineHeight
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
        BindableProperty.CreateAttached(
                "On",
                typeof(bool?),
                typeof(AlterLineHeight),
                null,
                BindingMode.OneWay,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AlterLineHeightRoutingEffect>
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
        /// The multiple property.
        /// </summary>
        public static readonly BindableProperty MultipleProperty =
            BindableProperty.CreateAttached(
                    "Multiple",
                    typeof(double),
                    typeof(AlterLineHeight),
                    default(double),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<AlterLineHeightRoutingEffect>
                );

        /// <summary>
        /// Sets the multiple.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetMultiple(BindableObject view, double value)
        {
            view.SetValue(MultipleProperty, value);
        }

        /// <summary>
        /// Gets the multiple.
        /// </summary>
        /// <returns>The multiple.</returns>
        /// <param name="view">View.</param>
        public static double GetMultiple(BindableObject view)
        {
            return (double)view.GetValue(MultipleProperty);
        }


    }

    internal class AlterLineHeightRoutingEffect : AiRoutingEffectBase
    {
        public AlterLineHeightRoutingEffect() : base("AiForms." + nameof(AlterLineHeight))
        {
        }
    }
}

