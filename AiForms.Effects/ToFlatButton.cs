using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class ToFlatButton
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool?),
                    declaringType: typeof(ToFlatButton),
                    defaultValue: null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<ToFlatButtonRoutingEffect>
                );

        public static void SetOn(BindableObject view, bool? value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool? GetOn(BindableObject view)
        {
            return (bool?)view.GetValue(OnProperty);
        }


        public static readonly BindableProperty RippleColorProperty =
            BindableProperty.CreateAttached(
                    "RippleColor",
                    typeof(Color),
                    typeof(ToFlatButton),
                    default(Color),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<ToFlatButtonRoutingEffect>
                );

        public static void SetRippleColor(BindableObject view, Color value)
        {
            view.SetValue(RippleColorProperty, value);
        }

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
