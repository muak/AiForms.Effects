using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class AddCommand
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool),
                    declaringType: typeof(AddCommand),
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
                view.Effects.Add(new AddCommandRoutingEffect());
            }
            else {
                var toRemove = view.Effects.FirstOrDefault(e => e is AddCommandRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                    "Command",
                    typeof(ICommand),
                    typeof(AddCommand),
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


        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.CreateAttached(
                    "CommandParameter",
                    typeof(object),
                    typeof(AddCommand),
                    default(object)
                );

        public static void SetCommandParameter(BindableObject view, object value)
        {
            view.SetValue(CommandParameterProperty, value);
        }

        public static object GetCommandParameter(BindableObject view)
        {
            return (object)view.GetValue(CommandParameterProperty);
        }

        public static readonly BindableProperty EffectColorProperty =
            BindableProperty.CreateAttached(
                    "EffectColor",
                    typeof(Color),
                    typeof(AddCommand),
                    Color.Default
                );

        public static void SetEffectColor(BindableObject view, Color value)
        {
            view.SetValue(EffectColorProperty, value);
        }

        public static Color GetEffectColor(BindableObject view)
        {
            return (Color)view.GetValue(EffectColorProperty);
        }

        class AddCommandRoutingEffect : RoutingEffect
        {
            public AddCommandRoutingEffect() : base("AiForms." + nameof(AddCommand)) { }
        }
    }

}

