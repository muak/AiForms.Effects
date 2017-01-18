using System;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;

[assembly: ResolutionGroupName("AiForms")]
[assembly: ExportEffect(typeof(AddCommandPlatformEffect), nameof(AddCommand))]
namespace AiForms.Effects.iOS
{
    public class AddCommandPlatformEffect : PlatformEffect
    {

        private ICommand _command;
        private object _commandParameter;
        private ICommand _longCommand;
        private object _longCommandParameter;
        private UITapGestureRecognizer _tapGesture;
        private UILongPressGestureRecognizer _longTapGesture;
        private UIView _view;
        private UIView _layer;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            _tapGesture = new UITapGestureRecognizer(async (obj) => {

                await TapAnimation(0.3);
                _command?.Execute(_commandParameter ?? Element);
            });

            _view.UserInteractionEnabled = true;
            _view.AddGestureRecognizer(_tapGesture);

            UpdateCommand();
            UpdateCommandParameter();
            UpdateLongCommand();
            UpdateLongCommandParameter();
            UpdateEffectColor();
        }

        protected override void OnDetached()
        {

            _view.RemoveGestureRecognizer(_tapGesture);
            _tapGesture.Dispose();

            if (_longTapGesture != null) {
                _view.RemoveGestureRecognizer(_longTapGesture);
                _longTapGesture.Dispose();
            }

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
            _longCommand = AddCommand.GetLongCommand(Element);
            if (_longTapGesture != null) {
                _view.RemoveGestureRecognizer(_longTapGesture);
                _longTapGesture.Dispose();
            }
            if (_longCommand == null) {
                return;
            }
            _longTapGesture = new UILongPressGestureRecognizer(async (obj) => {
                if (obj.State == UIGestureRecognizerState.Began) {

                    _longCommand?.Execute(_longCommandParameter ?? Element);

                    await TapAnimation(0.5, 0, 1, false);
                }
                else if (obj.State == UIGestureRecognizerState.Ended ||
                         obj.State == UIGestureRecognizerState.Cancelled ||
                         obj.State == UIGestureRecognizerState.Failed) {

                    await TapAnimation(0.5);
                }
            });
            _view.AddGestureRecognizer(_longTapGesture);

        }
        void UpdateLongCommandParameter()
        {
            _longCommandParameter = AddCommand.GetLongCommandParameter(Element);
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

        async Task TapAnimation(double duration, float start = 1, float end = 0, bool remove = true)
        {
            if (_layer != null) {
                _layer.Frame = new CGRect(0, 0, Container.Bounds.Width, Container.Bounds.Height);
                Container.AddSubview(_layer);
                Container.BringSubviewToFront(_layer);
                _layer.Alpha = start;
                await UIView.AnimateAsync(duration, () => {
                    _layer.Alpha = end;
                });
                if (remove) {
                    _layer.RemoveFromSuperview();
                }
            }
        }
    }
}

