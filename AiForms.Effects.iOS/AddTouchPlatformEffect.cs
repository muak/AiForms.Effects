using AiForms.Effects;
using AiForms.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(AddTouchPlatformEffect), nameof(AddTouch))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
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
            Element.ClearValue(AddTouch.RecognizerProperty);
            _view?.RemoveGestureRecognizer(_recognizer);
            _view = null;
            _recognizer?.Dispose();
            _recognizer = null;

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }
    }
}
