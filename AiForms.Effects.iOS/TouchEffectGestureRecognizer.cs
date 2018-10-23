using UIKit;
using Xamarin.Forms;

namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class TouchEffectGestureRecognizer : UIGestureRecognizer
    {

        TouchRecognizer _recognizer;

        public TouchEffectGestureRecognizer(TouchRecognizer recognizer)
        {
            _recognizer = recognizer;
        }


        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            _recognizer.OnTouchBegin(CreateTouchEventArgs());

            this.State = UIGestureRecognizerState.Began;
        }

        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            _recognizer.OnTouchMove(CreateTouchEventArgs());

            this.State = UIGestureRecognizerState.Changed;
        }

        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            _recognizer.OnTouchEnd(CreateTouchEventArgs());

            this.State = UIGestureRecognizerState.Ended;
        }

        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            _recognizer.OnTouchCancel(CreateTouchEventArgs());

            this.State = UIGestureRecognizerState.Cancelled;
        }

        TouchEventArgs CreateTouchEventArgs()
        {
            var location = LocationOfTouch(0, this.View);
            return new TouchEventArgs
            {
                Location = new Point(location.X, location.Y)
            };
        }
    }
}
