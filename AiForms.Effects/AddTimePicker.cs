using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Linq;

namespace AiForms.Effects
{
    public static class AddTimePicker
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                propertyName: "On",
                returnType: typeof(bool),
                declaringType: typeof(AddTimePicker),
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
                view.Effects.Add(new AddTimePickerRoutingEffect());
            }
            else {
                var toRemove = view.Effects.FirstOrDefault(e => e is AddTimePickerRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        public static readonly BindableProperty TimeProperty =
            BindableProperty.CreateAttached(
                    "Time",
                    typeof(TimeSpan),
                    typeof(AddTimePicker),
                    default(TimeSpan),
                    defaultBindingMode:BindingMode.TwoWay
                );

        public static void SetTime(BindableObject view, TimeSpan value)
        {
            view.SetValue(TimeProperty, value);
        }

        public static TimeSpan GetTime(BindableObject view)
        {
            return (TimeSpan)view.GetValue(TimeProperty);
        }


        public static readonly BindableProperty TitleProperty =
            BindableProperty.CreateAttached(
                    "Title",
                    typeof(string),
                    typeof(AddTimePicker),
                    default(string)
                );

        public static void SetTitle(BindableObject view, string value)
        {
            view.SetValue(TitleProperty, value);
        }

        public static string GetTitle(BindableObject view)
        {
            return (string)view.GetValue(TitleProperty);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                    "Command",
                    typeof(ICommand),
                    typeof(AddTimePicker),
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

        class AddTimePickerRoutingEffect : RoutingEffect
        {
            public AddTimePickerRoutingEffect() : base("AiForms." + nameof(AddTimePicker))
            {

            }
        }
    }
}
