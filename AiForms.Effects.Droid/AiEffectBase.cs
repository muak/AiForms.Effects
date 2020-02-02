using System;
using Xamarin.Forms.Platform.Android;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using Xamarin.Forms;
using Android.App;

namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public abstract class AiEffectBase : PlatformEffect
    {
        public static bool IsFastRenderers = global::Xamarin.Forms.Forms.Flags.Any(x => x == "FastRenderers_Experimental");

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

                if (IsFastRendererButton) {
                    return CheckButtonIsDisposed();
                }

                return _isDisposed = _renderer?.Tracker == null; //disposed check
            }
        }

        protected bool IsNullOrDisposed{
            get{
                if(Element.BindingContext == null){
                    return true;
                }

                return IsDisposed;
            }
        }

        protected bool IsSupportedByApi => Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop;

        protected override void OnAttached()
        {
            if (!IsSupportedByApi)
                return;

            var visual = Element as VisualElement;
            var page = visual.Navigation.NavigationStack.LastOrDefault();
            if(page == null)
            {
                page = visual.Navigation.ModalStack.LastOrDefault();
                if (page == null) {
                    // In case the element in DataTemplate, NavigationProxycan't be got.
                    // Instead of it, the page dismissal is judged by whether the BindingContext is null.
                    Element.BindingContextChanged += BindingContextChanged;
                    OnAttachedOverride();
                    return;
                }
            }

            // To call certainly a OnDetached method when the page is popped, 
            // it executes the process removing all the effects in the page at once with Attached bindable property.
            if (!GetIsRegistered(page))
            {
                SetIsRegistered(page, true);
            }

            OnAttachedOverride();
        }

        protected virtual void OnAttachedOverride() { }

        protected override void OnDetached()
        {
            if (!IsSupportedByApi)
                return;

            OnDetachedOverride();

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
            Element.BindingContextChanged -= BindingContextChanged;

            _renderer = null;
        }

        protected virtual void OnDetachedOverride() { }


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

        void BindingContextChanged(object sender, EventArgs e)
        {
            if (Element.BindingContext != null)
                return;

            // For Android, when a page is popped, OnDetached is automatically not called. (when iOS, it is called)
            // So, made the BindingContextChanged event subscribe in advance 
            // and make the effect manually removed when the BindingContext is null.
            // However, there is the problem that it isn't called when the BindingContext remains null all along.
            // The above solution is to use NavigationPage.Popped or Application.ModalPopping event.
            // That's why the following code runs only when the element is in a DataTemplate.
            if (IsAttached)
            {
                var toRemove = Element.Effects.OfType<AiRoutingEffectBase>().FirstOrDefault(x => x.EffectId == ResolveId);
                Device.BeginInvokeOnMainThread(() => Element?.Effects.Remove(toRemove));
            }
        }

        internal static readonly BindableProperty IsRegisteredProperty =
            BindableProperty.CreateAttached(
                    "IsRegistered",
                    typeof(bool),
                    typeof(AiEffectBase),
                    default(bool),
                    propertyChanged: IsRegisteredPropertyChanged
                );

        internal static void SetIsRegistered(BindableObject view, bool value)
        {
            view.SetValue(IsRegisteredProperty, value);
        }

        internal static bool GetIsRegistered(BindableObject view)
        {
            return (bool)view.GetValue(IsRegisteredProperty);
        }

        static void IsRegisteredPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bool)newValue) return;

            var page = bindable as Page;

            if (page.Parent is NavigationPage navi)
            {
                navi.Popped += NaviPopped;
            }
            else
            {
                Xamarin.Forms.Application.Current.ModalPopping += ModalPopping;
            }

            void NaviPopped(object sender, NavigationEventArgs e)
            {
                if (e.Page != page)
                    return;

                navi.Popped -= NaviPopped;

                RemoveEffects();
            }

            void ModalPopping(object sender, ModalPoppingEventArgs e)
            {
                if (e.Modal != page)
                    return;

                Xamarin.Forms.Application.Current.ModalPopping -= ModalPopping;

                RemoveEffects();
            }

            void RemoveEffects()
            {
                foreach (var child in page.Descendants())
                {
                    foreach (var effect in child.Effects.OfType<AiRoutingEffectBase>())
                    {
                        Device.BeginInvokeOnMainThread(() => child.Effects.Remove(effect));
                    }
                }

                foreach(var effect in page.Effects.OfType<AiRoutingEffectBase>())
                {
                    Device.BeginInvokeOnMainThread(() => page.Effects.Remove(effect));
                }

                page.ClearValue(IsRegisteredProperty);
            }
        }
    }
}
