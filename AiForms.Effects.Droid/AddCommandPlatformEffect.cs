using System;
using System.Linq;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Content.Res;

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
            UpdateEffectColor();

            _view.Click += OnClick;
        }

        protected override void OnDetached()
        {
            var renderer = Container as IVisualElementRenderer;
            if (renderer?.Element != null) {    // Disposeされているかの判定
                _view.Click -= OnClick;
                _view.Touch -= View_Touch;
                _view.LongClick -= OnLongClick;
            }
            _command = null;
            _commandParameter = null;
            _longCommand = null;
            _longCommandParameter = null;
            _view = null;

            if (_layer != null) {
                _layer.Dispose();
                _layer = null;
            }
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
            else if (e.PropertyName == AddCommand.EffectColorProperty.PropertyName ||
                     e.PropertyName == AddCommand.EnableRippleProperty.PropertyName) {
                UpdateEffectColor();
            }
            else if (e.PropertyName == AddCommand.LongCommandProperty.PropertyName) {
                UpdateLongCommand();
            }
            else if (e.PropertyName == AddCommand.LongCommandParameterProperty.PropertyName) {
                UpdateLongCommandParameter();
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
            _command?.Execute(_commandParameter ?? Element);
        }

        void OnLongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            if (_longCommand == null) {
                e.Handled = false;
                return;
            }

            _longCommand?.Execute(_longCommandParameter ?? Element);

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
            var useRipple = AddCommand.GetEnableRipple(Element);

            if (useRipple) {
                _view.Background = CreateRipple(color.ToAndroid());
            }
            else {
                _layer = new FrameLayout(Container.Context);
                _layer.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);
                _layer.SetBackgroundColor(color.ToAndroid());
                _view.Touch += View_Touch;
            }
        }

        RippleDrawable CreateRipple(Android.Graphics.Color color)
        {
            var back = _view.Background;
            if (back == null) {
                var mask = new ColorDrawable(Android.Graphics.Color.White);
                return new RippleDrawable(getPressedColorSelector(color), null, mask);
            }
            else if (back is RippleDrawable) {
                var ripple = back as RippleDrawable;
                ripple.SetColor(getPressedColorSelector(color));

                return ripple;
            }
            else {
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
}

