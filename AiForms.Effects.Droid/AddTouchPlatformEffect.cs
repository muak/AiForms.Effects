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
    public class AddTouchPlatformEffect:PlatformEffect
    {
        WeakReference<Android.Views.View> _viewRef;
        TouchRecognizer _recognizer;
        Context _context;

        protected override void OnAttached()
        {
            _viewRef = new WeakReference<Android.Views.View>(Control ?? Container);


            if (Control is Android.Widget.ListView || Control is Android.Widget.ScrollView)
            {
                // Except ListView and ScrollView because of Raising Exception. 
                Device.BeginInvokeOnMainThread(() => AddTouch.SetOn(Element, false));
                return;
            }

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


        protected override void OnDetached()
        {
            _context = null;
            _recognizer = null;
            _viewRef = null;
        }

    }
}
