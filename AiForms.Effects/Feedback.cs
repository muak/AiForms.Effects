using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Feedback.
    /// </summary>
    public static class Feedback
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool?),
                    declaringType: typeof(Feedback),
                    defaultValue: null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<FeedbackRoutingEffect>
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
        /// The effect color property.
        /// </summary>
        public static readonly BindableProperty EffectColorProperty =
            BindableProperty.CreateAttached(
                "EffectColor",
                typeof(Color),
                typeof(Feedback),
                Color.Transparent,
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<FeedbackRoutingEffect>
            );

        /// <summary>
        /// Sets the color of the effect.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetEffectColor(BindableObject view, Color value)
        {
            view.SetValue(EffectColorProperty, value);
        }

        /// <summary>
        /// Gets the color of the effect.
        /// </summary>
        /// <returns>The effect color.</returns>
        /// <param name="view">View.</param>
        public static Color GetEffectColor(BindableObject view)
        {
            return (Color)view.GetValue(EffectColorProperty);
        }

        /// <summary>
        /// The enable sound property.
        /// </summary>
        public static readonly BindableProperty EnableSoundProperty =
            BindableProperty.CreateAttached(
                "EnableSound",
                typeof(bool),
                typeof(Feedback),
                false,
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<FeedbackRoutingEffect>
            );

        /// <summary>
        /// Sets the enable sound.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void SetEnableSound(BindableObject view, bool value)
        {
            view.SetValue(EnableSoundProperty, value);
        }

        /// <summary>
        /// Gets the enable sound.
        /// </summary>
        /// <returns><c>true</c>, if enable sound was gotten, <c>false</c> otherwise.</returns>
        /// <param name="view">View.</param>
        public static bool GetEnableSound(BindableObject view)
        {
            return (bool)view.GetValue(EnableSoundProperty);
        }

    }

    internal class FeedbackRoutingEffect : AiRoutingEffectBase
    {
        public FeedbackRoutingEffect() : base("AiForms." + nameof(Feedback))
        {
        }
    }
}
