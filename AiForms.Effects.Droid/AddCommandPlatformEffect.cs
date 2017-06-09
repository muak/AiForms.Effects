using System;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Linq;
using Android.Media;
using Android.Content;

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
        private bool _isSoundEffect;
        private bool _isTapTargetSoundEffect;
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
        private AudioManager _audioManager;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            if (Control is Android.Widget.ListView) {
                //ListViewはOnClickで例外を出すので除外
                return;
            }

            UpdateCommand();
            UpdateCommandParameter();
            UpdateLongCommand();
            UpdateLongCommandParameter();
            UpdateEnableRipple();
            //UpdateEffectColor();
            UpdateIsSoundEffect();
            _isTapTargetSoundEffect = TapSoundEffectElementType.Any(x => x == Element.GetType());
            if (_audioManager == null) {
                _audioManager = (AudioManager)Forms.Context.GetSystemService(Context.AudioService);
            }

            _view.Click += OnClick;
        }

        protected override void OnDetached()
        {
            var renderer = Container as IVisualElementRenderer;
            if (renderer?.Element != null) {    // Disposeされているかの判定
                _view.Click -= OnClick;
                _view.Touch -= View_Touch;
                _view.LongClick -= OnLongClick;
                if (_useRipple) {
                    RemoveRipple();
                }
            }
            _command = null;
            _commandParameter = null;
            _longCommand = null;
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
            else if (e.PropertyName == AddCommand.IsSoundEffectProperty.PropertyName) {
                UpdateIsSoundEffect();
            }
        }

        void UpdateCommand()
        {
            _command = AddCommand.GetCommand(Element);
        }

        void UpdateCommandParameter()
        {
            _commandParameter = AddCommand.GetCommandParameter(Element);
        }

        void UpdateLongCommand()
        {
            if (_longCommand != null) {
                _view.LongClick -= OnLongClick;
            }
            _longCommand = AddCommand.GetLongCommand(Element);
            if (_longCommand == null) {
                return;
            }

            _view.LongClick += OnLongClick;

        }
        void UpdateLongCommandParameter()
        {
            _longCommandParameter = AddCommand.GetLongCommandParameter(Element);
        }

        void OnClick(object sender, EventArgs e)
        {
            if (_command == null)
                return;
            
            if (_isTapTargetSoundEffect && _isSoundEffect) {
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

            if (_isSoundEffect) {
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

        void UpdateIsSoundEffect()
        {
            _isSoundEffect = AddCommand.GetIsSoundEffect(Element);
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

