using System.Threading.Tasks;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AudioToolbox;
using System;
using System.Linq;

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
        private bool _enableSound;
        private UIView _view;
        private UIView _layer;
        private double _alpha;
        private SystemSound _clickSound;
        private bool _syncCanExecute;
        private bool _isDisableEffectTarget;
        private static Type[] ExceptDisableEffectTargetType = {
            typeof(Button),
            typeof(Picker),
            typeof(Slider),
            typeof(Switch),
        };

        private readonly float _disabledAlpha = 0.3f;
        public uint PlaySoundNo { get; set; } = 1306;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            _tapGesture = new UITapGestureRecognizer(async (obj) => {
                if (_command == null)
                    return;

                if (!_command.CanExecute(_commandParameter))
                    return;

                await TapAnimation(0.3, _alpha, 0);

                if (_enableSound)
                    PlayClickSound();

                _command.Execute(_commandParameter ?? Element);
            });

            _view.UserInteractionEnabled = true;
            _view.AddGestureRecognizer(_tapGesture);
            _isDisableEffectTarget = !ExceptDisableEffectTargetType.Any(x => x == Element.GetType());

            UpdateSyncCanExecute();
            UpdateCommandParameter();
            UpdateLongCommandParameter();
            UpdateEffectColor();
            UpdateEnableSound();
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

            if (_command != null) {
                _command.CanExecuteChanged -= CommandCanExecuteChanged;
            }

            if (_longCommand != null) {
                _longCommand.CanExecuteChanged -= CommandCanExecuteChanged;
            }

            _clickSound?.Dispose();
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
            UpdateCommand();
            UpdateLongCommand();
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
            var forms = Element as View;
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
            }

            _longCommand = AddCommand.GetLongCommand(Element);

            if (_longTapGesture != null) {
                _view.RemoveGestureRecognizer(_longTapGesture);
                _longTapGesture.Dispose();
            }

            if (_longCommand == null) {
                return;
            }

            if (_syncCanExecute) {
                _longCommand.CanExecuteChanged += CommandCanExecuteChanged;
                CommandCanExecuteChanged(_longCommand, System.EventArgs.Empty);
            }

            _longTapGesture = new UILongPressGestureRecognizer(async (obj) => {
                if (!_longCommand.CanExecute(_longCommandParameter)) {
                    return;
                }

                if (obj.State == UIGestureRecognizerState.Began) {
                    if (_enableSound)
                        PlayClickSound();

                    _longCommand?.Execute(_longCommandParameter ?? Element);

                    await TapAnimation(0.5, 0, _alpha, false);
                }
                else if (obj.State == UIGestureRecognizerState.Ended ||
                         obj.State == UIGestureRecognizerState.Cancelled ||
                         obj.State == UIGestureRecognizerState.Failed) {

                    await TapAnimation(0.5, _alpha, 0);
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
            _alpha = color.A < 1.0 ? 1 : 0.3;

            _layer = new UIView();
            _layer.BackgroundColor = color.ToUIColor();

        }

        void UpdateEnableSound()
        {
            _enableSound = AddCommand.GetEnableSound(Element);
        }

        async Task TapAnimation(double duration, double start = 1, double end = 0, bool remove = true)
        {
            if (_layer != null) {
                _layer.Frame = new CGRect(0, 0, Container.Bounds.Width, Container.Bounds.Height);
                Container.AddSubview(_layer);
                Container.BringSubviewToFront(_layer);
                _layer.Alpha = (float)start;
                await UIView.AnimateAsync(duration, () => {
                    _layer.Alpha = (float)end;
                });
                if (remove) {
                    _layer.RemoveFromSuperview();
                }
            }
        }

        void PlayClickSound()
        {
            if (_clickSound == null)
                _clickSound = new SystemSound(PlaySoundNo);

            _clickSound.PlaySystemSoundAsync();
        }

    }
}

