using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// To flat button.
    /// </summary>
    public static class ToFlatButton
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool?),
                    declaringType: typeof(ToFlatButton),
                    defaultValue: null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<ToFlatButtonRoutingEffect>
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
        /// The ripple color property.
        /// </summary>
        public static readonly BindableProperty RippleColorProperty =
            BindableProperty.CreateAttached(
                    "RippleColor",
                    typeof(Color),
                    typeof(ToFlatButton),
                    default(Color),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<ToFlatButtonRoutingEffect>
                );

        /// <summary>
        /// Sets the color of the ripple.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetRippleColor(BindableObject view, Color value)
        {
            view.SetValue(RippleColorProperty, value);
        }

        /// <summary>
        /// Gets the color of the ripple.
        /// </summary>
        /// <returns>The ripple color.</returns>
        /// <param name="view">View.</param>
        public static Color GetRippleColor(BindableObject view)
        {
            return (Color)view.GetValue(RippleColorProperty);
        }

    }

    internal class ToFlatButtonRoutingEffect : AiRoutingEffectBase
    {
        public ToFlatButtonRoutingEffect() : base("AiForms." + nameof(ToFlatButton)) { }
    }
}
