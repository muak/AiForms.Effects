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
            if (!EffectConfig.EnableTriggerProperty) return;

            if (!(bindable is VisualElement element)) return;

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
            }
        }

        static void RemoveEffect<T>(Element element) where T : AiRoutingEffectBase
        {
            var remove = element.Effects.OfType<T>().FirstOrDefault();
            if (remove != null)
            {
                element.Effects.Remove(remove);
            }
        }


        public string EffectId { get; }
        public AiRoutingEffectBase(string effectId) : base(effectId)
        {
            EffectId = effectId;
        }
    }
}
