using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class Feedback
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool?),
                    declaringType: typeof(Feedback),
                    defaultValue: null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<FeedbackRoutingEffect>
                );

        public static void SetOn(BindableObject view, bool? value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool? GetOn(BindableObject view)
        {
            return (bool?)view.GetValue(OnProperty);
        }

        public static readonly BindableProperty EffectColorProperty =
            BindableProperty.CreateAttached(
                "EffectColor",
                typeof(Color),
                typeof(Feedback),
                Color.Transparent,
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<FeedbackRoutingEffect>
            );

        public static void SetEffectColor(BindableObject view, Color value)
        {
            view.SetValue(EffectColorProperty, value);
        }

        public static Color GetEffectColor(BindableObject view)
        {
            return (Color)view.GetValue(EffectColorProperty);
        }

        public static readonly BindableProperty EnableSoundProperty =
            BindableProperty.CreateAttached(
                "EnableSound",
                typeof(bool),
                typeof(Feedback),
                false,
                propertyChanged: AiRoutingEffectBase.AddEffectHandler<FeedbackRoutingEffect>
            );

        public static void SetEnableSound(BindableObject view, bool value)
        {
            view.SetValue(EnableSoundProperty, value);
        }
        public static bool GetEnableSound(BindableObject view)
        {
            return (bool)view.GetValue(EnableSoundProperty);
        }


        class FeedbackRoutingEffect : AiRoutingEffectBase
        {
            public FeedbackRoutingEffect() : base("AiForms." + nameof(Feedback)) 
            {
            }
        }

    }
}
