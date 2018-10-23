using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class AddTouch
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    "On",
                    typeof(bool?),
                    typeof(AddTouch),
                    null,
                    propertyChanged: AiRoutingEffectBase.ToggleEffectHandler<AddTouchRoutingEffect>,
                    propertyChanging: OnOffChanging            
                );

        public static void SetOn(BindableObject view, bool? value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool? GetOn(BindableObject view)
        {
            return (bool?)view.GetValue(OnProperty);
        }


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

        public static readonly BindableProperty RecognizerProperty =
            BindableProperty.CreateAttached(
                    "Recognizer",
                    typeof(TouchRecognizer),
                    typeof(AddTouch),
                    default(TouchRecognizer)
                );

        public static void SetRecognizer(BindableObject view, TouchRecognizer value)
        {
            view.SetValue(RecognizerProperty, value);
        }

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
