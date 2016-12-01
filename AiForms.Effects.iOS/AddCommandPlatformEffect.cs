using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("AiForms")]
[assembly: ExportEffect(typeof(AddCommandPlatformEffect), nameof(AddCommand))]
namespace AiForms.Effects.iOS
{
    public class AddCommandPlatformEffect : PlatformEffect
    {

        private ICommand _command;
        private object _commandParameter;
        private UITapGestureRecognizer _tapGesture;
        private UIView _view;
        private UIView _layer;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            _tapGesture = new UITapGestureRecognizer(async (obj) => {
                if (_layer != null) {
                    _layer.Frame = new CGRect(0, 0, _view.Bounds.Width, _view.Bounds.Height);
                    _view.AddSubview(_layer);
                    _view.BringSubviewToFront(_layer);
                    _layer.Alpha = 1;
                    await UIView.AnimateAsync(0.3f, () => {
                        _layer.Alpha = 0;
                    });
                    _layer.RemoveFromSuperview();
                }

                _command?.Execute(_commandParameter ?? Element);
            });

            _view.UserInteractionEnabled = true;
            _view.AddGestureRecognizer(_tapGesture);

            UpdateCommand();
            UpdateCommandParameter();
            UpdateEffectColor();
        }

        protected override void OnDetached()
        {

            _view.RemoveGestureRecognizer(_tapGesture);
            _tapGesture.Dispose();
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

            if (_layer != null) {
                _layer.Dispose();
                _layer = null;
            }

            var color = AddCommand.GetEffectColor(Element);
            if (color == Xamarin.Forms.Color.Default) {
                return;
            }

            _layer = new UIView();
            _layer.BackgroundColor = color.ToUIColor();

        }
    }
}

