using System;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ResolutionGroupName("AiForms")]
[assembly: ExportEffect(typeof(AddCommandPlatformEffect), nameof(AddCommand))]
namespace AiForms.Effects.Droid
{

    public class AddCommandPlatformEffect : PlatformEffect
    {
        private ICommand command;
        private object commandParameter;
        private Android.Views.View view;
        private FrameLayout layer;

        protected override void OnAttached() {

            view = Control ?? Container;

            UpdateCommand();
            UpdateCommandParameter();
            UpdateEffectColor();

            view.Click += OnClick;
        }

        protected override void OnDetached() {
            var renderer = Container as IVisualElementRenderer;
            if (renderer?.Element != null) {    // Disposeされているかの判定
                view.Click -= OnClick;
                view.Touch -= View_Touch;
            }
            command = null;
            commandParameter = null;
            view = null;

            if (layer != null) {
                layer.Dispose();
                layer = null;
            }
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e) {
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


        void OnClick(object sender, EventArgs e) {
            command?.Execute(commandParameter ?? Element);
        }

        void UpdateCommand() {
            command = AddCommand.GetCommand(Element);
        }

        void UpdateCommandParameter() {
            commandParameter = AddCommand.GetCommandParameter(Element);
        }

        void UpdateEffectColor() {

            view.Touch -= View_Touch;
            if (layer != null) {
                layer.Dispose();
                layer = null;
            }
            var color = AddCommand.GetEffectColor(Element);
            if (color == Xamarin.Forms.Color.Default) {
                return;
            }

            layer = new FrameLayout(Container.Context);
            layer.LayoutParameters = new ViewGroup.LayoutParams(-1, -1);
            layer.SetBackgroundColor(color.ToAndroid());
            view.Touch += View_Touch;
        }

        void View_Touch(object sender, Android.Views.View.TouchEventArgs e) {
            if (e.Event.Action == MotionEventActions.Down) {
                Container.AddView(layer);
                layer.Top = 0;
                layer.Left = 0;
                layer.Right = view.Width;
                layer.Bottom = view.Height;
                layer.BringToFront();
            }
            if (e.Event.Action == MotionEventActions.Up || e.Event.Action == MotionEventActions.Cancel) {
                Container.RemoveView(layer);
            }

            e.Handled = false;
        }
    }
}

