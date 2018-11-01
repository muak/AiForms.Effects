using System;
using AiForms.Effects;
using AiForms.Effects.iOS;
using AudioToolbox;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Linq;

[assembly: ExportEffect(typeof(FeedbackPlatformEffect), nameof(Feedback))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class FeedbackPlatformEffect:PlatformEffect
    {
        public static uint PlaySoundNo = 1306;

        TouchRecognizer _toucheController;
        TouchEffectGestureRecognizer _touchRecognizer;
        UIView _view;
        UIView _layer;
        bool _enableSound;
        SystemSound _clickSound;
        float _alpha;
        VisualElement visualElement => Element as VisualElement;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            _view.UserInteractionEnabled = true;

            _layer = new UIView { 
                Alpha = 0,
                Opaque = false,
                UserInteractionEnabled = false
            };
            _view.AddSubview(_layer);

            _layer.TranslatesAutoresizingMaskIntoConstraints = false;

            _layer.TopAnchor.ConstraintEqualTo(_view.TopAnchor).Active = true;
            _layer.LeftAnchor.ConstraintEqualTo(_view.LeftAnchor).Active = true;
            _layer.BottomAnchor.ConstraintEqualTo(_view.BottomAnchor).Active = true;
            _layer.RightAnchor.ConstraintEqualTo(_view.RightAnchor).Active = true;


            _view.BringSubviewToFront(_layer);

            _toucheController = new TouchRecognizer();
            _touchRecognizer = new TouchEffectGestureRecognizer(_toucheController);
            _touchRecognizer.Delegate = new AlwaysSimultaneouslyGestureRecognizerDelegate();

            _view.AddGestureRecognizer(_touchRecognizer);

            _toucheController.TouchBegin += OnTouchBegin;
            _toucheController.TouchEnd += OnTouchEnd;
            _toucheController.TouchCancel += OnTouchEnd;

            UpdateEffectColor();
            UpdateEnableSound();
        }

        protected override void OnDetached()
        {
            _toucheController.TouchBegin -= OnTouchBegin;
            _toucheController.TouchEnd -= OnTouchEnd;
            _toucheController.TouchCancel -= OnTouchEnd;

            _view.RemoveGestureRecognizer(_touchRecognizer);
            _touchRecognizer.Delegate?.Dispose();
            _touchRecognizer.Delegate = null;
            _touchRecognizer.Dispose();

            _touchRecognizer = null;
            _toucheController = null;

            _layer.RemoveFromSuperview();
            _layer.Dispose();
            _layer = null;

            _clickSound?.Dispose();
            _clickSound = null;

            _view = null;

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == Feedback.EffectColorProperty.PropertyName)
            {
                UpdateEffectColor();
            }
            else if (e.PropertyName == Feedback.EnableSoundProperty.PropertyName)
            {
                UpdateEnableSound();
            }
        }

        async void OnTouchBegin(object sender, TouchEventArgs e)
        {
            if (!visualElement.IsEnabled) return;

            // if it is a variety of Picker, make it fire.
            _view.Subviews.FirstOrDefault(x => x is NoCaretField)?.BecomeFirstResponder();

            _view.BecomeFirstResponder();

            if (_enableSound)
            {
                PlayClickSound();
            }

            await UIView.AnimateAsync(0.5, () =>
            {
                _layer.Alpha = _alpha;
            });
        }

        async void OnTouchEnd(object sender, TouchEventArgs e)
        {
            if (!visualElement.IsEnabled) return;

            await UIView.AnimateAsync(0.5, () =>
            {
                _layer.Alpha = 0;
            });
        }

        void UpdateEffectColor()
        {
            var color = GetEffectColor();

            _alpha = color.A < 1.0f ? 1f : 0.3f;
            _layer.BackgroundColor = color.ToUIColor();
        }

        void UpdateEnableSound()
        {
            _enableSound = GetEnableSound();
        }

        protected virtual Color GetEffectColor()
        {
            return Feedback.GetEffectColor(Element);
        }

        protected virtual bool GetEnableSound()
        {
            return Feedback.GetEnableSound(Element);
        }

        void PlayClickSound()
        {
            if (_clickSound == null)
                _clickSound = new SystemSound(PlaySoundNo);

            _clickSound.PlaySystemSoundAsync();
        }

    }

    public class AlwaysSimultaneouslyGestureRecognizerDelegate:UIGestureRecognizerDelegate
    {
        public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            // always recognize simultaneously.
            return true;
        }
    }
}
