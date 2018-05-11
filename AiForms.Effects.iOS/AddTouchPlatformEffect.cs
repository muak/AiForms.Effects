using AiForms.Effects;
using AiForms.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(AddTouchPlatformEffect), nameof(AddTouch))]
namespace AiForms.Effects.iOS
{
    public class AddTouchPlatformEffect : PlatformEffect
    {
        UIView _view;
        TouchEffectGestureRecognizer _recognizer;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            _recognizer = new TouchEffectGestureRecognizer(AddTouch.GetRecognizer(Element));

            _view.AddGestureRecognizer(_recognizer);
        }

        protected override void OnDetached()
        {
            _view?.RemoveGestureRecognizer(_recognizer);
            _view = null;
            _recognizer?.Dispose();
            _recognizer = null;
        }
    }
}
