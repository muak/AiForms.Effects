using System;
using System.Linq;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Media;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(FeedbackPlatformEffect), nameof(Feedback))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class FeedbackPlatformEffect:AiEffectBase
    {
        public static SoundEffect PlaySoundEffect = SoundEffect.KeyClick;
        public static Type[] TapSoundEffectElementType = {
            typeof(ContentPresenter),
            typeof(ContentView),
            typeof(Frame),
            typeof(Cell),
            typeof(Xamarin.Forms.ScrollView),
            typeof(TemplatedView),
            typeof(Xamarin.Forms.AbsoluteLayout),
            typeof(Grid),
            typeof(Xamarin.Forms.RelativeLayout),
            typeof(StackLayout),
            typeof(Xamarin.Forms.Button),
            typeof(Xamarin.Forms.Image),
            typeof(Xamarin.Forms.BoxView),
            typeof(Xamarin.Forms.Label),
        };

        private Android.Views.View _view;
        private RippleDrawable _ripple;
        private Drawable _orgDrawable;
        private FrameLayout _rippleOverlay;
        private FastRendererOnLayoutChangeListener _fastListener;
        private bool _enableSound;
        private bool _isTapTargetSoundEffect;
        private AudioManager _audioManager;


        protected override void OnAttachedOverride()
        {
            _view = Control ?? Container;

            _isTapTargetSoundEffect = TapSoundEffectElementType.Any(x => x == Element.GetType());

            if (_audioManager == null)
            {
                _audioManager = (AudioManager)_view.Context.GetSystemService(Context.AudioService);
            }

            _view.Clickable = true;
            _view.LongClickable = true;

            SetUpRipple();

            UpdateEnableSound();

            if(IsClickable)
            {
                _view.Touch += OnViewTouch;
            }
            
            UpdateEffectColor();
            UpdateIsEnabled();
        }

        protected override void OnDetachedOverride()
        {
            if (!IsDisposed)
            {
                if (!IsClickable)
                {
                    _view.Touch -= OnOverlayTouch;
                    _view.RemoveOnLayoutChangeListener(_fastListener);
                    // If a NavigationPage is used and the following code is enabled, a null exception occurs when a NavigationPageRenderer is disposed of.
                    // So this code is disabled.
                    //_rippleOverlay.RemoveFromParent();
                    _fastListener.Dispose();
                    _fastListener = null;
                    _rippleOverlay.Dispose();
                    _rippleOverlay = null;
                }
                else
                {
                    _view.Touch -= OnViewTouch;
                    _view.Background = _orgDrawable;
                    _orgDrawable = null;
                }
            }

            _ripple?.Dispose();
            _ripple = null;

            _audioManager = null;
            _view = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (!IsSupportedByApi)
                return;

            if (IsDisposed)
            {
                return;
            }

            if (e.PropertyName == Feedback.EffectColorProperty.PropertyName)
            {
                UpdateEffectColor();
            }
            else if (e.PropertyName == Feedback.EnableSoundProperty.PropertyName)
            {
                UpdateEnableSound();
            }
            else if(e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            {
                UpdateIsEnabled();
            }
        }

        void OnViewTouch(object sender, Android.Views.View.TouchEventArgs e)
        {
            if(e.Event.Action == MotionEventActions.Down)
            {
                PlaySound();
            }

            e.Handled = false;
        }

        void OnOverlayTouch(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                PlaySound();
            }

            _view?.DispatchTouchEvent(e.Event);

            e.Handled = false;
        }

        void PlaySound()
        {
            if (_isTapTargetSoundEffect && _enableSound)
            {
                _audioManager?.PlaySoundEffect(PlaySoundEffect);
            }
        }

        protected virtual Color GetEffectColor()
        {
            return Feedback.GetEffectColor(Element);
        }

        protected virtual bool GetEnableSound()
        {
            return Feedback.GetEnableSound(Element);
        }

        void UpdateEffectColor()
        {
            var color = GetEffectColor();
            _ripple?.SetColor(getPressedColorSelector(color.ToAndroid()));
        }

        void UpdateEnableSound()
        {
            _enableSound = GetEnableSound();
        }

        protected void UpdateIsEnabled()
        {
            if(!IsClickable)
            {
                _rippleOverlay.Enabled = (Element as VisualElement).IsEnabled;
            }
        }

        void SetUpRipple()
        {
            _ripple = CreateRipple(Android.Graphics.Color.Transparent);

            if (!IsClickable)
            {
                _rippleOverlay = new FrameLayout(_view.Context)
                {
                    Clickable = true,
                    LongClickable = true,
                    Foreground = _ripple
                };
                _fastListener = new FastRendererOnLayoutChangeListener(this);
                _view.AddOnLayoutChangeListener(_fastListener);
                _view.RequestLayout();
            }
            else
            {
                _orgDrawable = _view.Background;
                _view.Background = _ripple;
            }
        }

        void SetUpOverlay()
        {
            var parent = _view.Parent as Android.Views.ViewGroup;

            parent.AddView(_rippleOverlay);

            _rippleOverlay.BringToFront();
            _rippleOverlay.Touch += OnOverlayTouch;
        }


        RippleDrawable CreateRipple(Android.Graphics.Color color)
        {
            if (!IsClickable)
            {
                var mask = new ColorDrawable(Android.Graphics.Color.White);
                return new RippleDrawable(getPressedColorSelector(color), null, mask);
            }

            var back = _view.Background;
            if (back == null)
            {
                var mask = new ColorDrawable(Android.Graphics.Color.White);
                return new RippleDrawable(getPressedColorSelector(color), null, mask);
            }
            else
            {
                return new RippleDrawable(getPressedColorSelector(color), back, null);
            }
        }

        ColorStateList getPressedColorSelector(int pressedColor)
        {
            return new ColorStateList(
                new int[][]
                {
                    new int[]{}
                },
                new int[]
                {
                    pressedColor,
                });
        }

        internal class FastRendererOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
        {
            bool _alreadyGotParent = false;
            FeedbackPlatformEffect _effect;

            public FastRendererOnLayoutChangeListener(FeedbackPlatformEffect effect)
            {
                _effect = effect;
            }

            // Because FastRenderer of Label or Image can't be set ClickListener, 
            // insert FrameLayout with same position and same size on the view.
            public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
            {
                _effect._rippleOverlay.Layout(v.Left, v.Top, v.Right, v.Bottom);

                if (_alreadyGotParent)
                {
                    return;
                }

                _alreadyGotParent = true;

                // FastRenderer Control's parent can be got at only this timing.
                _effect.SetUpOverlay();
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
