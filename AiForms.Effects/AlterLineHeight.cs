using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class AlterLineHeight
    {
        public static readonly BindableProperty OnProperty =
        BindableProperty.CreateAttached(
                "On",
                typeof(bool?),
                typeof(AlterLineHeight),
                null,
                BindingMode.OneWay,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<LineHeightEffectRoutingEffect>
            );

        public static void SetOn(BindableObject view, bool? value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool? GetOn(BindableObject view)
        {
            return (bool?)view.GetValue(OnProperty);
        }

        public static readonly BindableProperty MultipleProperty =
            BindableProperty.CreateAttached(
                    "Multiple",
                    typeof(double),
                    typeof(AlterLineHeight),
                    default(double),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<LineHeightEffectRoutingEffect>
                );

        public static void SetMultiple(BindableObject view, double value)
        {
            view.SetValue(MultipleProperty, value);
        }

        public static double GetMultiple(BindableObject view)
        {
            return (double)view.GetValue(MultipleProperty);
        }

        class LineHeightEffectRoutingEffect : AiRoutingEffectBase
        {
            public LineHeightEffectRoutingEffect() : base("AiForms." + nameof(AlterLineHeight))
            {
            }
        }

    }
}

