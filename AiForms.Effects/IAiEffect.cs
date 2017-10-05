using System;

namespace AiForms.Effects
{
    public interface IAiEffect
    {
        void OnDetached();
        void Update();
    }
}
