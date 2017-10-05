using System;
using Xamarin.Forms.Platform.Android;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace AiForms.Effects.Droid
{
    public abstract class AiEffectBase : PlatformEffect
    {
        IVisualElementRenderer _renderer;
        bool _isDisposed = false;
        protected bool IsDisposed {
            get {
                if (_isDisposed) {
                    return _isDisposed;
                }

                if (_renderer == null) {
                    _renderer = (Container ?? Control) as IVisualElementRenderer;
                }

                if (Element.IsFastRendererButton()) {
                    return CheckButtonIsDisposed();
                }

                return _isDisposed = _renderer?.Tracker == null; //disposed check
            }
        }

        static Func<object, object> GetDisposed; //cache

        // In case Button of FastRenderer, IVisualElementRenderer.Tracker don't become null.
        // So refered to the private field of "_disposed", judge whether being disposed. 
        bool CheckButtonIsDisposed()
        {
            if (GetDisposed == null) {
                GetDisposed = CreateGetField(typeof(VisualElementTracker));
            }
            _isDisposed = (bool)GetDisposed(_renderer.Tracker);

            return _isDisposed;
        }

        Func<object, object> CreateGetField(Type t)
        {
            var prop = t.GetRuntimeFields()
                                .Where(x => x.DeclaringType == t && x.Name == "_disposed").FirstOrDefault();

            var target = Expression.Parameter(typeof(object), "target");

            var body = Expression.PropertyOrField(Expression.Convert(target, t), prop.Name);

            var lambda = Expression.Lambda<Func<object, object>>(
                Expression.Convert(body, typeof(object)), target
            );

            return lambda.Compile();
        }
    }
}
