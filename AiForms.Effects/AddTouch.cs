using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Add touch.
    /// </summary>
    public static class AddTouch
    {
        /// <summary>
        /// The on property.
        /// </summary>
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    "On",
                    typeof(bool?),
                    typeof(AddTouch),
                    null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AddTouchRoutingEffect>,
                    propertyChanging: OnOffChanging            
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
        /// Ons the off changing.
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        static void OnOffChanging(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view))
                return;

            if ((bool)newValue)
            {
                SetRecognizer(view, new TouchRecognizer());
            }
            else
            {
                view.ClearValue(RecognizerProperty);                
            }
        }

        /// <summary>
        /// The recognizer property.
        /// </summary>
        public static readonly BindableProperty RecognizerProperty =
            BindableProperty.CreateAttached(
                    "Recognizer",
                    typeof(TouchRecognizer),
                    typeof(AddTouch),
                    default(TouchRecognizer)
                );

        /// <summary>
        /// Sets the recognizer.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="value">Value.</param>
        public static void SetRecognizer(BindableObject view, TouchRecognizer value)
        {
            view.SetValue(RecognizerProperty, value);
        }

        /// <summary>
        /// Gets the recognizer.
        /// </summary>
        /// <returns>The recognizer.</returns>
        /// <param name="view">View.</param>
        public static TouchRecognizer GetRecognizer(BindableObject view)
        {
            return (TouchRecognizer)view.GetValue(RecognizerProperty);
        }
    }

    internal class AddTouchRoutingEffect : AiRoutingEffectBase
    {
        public AddTouchRoutingEffect() : base("AiForms." + nameof(AddTouch))
        {
        }
    }
}
