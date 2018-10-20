using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public static class Feedback
    {
        public static readonly BindableProperty OnProperty =
            BindableProperty.CreateAttached(
                    propertyName: "On",
                    returnType: typeof(bool),
                    declaringType: typeof(Feedback),
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

        public static readonly BindableProperty EffectColorProperty =
            BindableProperty.CreateAttached(
                    "EffectColor",
                    typeof(Color),
                    typeof(Feedback),
                    Color.Transparent,
                    propertyChanged: OnPropertyChanged
                );

        public static void SetEffectColor(BindableObject view, Color value)
        {
            view.SetValue(EffectColorProperty, value);
        }

        public static Color GetEffectColor(BindableObject view)
        {
            return (Color)view.GetValue(EffectColorProperty);
        }

        public static readonly BindableProperty EnableSoundProperty =
            BindableProperty.CreateAttached(
                "EnableSound",
                typeof(bool),
                typeof(Feedback),
                false);

        public static void SetEnableSound(BindableObject view, bool value)
        {
            view.SetValue(EnableSoundProperty, value);
        }
        public static bool GetEnableSound(BindableObject view)
        {
            return (bool)view.GetValue(EnableSoundProperty);
        }

        static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (GetOn(bindable)) return;

            SetOn(bindable, true);
        }

        static void OnOffChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
                return;

            if ((bool)newValue)
            {
                view.Effects.Add(new FeedbackRoutingEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is FeedbackRoutingEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        class FeedbackRoutingEffect : AiRoutingEffectBase
        {
            public FeedbackRoutingEffect() : base("AiForms." + nameof(Feedback)) 
            {
            }
        }

    }
}
