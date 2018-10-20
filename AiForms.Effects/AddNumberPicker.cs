using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class AddNumberPicker
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool?),
                    declaringType: typeof(AddNumberPicker),
                    defaultValue: null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AddNumberPickerRoutingEffect>
                );

        public static void SetOn(BindableObject view, bool? value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool? GetOn(BindableObject view)
        {
            return (bool?)view.GetValue(OnProperty);
        }

        public static readonly BindableProperty MaxProperty =
            BindableProperty.CreateAttached(
                    "Max",
                    typeof(int),
                    typeof(AddNumberPicker),
                    default(int)
                );

        public static void SetMax(BindableObject view, int value)
        {
            view.SetValue(MaxProperty, value);
        }

        public static int GetMax(BindableObject view)
        {
            return (int)view.GetValue(MaxProperty);
        }

        public static readonly BindableProperty MinProperty =
            BindableProperty.CreateAttached(
                    "Min",
                    typeof(int),
                    typeof(AddNumberPicker),
                    default(int)
                );

        public static void SetMin(BindableObject view, int value)
        {
            view.SetValue(MinProperty, value);
        }

        public static int GetMin(BindableObject view)
        {
            return (int)view.GetValue(MinProperty);
        }

        public static readonly BindableProperty NumberProperty =
            BindableProperty.CreateAttached(
                    "Number",
                    typeof(int),
                    typeof(AddNumberPicker),
                    default(int),
                    BindingMode.TwoWay,
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<AddNumberPickerRoutingEffect>
                );

        public static void SetNumber(BindableObject view, int value)
        {
            view.SetValue(NumberProperty, value);
        }

        public static int GetNumber(BindableObject view)
        {
            return (int)view.GetValue(NumberProperty);
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.CreateAttached(
                    "Title",
                    typeof(string),
                    typeof(AddNumberPicker),
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
                    typeof(AddNumberPicker),
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



        class AddNumberPickerRoutingEffect : AiRoutingEffectBase
        {
            public AddNumberPickerRoutingEffect() : base("AiForms." + nameof(AddNumberPicker))
            {

            }
        }
    }
}
