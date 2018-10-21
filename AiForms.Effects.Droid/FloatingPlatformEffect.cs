using System;
using XF = Xamarin.Forms;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using System.Runtime.InteropServices;
using AiForms.Effects.Droid;
using AiForms.Effects;

[assembly: XF.ExportEffect(typeof(FloatingPlatformEffect), nameof(Floating))]
namespace AiForms.Effects.Droid
{
    public class FloatingPlatformEffect : AiEffectBase
    {
        ViewGroup _nativePage;
        View _layoutView;
        Action OnceInitializeAction;
        XF.Page _page;
        XF.View _formsLayout;
        IVisualElementRenderer _renderer;
        SizeToFitOnLayoutChangeListener _onLayoutListener;

        public FloatingPlatformEffect()
        {

        }

        protected override void OnAttached()
        {
            if (!(Element is XF.Page)) return;

            base.OnAttached();

            _onLayoutListener = new SizeToFitOnLayoutChangeListener(this);
            Container.AddOnLayoutChangeListener(_onLayoutListener);

            OnceInitializeAction = Initialize;

            _page = Element as XF.Page;
            _page.SizeChanged += _page_SizeChanged;
        }

        protected override void OnDetached()
        {

        }

        void _page_SizeChanged(object sender, EventArgs e)
        {
            OnceInitializeAction?.Invoke();

            //var rect = new XF.Rectangle(
            //    0,
            //    0,
            //    Container.Context.FromPixels(_nativePage.Width),
            //    Container.Context.FromPixels(_nativePage.Height)
            //);
            _formsLayout.Layout(_page.Bounds);
        }

        void Initialize()
        {
            OnceInitializeAction = null;

            _nativePage = Container;

            _formsLayout = Floating.GetContent(Element);
            //_formsLayout.InputTransparent = true;

            _formsLayout.Parent = Element;

            _renderer = Platform.GetRenderer(_formsLayout);
            if (_renderer == null)
            {
                _renderer = Platform.CreateRendererWithContext(_formsLayout,Container.Context);
            }

            _layoutView = _renderer.View;

            _nativePage.AddView(_layoutView);

            _layoutView.Background = null;
        }

        internal class SizeToFitOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
        {
            FloatingPlatformEffect _effect;

            public SizeToFitOnLayoutChangeListener(FloatingPlatformEffect effect)
            {
                _effect = effect;
            }

            public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
            {
                _effect._layoutView.Layout(v.Left, v.Top, v.Right, v.Bottom);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _effect = null;
                }
                base.Dispose(disposing);
            }
        }
    }


}
