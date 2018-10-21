using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class Floating
    {
        public static readonly BindableProperty ContentProperty =
            BindableProperty.CreateAttached(
                    "Content",
                    typeof(FloatingLayout),
                    typeof(Floating),
                    default(FloatingLayout),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<FloatingRoutingEffect>
                );

        public static void SetContent(BindableObject view, FloatingLayout value)
        {
            view.SetValue(ContentProperty, value);
        }

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
