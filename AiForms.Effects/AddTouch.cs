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
                    typeof(bool),
                    typeof(AddTouch),
                    default(bool),
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

            if ((bool)newValue)
            {
                SetRecognizer(view, new TouchRecognizer());
                view.Effects.Add(new AddTouchRoutingEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is AddTouchRoutingEffect);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                    SetRecognizer(view, null);
                }
                
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

        class AddTouchRoutingEffect : RoutingEffect
        {
            public AddTouchRoutingEffect() : base("AiForms." + nameof(AddTouch)) { }
        }
    }
}
