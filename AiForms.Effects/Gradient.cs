using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AiForms.Effects
{
    /// <summary>
    /// GradientOrientation
    /// </summary>
    [TypeConverter(typeof(GradientOrientationTypeConverter))]
    public enum GradientOrientation
    {
        /// <summary>
        /// Left to Right
        /// </summary>
        LeftRight = 0,
        /// <summary>
        /// Bottom Left to Top Right
        /// </summary>
        BlTr = 45,
        /// <summary>
        /// Bottom to Top
        /// </summary>
        BottomTop = 90,
        /// <summary>
        /// Bottom Right to Top Left
        /// </summary>
        BrTl = 135,
        /// <summary>
        /// Right to Left
        /// </summary>
        RightLeft = 180,
        /// <summary>
        /// Top Right to Bottom Left
        /// </summary>
        TrBl = 225,
        /// <summary>
        /// Top to Bottom
        /// </summary>
        TopBottom = 270,
        /// <summary>
        /// Top Left to Bottom Right
        /// </summary>
        TlBr = 315,
    }

    /// <summary>
    /// GradientOrientationTypeConverter
    /// </summary>
    [TypeConversion(typeof(GradientOrientation))]
    public class GradientOrientationTypeConverter:TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                if (Enum.TryParse(value, true, out GradientOrientation orientation))
                    return orientation;
            }
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(GradientOrientation)}");
        }
    }

    /// <summary>
    /// GradientColors
    /// </summary>
    [TypeConverter(typeof(ColorsTypeConverter))]
    public class GradientColors:List<Color>
    {
        public GradientColors() : base() { }
        public GradientColors(IEnumerable<Color> colors) : base(colors) { }
    }

    /// <summary>
    /// ColorsTypeConverter
    /// </summary>
    [TypeConversion(typeof(GradientColors))]
    public class ColorsTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                var colors = value.Split(',');
                var conv = new ColorTypeConverter();

                return new GradientColors(colors.Select(x => (Color)conv.ConvertFromInvariantString(x)));
            }

            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(GradientColors)}");
        }
    }

    /// <summary>
    /// Gradient
    /// </summary>
    public static class Gradient
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool?),
                declaringType: typeof(Gradient),
                defaultValue: null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<GradientRoutingEffect>
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

        public static readonly BindableProperty ColorsProperty =
    BindableProperty.CreateAttached(
            "Colors",
            typeof(GradientColors),
            typeof(Gradient),
            default(GradientColors),
            propertyChanged: AiRoutingEffectBase.AddEffectHandler<GradientRoutingEffect>
        );

        public static void SetColors(BindableObject view, GradientColors value)
        {
            view.SetValue(ColorsProperty, value);
        }

        public static GradientColors GetColors(BindableObject view)
        {
            return (GradientColors)view.GetValue(ColorsProperty);
        }

        public static readonly BindableProperty OrientationProperty =
    BindableProperty.CreateAttached(
            "Orientation",
            typeof(GradientOrientation),
            typeof(Gradient),
            default(GradientOrientation)
        );

        public static void SetOrientation(BindableObject view, GradientOrientation value)
        {
            view.SetValue(OrientationProperty, value);
        }

        public static GradientOrientation GetOrientation(BindableObject view)
        {
            return (GradientOrientation)view.GetValue(OrientationProperty);
        }
    }

    internal class GradientRoutingEffect : AiRoutingEffectBase
    {
        public GradientRoutingEffect() : base("AiForms." + nameof(Gradient)) { }
    }
}
