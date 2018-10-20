using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class SizeToFit
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                "On",
                typeof(bool?),
                typeof(SizeToFit),
                null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<SizeToFitRoutingEffect>
            );

        public static void SetOn(BindableObject view, bool? value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool? GetOn(BindableObject view)
        {
            return (bool?)view.GetValue(OnProperty);
        }

        public static readonly BindableProperty CanExpandProperty =
            BindableProperty.CreateAttached(
                    "CanExpand",
                    typeof(bool),
                    typeof(SizeToFit),
                    true,
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<SizeToFitRoutingEffect>
                );

        public static void SetCanExpand(BindableObject view, bool value)
        {
            view.SetValue(CanExpandProperty, value);
        }

        public static bool GetCanExpand(BindableObject view)
        {
            return (bool)view.GetValue(CanExpandProperty);
        }

        class SizeToFitRoutingEffect : AiRoutingEffectBase
        {
            public SizeToFitRoutingEffect() : base("AiForms." + nameof(SizeToFit)) { }
        }
    }
}
