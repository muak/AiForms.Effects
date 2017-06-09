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

    public class AddCommandPlatformEffect : PlatformEffect
    {
        private ICommand _command;
        private object _commandParameter;
        private ICommand _longCommand;
        private object _longCommandParameter;
        private Android.Views.View _view;
        private FrameLayout _layer;
        private RippleDrawable _ripple;
        private Drawable _orgDrawable;
        private bool _useRipple;
        private FrameLayout _rippleOverlay;
        private ContainerOnLayoutChangeListener _rippleListener;
        private bool _enableSound;
        private bool _isTapTargetSoundEffect;
        private static Type[] TapSoundEffectElementType = {
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
        private bool _isDisableEffectTarget;
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

        private AudioManager _audioManager;
        private bool _syncCanExecute;

        private readonly float _disabledAlpha = 0.3f;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            if (Control is Android.Widget.ListView) {
                //Except ListView because of Raising Exception OnClick
                return;
            }

            _isTapTargetSoundEffect = TapSoundEffectElementType.Any(x => x == Element.GetType());

            UpdateSyncCanExecute();
            UpdateLongCommand();
            UpdateLongCommandParameter();
            UpdateEnableRipple();
            UpdateEnableSound();

            if (_audioManager == null) {
                _audioManager = (AudioManager)Forms.Context.GetSystemService(Context.AudioService);
            }

            _view.Click += OnClick;
        }

        protected override void OnDetached()
        {
            var renderer = Container as IVisualElementRenderer;
            if (renderer?.Element != null) {    // Check disposed
                _view.Click -= OnClick;
                _view.Touch -= View_Touch;
                _view.LongClick -= OnLongClick;
                if (_useRipple) {
                    RemoveRipple();
                }
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
            _rippleOverlay?.Dispose();
            _rippleOverlay = null;
            _layer?.Dispose();
            _layer = null;
            _ripple?.Dispose();
            _ripple = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

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
                //when it was setted TextColor or BackgroundColor,it will not be turn color of disabled.
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
                    forms.FadeTo(_disabledAlpha);
                }
            }
            else {
                forms.IsEnabled = true;
                if (_isDisableEffectTarget) {
                    forms.FadeTo(1f);
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
                _view.LongClick -= OnLongClick;
            }

            _longCommand = AddCommand.GetLongCommand(Element);

            if (_longCommand == null) {
                return;
            }

            _view.LongClick += OnLongClick;

            if (_syncCanExecute) {
                _longCommand.CanExecuteChanged += CommandCanExecuteChanged;
                CommandCanExecuteChanged(_longCommand, System.EventArgs.Empty);
            }

        }
        void UpdateLongCommandParameter()
        {
            _longCommandParameter = AddCommand.GetLongCommandParameter(Element);
        }

        void OnClick(object sender, EventArgs e)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(_commandParameter))
                return;

            if (_isTapTargetSoundEffect && _enableSound) {
                _audioManager?.PlaySoundEffect(SoundEffect.KeyClick);
            }

            _command.Execute(_commandParameter ?? Element);
        }

        void OnLongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            if (_longCommand == null) {
                e.Handled = false;
                return;
            }

            if (_longCommand == null)
                return;

            if (!_longCommand.CanExecute(_longCommandParameter))
                return;

            if (_enableSound) {
                _audioManager?.PlaySoundEffect(SoundEffect.KeyClick);
            }

            _longCommand.Execute(_longCommandParameter ?? Element);

            e.Handled = true;
        }

        void UpdateEffectColor()
        {

            _view.Touch -= View_Touch;
            if (_layer != null) {
                _layer.Dispose();
                _layer = null;
            }
            var color = AddCommand.GetEffectColor(Element);
            if (color == Xamarin.Forms.Color.Default) {
                return;
            }

            if (_useRipple) {
                _ripple.SetColor(getPressedColorSelector(color.ToAndroid()));
            }
            else {
                _layer = new FrameLayout(Container.Context);
                _layer.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);
                _layer.SetBackgroundColor(color.ToAndroid());
                _view.Touch += View_Touch;
            }
        }

        void UpdateEnableRipple()
        {
            var oldValue = _useRipple;
            var newValue = AddCommand.GetEnableRipple(Element);
            _useRipple = newValue;
            if (newValue == oldValue) {
                return;
            }

            var color = AddCommand.GetEffectColor(Element);
            if (color == Xamarin.Forms.Color.Default) {
                return;
            }

            if (!oldValue && newValue) {
                AddRipple();
            }
            if (oldValue && !newValue) {
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
            if (Element is Layout) {
                _rippleOverlay = new FrameLayout(Container.Context);
                _rippleOverlay.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);

                _rippleListener = new ContainerOnLayoutChangeListener(_rippleOverlay);
                _view.AddOnLayoutChangeListener(_rippleListener);

                (_view as ViewGroup).AddView(_rippleOverlay);

                _rippleOverlay.BringToFront();

                _rippleOverlay.Foreground = CreateRipple(Color.Accent.ToAndroid());
            }
            else {
                _orgDrawable = _view.Background;
                _view.Background = CreateRipple(Color.Accent.ToAndroid());
            }
        }

        void RemoveRipple()
        {
            if (Element is Layout) {
                var viewgrp = _view as ViewGroup;

                viewgrp.RemoveOnLayoutChangeListener(_rippleListener);
                _rippleListener.Dispose();

                viewgrp.RemoveView(_rippleOverlay);
                _rippleOverlay.Dispose();

                _rippleOverlay = null;
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
            if (Element is Layout) {
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

        void View_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down) {
                Container.AddView(_layer);
                _layer.Top = 0;
                _layer.Left = 0;
                _layer.Right = _view.Width;
                _layer.Bottom = _view.Height;
                _layer.BringToFront();
            }
            if (e.Event.Action == MotionEventActions.Up || e.Event.Action == MotionEventActions.Cancel) {
                Container.RemoveView(_layer);
            }

            e.Handled = false;
        }
    }

    internal class ContainerOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
    {
        private Android.Widget.FrameLayout _layout;

        public ContainerOnLayoutChangeListener(Android.Widget.FrameLayout layout)
        {
            _layout = layout;
        }

        //ContainerにAddViewした子要素のサイズを確定する必要があるため
        //ContainerのOnLayoutChangeのタイミングでセットする
        public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
        {
            _layout.Right = v.Width;
            _layout.Bottom = v.Height;
        }
    }
}

