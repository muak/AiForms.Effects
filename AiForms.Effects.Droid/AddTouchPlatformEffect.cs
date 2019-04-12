using System;
using Android.Content;
using Android.Views;
using AiForms.Effects.Droid;
using AiForms.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(AddTouchPlatformEffect), nameof(AddTouch))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AddTouchPlatformEffect:AiEffectBase
    {
        WeakReference<Android.Views.View> _viewRef;
        TouchRecognizer _recognizer;
        Context _context;

        protected override void OnAttachedOverride()
        {
            _viewRef = new WeakReference<Android.Views.View>(Control ?? Container);

            _recognizer = AddTouch.GetRecognizer(Element);

            if(_viewRef.TryGetTarget(out var view)){
                _context = view.Context;
                view.Touch += _view_Touch;
            }           
        }

        void _view_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            var point = new Point(
                    _context.FromPixels(e.Event.GetX()),
                    _context.FromPixels(e.Event.GetY())
            );

            switch(e.Event.Action){
                
                case MotionEventActions.Down:
                    _recognizer.OnTouchBegin(new TouchEventArgs { Location = point });
                    break;
                case MotionEventActions.Move:
                    _recognizer.OnTouchMove(new TouchEventArgs { Location = point });
                    break;
                case MotionEventActions.Up :
                    _recognizer.OnTouchEnd(new TouchEventArgs { Location = point });
                    break;
                case MotionEventActions.Cancel:
                    _recognizer.OnTouchCancel(new TouchEventArgs { Location = point });
                    break;
            }
        }


        protected override void OnDetachedOverride()
        {
            if (!IsDisposed)
            {
                if (_viewRef.TryGetTarget(out var view))
                {
                    view.Touch -= _view_Touch;
                }
                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }
            Element.ClearValue(AddTouch.RecognizerProperty);
            _context = null;
            _recognizer = null;
            _viewRef = null;
            System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached completely");
        }

    }
}
