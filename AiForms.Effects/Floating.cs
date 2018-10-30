using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Floating.
    /// </summary>
    public static class Floating
    {
        /// <summary>
        /// The content property.
        /// </summary>
        public static readonly BindableProperty ContentProperty =
            BindableProperty.CreateAttached(
                    "Content",
                    typeof(FloatingLayout),
                    typeof(Floating),
                    default(FloatingLayout),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<FloatingRoutingEffect>
                );

        /// <summary>
        /// Sets the content.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetContent(BindableObject view, FloatingLayout value)
        {
            view.SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>The content.</returns>
        /// <param name="view">View.</param>
        public static FloatingLayout GetContent(BindableObject view)
        {
            return (FloatingLayout)view.GetValue(ContentProperty);
        }
    }

    internal class FloatingRoutingEffect : AiRoutingEffectBase
    {
        public FloatingRoutingEffect() : base("AiForms." + nameof(Floating))
        {
        }
    }
}
