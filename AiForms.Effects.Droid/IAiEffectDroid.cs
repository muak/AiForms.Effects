using System;
using AiForms.Effects;

namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public interface IAiEffectDroid : IAiEffect
    {
        void OnDetachedIfNotDisposed();
    }
}
