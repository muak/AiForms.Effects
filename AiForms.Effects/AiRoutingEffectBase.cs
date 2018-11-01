using System;
using Xamarin.Forms;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;

namespace AiForms.Effects
{
    public class AiRoutingEffectBase : RoutingEffect
    {
        internal static class InstanceCreator<TInstance>
        {
            public static Func<TInstance> Create { get; } = CreateInstance();

            static Func<TInstance> CreateInstance()
            {
                return Expression.Lambda<Func<TInstance>>(Expression.New(typeof(TInstance))).Compile();
            }
        }

        public static void AddEffectHandler<TRoutingEffect>(BindableObject bindable, object oldValue, object newValue) where TRoutingEffect :AiRoutingEffectBase
        {
            if (!EffectConfig.EnableTriggerProperty) 
                return;

            if (!(bindable is VisualElement element) || newValue == null)
                return;

            if (EffectShared<TRoutingEffect>.GetIsTriggered(element))
                return;

            AddEffect<TRoutingEffect>(element);
        }

        public static void ToggleEffectHandler<TRoutingEffect>(BindableObject bindable, object oldValue, object newValue) where TRoutingEffect : AiRoutingEffectBase
        {
            if (!(bindable is VisualElement element)) return;

            var enabled = (bool?)newValue;

            if (!enabled.HasValue) return;

            if(enabled.Value)
            {
                AddEffect<TRoutingEffect>(element);
            }
            else
            {
                RemoveEffect<TRoutingEffect>(element);
            }
        }

        static void AddEffect<T>(Element element) where T : AiRoutingEffectBase
        {
            if (!element.Effects.OfType<T>().Any())
            {
                element.Effects.Add(InstanceCreator<T>.Create());
                EffectShared<T>.SetIsTriggered(element, true);
            }
        }

        static void RemoveEffect<T>(Element element) where T : AiRoutingEffectBase
        {
            var remove = element.Effects.OfType<T>().FirstOrDefault();
            if (remove != null)
            {
                element.Effects.Remove(remove);
                // to avoid duplicate trigger
                Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                {
                    element.ClearValue(EffectShared<T>.IsTriggeredProperty);
                    return false;
                });
            }
        }


        public string EffectId { get; }
        public AiRoutingEffectBase(string effectId) : base(effectId)
        {
            EffectId = effectId;
        }
    }

    public static class EffectShared<T> where T:AiRoutingEffectBase
    {
        public static readonly BindableProperty IsTriggeredProperty =
            BindableProperty.CreateAttached(
                    "IsTriggered",
                    typeof(bool),
                    typeof(EffectShared<T>),
                    default(bool)
                );

        public static void SetIsTriggered(BindableObject view, bool value)
        {
            view.SetValue(IsTriggeredProperty, value);
        }

        public static bool GetIsTriggered(BindableObject view)
        {
            return (bool)view.GetValue(IsTriggeredProperty);
        }
    }
}
