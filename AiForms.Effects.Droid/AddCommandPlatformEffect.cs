using System;
using System.Linq;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.Droid;
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
        private Android.Views.View _view;
        private FrameLayout _layer;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            UpdateCommand();
            UpdateCommandParameter();
            UpdateEffectColor();

            if (Control is Android.Widget.ListView) {
                //ListViewはOnClickで例外を出すので除外
                return;
            }
            _view.Click += OnClick;
        }

        protected override void OnDetached()
        {
            var renderer = Container as IVisualElementRenderer;
            if (renderer?.Element != null) {    // Disposeされているかの判定
                _view.Click -= OnClick;
                _view.Touch -= View_Touch;
            }
            _command = null;
            _commandParameter = null;
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
            else if (e.PropertyName == AddCommand.EffectColorProperty.PropertyName) {
                UpdateEffectColor();
            }

        }


        void OnClick(object sender, EventArgs e)
        {
            _command?.Execute(_commandParameter ?? Element);
        }

        void UpdateCommand()
        {
            _command = AddCommand.GetCommand(Element);
        }

        void UpdateCommandParameter()
        {
            _commandParameter = AddCommand.GetCommandParameter(Element);
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

            _layer = new FrameLayout(Container.Context);
            _layer.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);
            _layer.SetBackgroundColor(color.ToAndroid());
            _view.Touch += View_Touch;
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

