using System;
using System.Linq;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("AiForms")]
[assembly: ExportEffect(typeof(AddCommandPlatformEffect), nameof(AddCommand))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers =true)]
    public class AddCommandPlatformEffect : FeedbackPlatformEffect
    {
        private static Type[] ExceptDisableEffectTargetType = {
            typeof(Button),
            typeof(Picker),
            typeof(Slider),
            typeof(Switch),
        };

        private ICommand _command;
        private object _commandParameter;
        private ICommand _longCommand;
        private object _longCommandParameter;
        private UITapGestureRecognizer _tapGesture;
        private UILongPressGestureRecognizer _longTapGesture;
        private UIView _view;
        private bool _syncCanExecute;
        private bool _isDisableEffectTarget;
        private readonly float _disabledAlpha = 0.3f;

        private bool _isForceDetached;

        protected override void OnAttached()
        {
            if (Control is UIWebView || Control is UIScrollView)
            {
                _isForceDetached = true;
                // Except WebView and ScrollView because of Raising Exception when OnDetached. 
                Device.BeginInvokeOnMainThread(() => AddCommand.SetOn(Element, false));
                return;
            }

            base.OnAttached();

            _view = Control ?? Container;

            _tapGesture = new UITapGestureRecognizer((obj) => {
                if (_command == null)
                    return;

                if (!_command.CanExecute(_commandParameter))
                    return;

                _command.Execute(_commandParameter ?? Element);
            });

            _view.UserInteractionEnabled = true;
            _view.AddGestureRecognizer(_tapGesture);
            _isDisableEffectTarget = !ExceptDisableEffectTargetType.Any(x => x == Element.GetType());

            UpdateSyncCanExecute();
            UpdateCommandParameter();
            UpdateLongCommandParameter();
        }

        protected override void OnDetached()
        {
            if(_isForceDetached)
            {
                return;
            }
            base.OnDetached();

            _view.RemoveGestureRecognizer(_tapGesture);
            _tapGesture.Dispose();
            _tapGesture = null;

            if (_longTapGesture != null) {
                _view.RemoveGestureRecognizer(_longTapGesture);
                _longTapGesture.Dispose();
                _longTapGesture = null;
            }

            if (_command != null) {
                _command.CanExecuteChanged -= CommandCanExecuteChanged;
            }

            if (_longCommand != null) {
                _longCommand.CanExecuteChanged -= CommandCanExecuteChanged;
            }

            _command = null;
            _longCommand = null;
            _commandParameter = null;
            _longCommandParameter = null;

            _view = null;

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
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
            else if (e.PropertyName == AddCommand.LongCommandProperty.PropertyName) {
                UpdateLongCommand();
            }
            else if (e.PropertyName == AddCommand.LongCommandParameterProperty.PropertyName) {
                UpdateLongCommandParameter();
            }
            else if (e.PropertyName == AddCommand.SyncCanExecuteProperty.PropertyName) {
                UpdateSyncCanExecute();
            }
        }

        protected override Color GetEffectColor()
        {
            return AddCommand.GetEffectColor(Element);
        }

        protected override bool GetEnableSound()
        {
            return AddCommand.GetEnableSound(Element);
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

            _longTapGesture = new UILongPressGestureRecognizer((obj) => {
                if (!_longCommand.CanExecute(_longCommandParameter)) {
                    return;
                }

                if (obj.State == UIGestureRecognizerState.Began) {

                    _longCommand?.Execute(_longCommandParameter ?? Element);

                }
            });
            _view.AddGestureRecognizer(_longTapGesture);

        }

        void UpdateLongCommandParameter()
        {
            _longCommandParameter = AddCommand.GetLongCommandParameter(Element);
        }
    }
}

