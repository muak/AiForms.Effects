using System;
using Xamarin.Forms.Platform.Android;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public abstract class AiEffectBase : PlatformEffect
    {
        public static bool IsFastRenderers = global::Xamarin.Forms.Forms.Flags.Any(x => x == "FastRenderers_Experimental");

        IVisualElementRenderer _renderer;
        bool _isDisposed = false;
        WeakReference<NavigationPage> _navigationRef;

        protected bool IsDisposed {
            get {
                if (_isDisposed) {
                    return _isDisposed;
                }

                if (_renderer == null) {
                    _renderer = (Container ?? Control) as IVisualElementRenderer;
                }

                if (IsFastRendererButton) {
                    return CheckButtonIsDisposed();
                }

                return _isDisposed = _renderer?.Tracker == null; //disposed check
            }
        }

        protected override void OnAttached()
        {
            Element view = Element;
            while (!(view is NavigationPage) && view != null)
            {
                view = view.Parent;
            }

            _navigationRef = new WeakReference<NavigationPage>(view as NavigationPage);

            if (_navigationRef.TryGetTarget(out var navi))
            {
                navi.Popped += PagePopped;
            }
            Xamarin.Forms.Application.Current.ModalPopped += ModalPopped;
        }


        // whether Element is FastRenderer.(Exept Button)
        protected bool IsFastRenderer{
            get{
                //If Container is null, it regards this as FastRenderer Element.
                //But this judging may not become right in the future. 
                return IsFastRenderers && (Container == null && !(Element is Xamarin.Forms.Button));
            }
        }

        // whether Element is a Button of FastRenderer.
        protected bool IsFastRendererButton{
            get{
                return (IsFastRenderers && (Element is Xamarin.Forms.Button));
            }
        }

        // whether Element can add ClickListener.
        protected bool IsClickable{
            get{
                return !(IsFastRenderer || Element is Xamarin.Forms.Layout || Element is Xamarin.Forms.BoxView);
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

        void PagePopped(object sender, NavigationEventArgs e)
        {
            Clear();
        }

        void ModalPopped(object sender, ModalPoppedEventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            if (_navigationRef.TryGetTarget(out var navi))
            {
                navi.Popped -= PagePopped;
            }
            Xamarin.Forms.Application.Current.ModalPopped -= ModalPopped;
            _navigationRef = null;

            // For Android, when a page is popped, OnDetached is automatically not called. (when iOS, it is called)
            // So, made the Popped & ModalPopped event subscribe in advance 
            // and make the effect manually removed when the page is popped.
            if (IsAttached && !IsDisposed)
            {
                var toRemove = Element.Effects.Cast<AiRoutingEffectBase>().FirstOrDefault(x => x.EffectId == ResolveId);
                Element.Effects.Remove(toRemove);
            }
        }
    }
}
