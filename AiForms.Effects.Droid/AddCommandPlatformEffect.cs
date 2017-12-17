using System;
using System.Linq;
using System.Windows.Input;
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

[assembly: ResolutionGroupName("AiForms")]
[assembly: ExportEffect(typeof(AddCommandPlatformEffect), nameof(AddCommand))]
namespace AiForms.Effects.Droid
{
    public class AddCommandPlatformEffect : AiEffectBase
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
            typeof(StackLayout)
        };

        public static float DisabledAlpha = 0.3f;   //if necessary, change this value.
        private static Type[] DisableEffectTargetType = {
            typeof(ActivityIndicator),
            typeof(BoxView),
            typeof(Xamarin.Forms.Image),
            typeof(Xamarin.Forms.ProgressBar),
            typeof(ContentPresenter),
            typeof(ContentView),
            typeof(TemplatedView),
            typeof(Xamarin.Forms.AbsoluteLayout),
            typeof(Grid),
            typeof(Xamarin.Forms.RelativeLayout),
            typeof(StackLayout)
        };

        private ICommand _command;
        private object _commandParameter;
        private ICommand _longCommand;
        private object _longCommandParameter;
        private Android.Views.View _view;
        private RippleDrawable _ripple;
        private Drawable _orgDrawable;
        private bool _useRipple;
        private FrameLayout _rippleOverlay;
        private ContainerOnLayoutChangeListener _rippleListener;
        private FastRendererOnLayoutChangeListener _fastListener;
        private bool _enableSound;
        private bool _isTapTargetSoundEffect;
        private bool _isDisableEffectTarget;
        private AudioManager _audioManager;
        private bool _syncCanExecute;

        private GestureDetector _gestureDetector;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            if (Control is Android.Widget.ListView || Control is Android.Widget.ScrollView) {
                // Except ListView and ScrollView because of Raising Exception. 
                Device.BeginInvokeOnMainThread(() => AddCommand.SetOn(Element, false));
                return;
            }

            _isTapTargetSoundEffect = TapSoundEffectElementType.Any(x => x == Element.GetType());

            if (_audioManager == null) {
                _audioManager = (AudioManager)_view.Context.GetSystemService(Context.AudioService);
            }

            _gestureDetector = new GestureDetector(_view.Context, new ViewGestureListener(this));
            _gestureDetector.IsLongpressEnabled = true;

            _view.Clickable = true;
            _view.LongClickable = true;

            UpdateSyncCanExecute();
            UpdateCommandParameter();
            UpdateLongCommandParameter();
            UpdateEnableSound();

            _view.Touch += _view_Touch;

            UpdateEnableRipple();


        }

        protected override void OnDetached()
        {
            System.Diagnostics.Debug.WriteLine(Element.GetType().FullName);
            if (!IsDisposed) {
                if (_useRipple) {
                    RemoveRipple();
                }
                if (_rippleOverlay != null) {
                    _rippleOverlay.Touch -= _view_Touch;
                    _rippleOverlay?.Dispose();
                }
                _view.Touch -= _view_Touch;
            }

            if (_command != null) {
                _command.CanExecuteChanged -= CommandCanExecuteChanged;
                _command = null;
            }
            if (_longCommand != null) {
                _longCommand.CanExecuteChanged -= CommandCanExecuteChanged;
                _longCommand = null;
            }
            _commandParameter = null;
            _longCommandParameter = null;
            _orgDrawable = null;
            _view = null;

            _rippleListener?.Dispose();
            _rippleListener = null;

            _rippleOverlay = null;

            _ripple?.Dispose();
            _ripple = null;
            _useRipple = false;

            _gestureDetector?.Dispose();
            _gestureDetector = null;

            _fastListener?.Dispose();
            _fastListener = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (IsDisposed) {
                return;
            }

            if (e.PropertyName == AddCommand.CommandProperty.PropertyName) {
                UpdateCommand();
            }
            else if (e.PropertyName == AddCommand.CommandParameterProperty.PropertyName) {
                UpdateCommandParameter();
            }
            else if (e.PropertyName == AddCommand.EffectColorProperty.PropertyName) {
                UpdateEffectColor();
            }
            else if (e.PropertyName == AddCommand.LongCommandProperty.PropertyName) {
                UpdateLongCommand();
            }
            else if (e.PropertyName == AddCommand.LongCommandParameterProperty.PropertyName) {
                UpdateLongCommandParameter();
            }
            else if (e.PropertyName == AddCommand.EnableRippleProperty.PropertyName) {
                UpdateEnableRipple();
            }
            else if (e.PropertyName == AddCommand.EnableSoundProperty.PropertyName) {
                UpdateEnableSound();
            }
            else if (e.PropertyName == AddCommand.SyncCanExecuteProperty.PropertyName) {
                UpdateSyncCanExecute();
            }
        }

        void _view_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            _gestureDetector.OnTouchEvent(e.Event);
            // for any reason depending type of element, Handled value must be changed.
            // I don't know the reason.
            if (!_useRipple && !IsClickable) {
                e.Handled = true;
                return;
            }
            e.Handled = false;
        }

        void UpdateSyncCanExecute()
        {
            _syncCanExecute = AddCommand.GetSyncCanExecute(Element);
            if (_syncCanExecute) {
                _isDisableEffectTarget = DisableEffectCheck();
            }
            UpdateCommand();
            UpdateLongCommand();
        }

        bool DisableEffectCheck()
        {
            if (Element is Label) {
                //when it was setted TextColor or BackgroundColor,it will not be turn disabled color.
                var label = Element as Label;
                return label.TextColor != Xamarin.Forms.Color.Default ||
                            label.BackgroundColor != Xamarin.Forms.Color.Default;
            }

            return DisableEffectTargetType.Any(x => x == Element.GetType());
        }

        void UpdateCommand()
        {
            if (_command != null) {
                _command.CanExecuteChanged -= CommandCanExecuteChanged;
            }

            _command = AddCommand.GetCommand(Element);

            if (_command != null && _syncCanExecute) {
                _command.CanExecuteChanged += CommandCanExecuteChanged;
                CommandCanExecuteChanged(_command, System.EventArgs.Empty);
            }
        }

        void CommandCanExecuteChanged(object sender, System.EventArgs e)
        {
            var forms = Element as Xamarin.Forms.View;
            if (forms == null)
                return;

            if (JudgeDisabled()) {
                //Entrust the process of disabled to Forms
                forms.IsEnabled = false;
                if (_isDisableEffectTarget) {
                    forms.FadeTo(DisabledAlpha);
                }
                if (IsFastRenderer) {
                    _view.Enabled = false;
                }
            }
            else {
                forms.IsEnabled = true;
                if (_isDisableEffectTarget) {
                    forms.FadeTo(1f);
                }
                if (IsFastRenderer) {
                    _view.Enabled = true;
                }
            }
        }

        bool JudgeDisabled()
        {

            if (_command != null && _longCommand == null) {
                return !_command.CanExecute(_commandParameter);
            }
            else if (_command == null && _longCommand != null) {
                return !_longCommand.CanExecute(_longCommandParameter);
            }
            else if (_command == null && _longCommand == null) {
                return false;
            }
            else {
                // only when both Command and LongCommand cannot execute,do disabled.
                return !_command.CanExecute(_commandParameter) && !_longCommand.CanExecute(_longCommandParameter);
            }
        }

        void UpdateCommandParameter()
        {
            _commandParameter = AddCommand.GetCommandParameter(Element);
        }

        void UpdateLongCommand()
        {
            if (_longCommand != null) {
                _longCommand.CanExecuteChanged -= CommandCanExecuteChanged;
            }

            _longCommand = AddCommand.GetLongCommand(Element);

            if (_longCommand == null) {
                return;
            }

            if (_syncCanExecute) {
                _longCommand.CanExecuteChanged += CommandCanExecuteChanged;
                CommandCanExecuteChanged(_longCommand, System.EventArgs.Empty);
            }

        }
        void UpdateLongCommandParameter()
        {
            _longCommandParameter = AddCommand.GetLongCommandParameter(Element);
        }

        void UpdateEffectColor()
        {
            var color = AddCommand.GetEffectColor(Element);
            if (color == Xamarin.Forms.Color.Default) {
                return;
            }

            if (_useRipple) {
                _ripple?.SetColor(getPressedColorSelector(color.ToAndroid()));
            }

        }

        void UpdateEnableRipple()
        {
            _useRipple = AddCommand.GetEnableRipple(Element);

            var color = AddCommand.GetEffectColor(Element);
            if (color == Xamarin.Forms.Color.Default) {
                return;
            }

            if (_useRipple) {
                AddRipple();
            }
            else {
                RemoveRipple();
            }

            UpdateEffectColor();
        }

        void UpdateEnableSound()
        {
            _enableSound = AddCommand.GetEnableSound(Element);
        }

        void AddRipple()
        {
            if (_ripple != null) {
                return;
            }

            if (Element is Layout || Element is BoxView) {
                _rippleOverlay = new FrameLayout(_view.Context);
                _rippleOverlay.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);

                _rippleListener = new ContainerOnLayoutChangeListener(_rippleOverlay);
                _view.AddOnLayoutChangeListener(_rippleListener);

                (_view as ViewGroup).AddView(_rippleOverlay);

                _rippleOverlay.BringToFront();

                _rippleOverlay.Foreground = CreateRipple(Color.Accent.ToAndroid());
                _rippleOverlay.Clickable = true;
                _rippleOverlay.LongClickable = true;

                _view.Touch -= _view_Touch;
                _rippleOverlay.Touch += _view_Touch;
            }
            else if (IsFastRenderer) {
                if (_fastListener == null) {
                    _fastListener = new FastRendererOnLayoutChangeListener(this);
                    _view.AddOnLayoutChangeListener(_fastListener);
                    _view.RequestLayout();
                    return;
                }
                _view.Foreground = CreateRipple(Color.Accent.ToAndroid());
                _view.Touch += _view_Touch;
            }
            else {
                _orgDrawable = _view.Background;
                _view.Background = CreateRipple(Color.Accent.ToAndroid());
            }
        }

        void RemoveRipple()
        {
            if (_ripple == null) {
                return;
            }

            if (Element is Layout || Element is BoxView) {
                _view.Touch += _view_Touch;
                _rippleOverlay.Touch -= _view_Touch;

                var viewgrp = _view as ViewGroup;

                viewgrp.RemoveOnLayoutChangeListener(_rippleListener);
                _rippleListener.Dispose();

                viewgrp.RemoveView(_rippleOverlay);
                _rippleOverlay.Dispose();

                _rippleOverlay = null;
            }
            else if (IsFastRenderer) {
                _view.Touch -= _view_Touch;
                Control.RemoveOnLayoutChangeListener(_fastListener);
                _view = Control;
                _fastListener.CleanUp();
                _fastListener.Dispose();
                _fastListener = null;
                _view.Touch += _view_Touch;
            }
            else {
                _view.Background = _orgDrawable;
                _orgDrawable = null;
            }
            _ripple?.Dispose();
            _ripple = null;
        }


        RippleDrawable CreateRipple(Android.Graphics.Color color)
        {
            if (Element is Layout || Element is BoxView) {
                var mask = new ColorDrawable(Android.Graphics.Color.White);
                return _ripple = new RippleDrawable(getPressedColorSelector(color), null, mask);
            }

            var back = _view.Background;
            if (back == null) {
                var mask = new ColorDrawable(Android.Graphics.Color.White);
                return _ripple = new RippleDrawable(getPressedColorSelector(color), null, mask);
            }
            else if (back is RippleDrawable) {
                _ripple = back.GetConstantState().NewDrawable() as RippleDrawable;
                _ripple.SetColor(getPressedColorSelector(color));

                return _ripple;
            }
            else {
                return _ripple = new RippleDrawable(getPressedColorSelector(color), back, null);
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

        class ViewGestureListener : Java.Lang.Object, GestureDetector.IOnGestureListener
        {
            AddCommandPlatformEffect _effect;
            public ViewGestureListener(AddCommandPlatformEffect effect)
            {
                _effect = effect;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) {
                    _effect = null;
                }
                base.Dispose(disposing);
            }

            public bool OnDown(MotionEvent e)
            {
                return false;
            }

            public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                return false;
            }

            public void OnLongPress(MotionEvent e)
            {
                if (_effect._longCommand == null) {
                    return;
                }

                if (_effect._longCommand == null)
                    return;

                if (!_effect._longCommand.CanExecute(_effect._longCommandParameter))
                    return;

                if (_effect._enableSound) {
                    _effect._audioManager?.PlaySoundEffect(PlaySoundEffect);
                }

                _effect._longCommand.Execute(_effect._longCommandParameter ?? _effect.Element);
            }

            public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
            {
                return false;
            }

            public void OnShowPress(MotionEvent e) { }

            public bool OnSingleTapUp(MotionEvent e)
            {
                if (_effect._command == null)
                    return false;

                if (!_effect._command.CanExecute(_effect._commandParameter))
                    return false;

                if (_effect._isTapTargetSoundEffect && _effect._enableSound) {
                    _effect._audioManager?.PlaySoundEffect(PlaySoundEffect);
                }

                _effect._command.Execute(_effect._commandParameter ?? _effect.Element);

                return false;
            }


        }

        internal class FastRendererOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
        {
            bool _alreadyGotParent = false;
            AddCommandPlatformEffect _effect;
            Android.Views.ViewGroup _parent;
            FrameLayout _overlay;

            //public FastRendererOnLayoutChangeListener(IntPtr handle, JniHandleOwnership transfer):base(handle, transfer)
            //{

            //}

            public FastRendererOnLayoutChangeListener(AddCommandPlatformEffect effect)
            {
                _effect = effect;
                _overlay = new FrameLayout(_effect._view.Context);
                _overlay.Clickable = true;
                _overlay.LongClickable = true;
            }

            // Because FastRenderer of Label or Image can't be set ClickListener, 
            // insert FrameLayout with same position and same size on the view.
            public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
            {
                _overlay.Layout(v.Left, v.Top, v.Right, v.Bottom);

                if (_alreadyGotParent) {
                    return;
                }

                _parent = _effect.Control.Parent as Android.Views.ViewGroup;
                _alreadyGotParent = true;

                _parent.AddView(_overlay);

                _overlay.BringToFront();

                _effect._view.Touch -= _effect._view_Touch;
                _effect._view = _overlay;
                _effect.UpdateEnableRipple();
            }

            public void CleanUp()
            {
                _parent.RemoveView(_overlay);
                _overlay.Dispose();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) {
                    _effect = null;
                    _parent = null;
                    _overlay = null;
                }
                base.Dispose(disposing);
            }
        }
    }


    internal class ContainerOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
    {
        private Android.Widget.FrameLayout _layout;

        public ContainerOnLayoutChangeListener(Android.Widget.FrameLayout layout)
        {
            _layout = layout;
        }

        //have to decide children size when OnLayoutChange.
        public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
        {
            _layout.Right = v.Width;
            _layout.Bottom = v.Height;
        }
    }


}

