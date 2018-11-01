using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Linq;

namespace AiForms.Effects
{
    /// <summary>
    /// Add time picker.
    /// </summary>
    public static class AddTimePicker
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool?),
                declaringType: typeof(AddTimePicker),
                defaultValue: null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AddTimePickerRoutingEffect>
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
        /// The time property.
        /// </summary>
        public static readonly BindableProperty TimeProperty =
            BindableProperty.CreateAttached(
                    "Time",
                    typeof(TimeSpan),
                    typeof(AddTimePicker),
                    default(TimeSpan),
                    defaultBindingMode:BindingMode.TwoWay,
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<AddTimePickerRoutingEffect>
                );

        /// <summary>
        /// Sets the time.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetTime(BindableObject view, TimeSpan value)
        {
            view.SetValue(TimeProperty, value);
        }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <returns>The time.</returns>
        /// <param name="view">View.</param>
        public static TimeSpan GetTime(BindableObject view)
        {
            return (TimeSpan)view.GetValue(TimeProperty);
        }

        /// <summary>
        /// The title property.
        /// </summary>
        public static readonly BindableProperty TitleProperty =
            BindableProperty.CreateAttached(
                    "Title",
                    typeof(string),
                    typeof(AddTimePicker),
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
                    typeof(AddTimePicker),
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

    internal class AddTimePickerRoutingEffect : AiRoutingEffectBase
    {
        public AddTimePickerRoutingEffect() : base("AiForms." + nameof(AddTimePicker))
        {
        }
    }
}
