using System;
using Xamarin.Forms.Platform.Android;

namespace AiForms.Effects.Droid
{
    public abstract class AlterColorBase
    {
        IVisualElementRenderer _renderer;

        public AlterColorBase(IVisualElementRenderer renderer)
        {
            _renderer = renderer;
        }

        protected bool IsDisposed()
        {
            return _renderer?.Element == null;
        }
    }
}
