using System;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;

namespace AiForms.Effects
{
    /// <summary>
    /// Add date picker.
    /// </summary>
    public class AddDatePicker
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool?),
                declaringType: typeof(AddDatePicker),
                defaultValue: null,
                propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AddDatePickerRoutingEffect>
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
        /// The date property.
        /// </summary>
        public static readonly BindableProperty DateProperty =
            BindableProperty.CreateAttached(
                    "Date",
                    typeof(DateTime),
                    typeof(AddDatePicker),
                    default(DateTime),
                    defaultBindingMode:BindingMode.TwoWay,
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<AddDatePickerRoutingEffect>
                );

        /// <summary>
        /// Sets the date.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetDate(BindableObject view, DateTime value)
        {
            view.SetValue(DateProperty, value);
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <returns>The date.</returns>
        /// <param name="view">View.</param>
        public static DateTime GetDate(BindableObject view)
        {
            return (DateTime)view.GetValue(DateProperty);
        }

        /// <summary>
        /// The max date property.
        /// </summary>
        public static readonly BindableProperty MaxDateProperty =
            BindableProperty.CreateAttached(
                    "MaxDate",
                    typeof(DateTime),
                    typeof(AddDatePicker),
                    new DateTime(2100, 12, 31)
                );

        /// <summary>
        /// Sets the max date.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetMaxDate(BindableObject view, DateTime value)
        {
            view.SetValue(MaxDateProperty, value);
        }

        /// <summary>
        /// Gets the max date.
        /// </summary>
        /// <returns>The max date.</returns>
        /// <param name="view">View.</param>
        public static DateTime GetMaxDate(BindableObject view)
        {
            return (DateTime)view.GetValue(MaxDateProperty);
        }

        /// <summary>
        /// The minimum date property.
        /// </summary>
        public static readonly BindableProperty MinDateProperty =
            BindableProperty.CreateAttached(
                    "MinDate",
                    typeof(DateTime),
                    typeof(AddDatePicker),
                    new DateTime(1900, 1, 1)
                );

        /// <summary>
        /// Sets the minimum date.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetMinDate(BindableObject view, DateTime value)
        {
            view.SetValue(MinDateProperty, value);
        }

        /// <summary>
        /// Gets the minimum date.
        /// </summary>
        /// <returns>The minimum date.</returns>
        /// <param name="view">View.</param>
        public static DateTime GetMinDate(BindableObject view)
        {
            return (DateTime)view.GetValue(MinDateProperty);
        }

        /// <summary>
        /// The command property.
        /// </summary>
        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                "Command",
                typeof(ICommand),
                typeof(AddDatePicker),
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

        /// <summary>
        /// The today text property.
        /// </summary>
        public static readonly BindableProperty TodayTextProperty =
            BindableProperty.CreateAttached(
                "TodayText",
                typeof(string),
                typeof(AddDatePicker),
                default(string)
            );

        /// <summary>
        /// Sets the today text.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetTodayText(BindableObject view, string value)
        {
            view.SetValue(TodayTextProperty, value);
        }

        /// <summary>
        /// Gets the today text.
        /// </summary>
        /// <returns>The today text.</returns>
        /// <param name="view">View.</param>
        public static string GetTodayText(BindableObject view)
        {
            return (string)view.GetValue(TodayTextProperty);
        }
    }

    internal class AddDatePickerRoutingEffect : AiRoutingEffectBase
    {
        public AddDatePickerRoutingEffect() : base("AiForms." + nameof(AddDatePicker))
        {
        }
    }
}
