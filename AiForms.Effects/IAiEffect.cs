using System;

namespace AiForms.Effects
{
    /// <summary>
    /// Ai effect.
    /// </summary>
    public interface IAiEffect
    {
        void OnDetached();
        void Update();
    }
}
