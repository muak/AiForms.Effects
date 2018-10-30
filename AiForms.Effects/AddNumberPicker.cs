using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Add number picker.
    /// </summary>
    public static class AddNumberPicker
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool?),
                    declaringType: typeof(AddNumberPicker),
                    defaultValue: null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AddNumberPickerRoutingEffect>
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
        /// The max property.
        /// </summary>
        public static readonly BindableProperty MaxProperty =
            BindableProperty.CreateAttached(
                    "Max",
                    typeof(int),
                    typeof(AddNumberPicker),
                    default(int)
                );

        /// <summary>
        /// Sets the max.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetMax(BindableObject view, int value)
        {
            view.SetValue(MaxProperty, value);
        }

        /// <summary>
        /// Gets the max.
        /// </summary>
        /// <returns>The max.</returns>
        /// <param name="view">View.</param>
        public static int GetMax(BindableObject view)
        {
            return (int)view.GetValue(MaxProperty);
        }

        /// <summary>
        /// The minimum property.
        /// </summary>
        public static readonly BindableProperty MinProperty =
            BindableProperty.CreateAttached(
                    "Min",
                    typeof(int),
                    typeof(AddNumberPicker),
                    default(int)
                );

        /// <summary>
        /// Sets the minimum.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetMin(BindableObject view, int value)
        {
            view.SetValue(MinProperty, value);
        }

        /// <summary>
        /// Gets the minimum.
        /// </summary>
        /// <returns>The minimum.</returns>
        /// <param name="view">View.</param>
        public static int GetMin(BindableObject view)
        {
            return (int)view.GetValue(MinProperty);
        }

        /// <summary>
        /// The number property.
        /// </summary>
        public static readonly BindableProperty NumberProperty =
            BindableProperty.CreateAttached(
                    "Number",
                    typeof(int),
                    typeof(AddNumberPicker),
                    default(int),
                    BindingMode.TwoWay,
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<AddNumberPickerRoutingEffect>
                );

        /// <summary>
        /// Sets the number.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetNumber(BindableObject view, int value)
        {
            view.SetValue(NumberProperty, value);
        }

        /// <summary>
        /// Gets the number.
        /// </summary>
        /// <returns>The number.</returns>
        /// <param name="view">View.</param>
        public static int GetNumber(BindableObject view)
        {
            return (int)view.GetValue(NumberProperty);
        }

        /// <summary>
        /// The title property.
        /// </summary>
        public static readonly BindableProperty TitleProperty =
            BindableProperty.CreateAttached(
                    "Title",
                    typeof(string),
                    typeof(AddNumberPicker),
                    default(string)
                );

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetTitle(BindableObject view, string value)
        {
            view.SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <returns>The title.</returns>
        /// <param name="view">View.</param>
        public static string GetTitle(BindableObject view)
        {
            return (string)view.GetValue(TitleProperty);
        }

        /// <summary>
        /// The command property.
        /// </summary>
        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                    "Command",
                    typeof(ICommand),
                    typeof(AddNumberPicker),
                    default(ICommand)
                );

        /// <summary>
        /// Sets the command.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetCommand(BindableObject view, ICommand value)
        {
            view.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <returns>The command.</returns>
        /// <param name="view">View.</param>
        public static ICommand GetCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(CommandProperty);
        }
    }

    internal class AddNumberPickerRoutingEffect : AiRoutingEffectBase
    {
        public AddNumberPickerRoutingEffect() : base("AiForms." + nameof(AddNumberPicker))
        {
        }
    }
}
