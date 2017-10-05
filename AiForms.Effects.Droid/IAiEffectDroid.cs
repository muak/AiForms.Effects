using System;
using AiForms.Effects;

namespace AiForms.Effects.Droid
{
    public interface IAiEffectDroid : IAiEffect
    {
        void OnDetachedIfNotDisposed();
    }
}
