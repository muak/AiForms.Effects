using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class AlterColor
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool?),
                declaringType: typeof(AlterColor),
                defaultValue: null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AlterColorRoutingEffect>
            );

        public static void SetOn(BindableObject view, bool? value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool? GetOn(BindableObject view)
        {
            return (bool?)view.GetValue(OnProperty);
        }


        public static readonly BindableProperty AccentProperty =
            BindableProperty.CreateAttached(
                "Accent",
                typeof(Color),
                typeof(AlterColor),
                default(Color),
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<AlterColorRoutingEffect>
            );

        public static void SetAccent(BindableObject view, Color value)
        {
            view.SetValue(AccentProperty, value);
        }

        public static Color GetAccent(BindableObject view)
        {
            return (Color)view.GetValue(AccentProperty);
        }

        class AlterColorRoutingEffect : AiRoutingEffectBase
        {
            public AlterColorRoutingEffect() : base("AiForms." + nameof(AlterColor)) { }
        }
    }
}
