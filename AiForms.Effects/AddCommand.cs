using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace AiForms.Effects
{
    /// <summary>
    /// Add command.
    /// </summary>
    public static class AddCommand
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool?),
                    declaringType: typeof(AddCommand),
                    defaultValue: null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AddCommandRoutingEffect>
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
        /// The enable sound property.
        /// </summary>
        public static readonly BindableProperty EnableSoundProperty =
            BindableProperty.CreateAttached(
                "EnableSound",
                typeof(bool),
                typeof(AddCommand),
                false);

        /// <summary>
        /// Sets the enable sound.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void SetEnableSound(BindableObject view, bool value)
        {
            view.SetValue(EnableSoundProperty, value);
        }

        /// <summary>
        /// Gets the enable sound.
        /// </summary>
        /// <returns><c>true</c>, if enable sound was gotten, <c>false</c> otherwise.</returns>
        /// <param name="view">View.</param>
        public static bool GetEnableSound(BindableObject view)
        {
            return (bool)view.GetValue(EnableSoundProperty);
        }

        /// <summary>
        /// The command property.
        /// </summary>
        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                    "Command",
                    typeof(ICommand),
                    typeof(AddCommand),
                    default(ICommand),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<AddCommandRoutingEffect>
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
        /// The command parameter property.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.CreateAttached(
                    "CommandParameter",
                    typeof(object),
                    typeof(AddCommand),
                    default(object)
                );

        /// <summary>
        /// Sets the command parameter.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetCommandParameter(BindableObject view, object value)
        {
            view.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Gets the command parameter.
        /// </summary>
        /// <returns>The command parameter.</returns>
        /// <param name="view">View.</param>
        public static object GetCommandParameter(BindableObject view)
        {
            return (object)view.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// The long command property.
        /// </summary>
        public static readonly BindableProperty LongCommandProperty =
            BindableProperty.CreateAttached(
                    "LongCommand",
                    typeof(ICommand),
                    typeof(AddCommand),
                    default(ICommand),
                    propertyChanged: AiRoutingEffectBase.AddEffectHandler<AddCommandRoutingEffect>
                );

        /// <summary>
        /// Sets the long command.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetLongCommand(BindableObject view, ICommand value)
        {
            view.SetValue(LongCommandProperty, value);
        }

        /// <summary>
        /// Gets the long command.
        /// </summary>
        /// <returns>The long command.</returns>
        /// <param name="view">View.</param>
        public static ICommand GetLongCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(LongCommandProperty);
        }

        /// <summary>
        /// The long command parameter property.
        /// </summary>
        public static readonly BindableProperty LongCommandParameterProperty =
            BindableProperty.CreateAttached(
                    "LongCommandParameter",
                    typeof(object),
                    typeof(AddCommand),
                    default(object)
                );

        /// <summary>
        /// Sets the long command parameter.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetLongCommandParameter(BindableObject view, object value)
        {
            view.SetValue(LongCommandParameterProperty, value);
        }

        /// <summary>
        /// Gets the long command parameter.
        /// </summary>
        /// <returns>The long command parameter.</returns>
        /// <param name="view">View.</param>
        public static object GetLongCommandParameter(BindableObject view)
        {
            return (object)view.GetValue(LongCommandParameterProperty);
        }

        /// <summary>
        /// The effect color property.
        /// </summary>
        public static readonly BindableProperty EffectColorProperty =
            BindableProperty.CreateAttached(
                    "EffectColor",
                    typeof(Color),
                    typeof(AddCommand),
                    Color.Transparent
                );

        /// <summary>
        /// Sets the color of the effect.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetEffectColor(BindableObject view, Color value)
        {
            view.SetValue(EffectColorProperty, value);
        }

        /// <summary>
        /// Gets the color of the effect.
        /// </summary>
        /// <returns>The effect color.</returns>
        /// <param name="view">View.</param>
        public static Color GetEffectColor(BindableObject view)
        {
            return (Color)view.GetValue(EffectColorProperty);
        }

        [Obsolete("This property is obsolete as of version 1.4. Ripple color is transparent by default.")]
        public static readonly BindableProperty EnableRippleProperty =
            BindableProperty.CreateAttached(
                    "EnableRipple",
                    typeof(bool),
                    typeof(AddCommand),
                    true
                );

        [Obsolete("This method is obsolete as of version 1.4. Ripple color is transparent by default.")]
        public static void SetEnableRipple(BindableObject view, bool value)
        {
            view.SetValue(EnableRippleProperty, value);
        }

        [Obsolete("This method is obsolete as of version 1.4. Ripple color is transparent by default.")]
        public static bool GetEnableRipple(BindableObject view)
        {
            return (bool)view.GetValue(EnableRippleProperty);
        }

        /// <summary>
        /// The sync can execute property.
        /// </summary>
        public static readonly BindableProperty SyncCanExecuteProperty =
            BindableProperty.CreateAttached(
                "SyncCanExecute",
                typeof(bool),
                typeof(AddCommand),
                false
            );

        /// <summary>
        /// Sets the sync can execute.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void SetSyncCanExecute(BindableObject view, bool value)
        {
            view.SetValue(SyncCanExecuteProperty, value);
        }

        /// <summary>
        /// Gets the sync can execute.
        /// </summary>
        /// <returns><c>true</c>, if sync can execute was gotten, <c>false</c> otherwise.</returns>
        /// <param name="view">View.</param>
        public static bool GetSyncCanExecute(BindableObject view)
        {
            return (bool)view.GetValue(SyncCanExecuteProperty);
        }
    }

    internal class AddCommandRoutingEffect : AiRoutingEffectBase
    {
        public AddCommandRoutingEffect() : base("AiForms." + nameof(AddCommand)) { }
    }
}

