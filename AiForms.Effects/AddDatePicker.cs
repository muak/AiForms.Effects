using System;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;

namespace AiForms.Effects
{
    public class AddDatePicker
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool),
                declaringType: typeof(AddDatePicker),
                defaultValue: false,
                propertyChanged: OnOffChanged
            );

        public static void SetOn(BindableObject view, bool value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool GetOn(BindableObject view)
        {
            return (bool)view.GetValue(OnProperty);
        }

        private static void OnOffChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
                return;

            if ((bool)newValue) {
                view.Effects.Add(new AddDatePickerRoutingEffect());
            }
            else {
                var toRemove = view.Effects.FirstOrDefault(e => e is AddDatePickerRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        public static readonly BindableProperty DateProperty =
            BindableProperty.CreateAttached(
                    "Date",
                    typeof(DateTime),
                    typeof(AddDatePicker),
                    default(DateTime),
                    defaultBindingMode:BindingMode.TwoWay
                );

        public static void SetDate(BindableObject view, DateTime value)
        {
            view.SetValue(DateProperty, value);
        }

        public static DateTime GetDate(BindableObject view)
        {
            return (DateTime)view.GetValue(DateProperty);
        }

        public static readonly BindableProperty MaxDateProperty =
            BindableProperty.CreateAttached(
                    "MaxDate",
                    typeof(DateTime),
                    typeof(AddDatePicker),
                    new DateTime(2100, 12, 31)
                );

        public static void SetMaxDate(BindableObject view, DateTime value)
        {
            view.SetValue(MaxDateProperty, value);
        }

        public static DateTime GetMaxDate(BindableObject view)
        {
            return (DateTime)view.GetValue(MaxDateProperty);
        }

        public static readonly BindableProperty MinDateProperty =
            BindableProperty.CreateAttached(
                    "MinDate",
                    typeof(DateTime),
                    typeof(AddDatePicker),
                    new DateTime(1900, 1, 1)
                );

        public static void SetMinDate(BindableObject view, DateTime value)
        {
            view.SetValue(MinDateProperty, value);
        }

        public static DateTime GetMinDate(BindableObject view)
        {
            return (DateTime)view.GetValue(MinDateProperty);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                "Command",
                typeof(ICommand),
                typeof(AddDatePicker),
                default(ICommand)
            );

        public static void SetCommand(BindableObject view, ICommand value)
        {
            view.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(CommandProperty);
        }

        public static readonly BindableProperty TodayTextProperty =
            BindableProperty.CreateAttached(
                "TodayText",
                typeof(string),
                typeof(AddDatePicker),
                default(string)
            );

        public static void SetTodayText(BindableObject view, string value)
        {
            view.SetValue(TodayTextProperty, value);
        }

        public static string GetTodayText(BindableObject view)
        {
            return (string)view.GetValue(TodayTextProperty);
        }

        class AddDatePickerRoutingEffect : RoutingEffect
        {
            public AddDatePickerRoutingEffect() : base("AiForms." + nameof(AddDatePicker))
            {

            }
        }

    }
}
