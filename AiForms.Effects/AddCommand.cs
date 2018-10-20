using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System;

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

        public static readonly BindableProperty EnableSoundProperty =
            BindableProperty.CreateAttached(
                "EnableSound",
                typeof(bool),
                typeof(AddCommand),
                false);
                
        public static void SetEnableSound(BindableObject view, bool value)
        {
            view.SetValue(EnableSoundProperty, value);
        }

        public static bool GetEnableSound(BindableObject view)
        {
            return (bool)view.GetValue(EnableSoundProperty);
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

        public static readonly BindableProperty LongCommandProperty =
            BindableProperty.CreateAttached(
                    "LongCommand",
                    typeof(ICommand),
                    typeof(AddCommand),
                    default(ICommand)
                );

        public static void SetLongCommand(BindableObject view, ICommand value)
        {
            view.SetValue(LongCommandProperty, value);
        }

        public static ICommand GetLongCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(LongCommandProperty);
        }

        public static readonly BindableProperty LongCommandParameterProperty =
            BindableProperty.CreateAttached(
                    "LongCommandParameter",
                    typeof(object),
                    typeof(AddCommand),
                    default(object)
                );

        public static void SetLongCommandParameter(BindableObject view, object value)
        {
            view.SetValue(LongCommandParameterProperty, value);
        }

        public static object GetLongCommandParameter(BindableObject view)
        {
            return (object)view.GetValue(LongCommandParameterProperty);
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

        public static readonly BindableProperty EnableRippleProperty =
            BindableProperty.CreateAttached(
                    "EnableRipple",
                    typeof(bool),
                    typeof(AddCommand),
                    true
                );

        public static void SetEnableRipple(BindableObject view, bool value)
        {
            view.SetValue(EnableRippleProperty, value);
        }

        public static bool GetEnableRipple(BindableObject view)
        {
            return (bool)view.GetValue(EnableRippleProperty);
        }

        public static readonly BindableProperty SyncCanExecuteProperty =
            BindableProperty.CreateAttached(
                "SyncCanExecute",
                typeof(bool),
                typeof(AddCommand),
                false
            );

        public static void SetSyncCanExecute(BindableObject view, bool value)
        {
            view.SetValue(SyncCanExecuteProperty, value);
        }

        public static bool GetSyncCanExecute(BindableObject view)
        {
            return (bool)view.GetValue(SyncCanExecuteProperty);
        }

        class AddCommandRoutingEffect : RoutingEffect
        {
            public AddCommandRoutingEffect() : base("AiForms." + nameof(AddCommand)) { }
        }

    }

}

