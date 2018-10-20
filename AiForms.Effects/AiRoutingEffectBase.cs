using System;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public class AiRoutingEffectBase : RoutingEffect
    {
        public string EffectId { get; }
        public AiRoutingEffectBase(string effectId) : base(effectId)
        {
            EffectId = effectId;
        }
    }
}
