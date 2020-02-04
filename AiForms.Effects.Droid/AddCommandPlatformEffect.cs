using System;
using System.Linq;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.Views;
using Xamarin.Forms;

[assembly: ResolutionGroupName("AiForms")]
[assembly: ExportEffect(typeof(AddCommandPlatformEffect), nameof(AddCommand))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers =true)]
    public class AddCommandPlatformEffect : FeedbackPlatformEffect
    {
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
        private bool _isTapTargetSoundEffect;
        private bool _isDisableEffectTarget;
        private bool _syncCanExecute;

        private GestureDetector _gestureDetector;

        protected override void OnAttachedOverride()
        {
            base.OnAttachedOverride();

            _view = Control ?? Container;

            if (Control is Android.Widget.ListView || Control is Android.Widget.ScrollView) {
                // Except ListView and ScrollView because of Raising Exception. 
                Device.BeginInvokeOnMainThread(() => AddCommand.SetOn(Element, false));
                return;
            }

            _isTapTargetSoundEffect = TapSoundEffectElementType.Any(x => x == Element.GetType());

            _gestureDetector = new GestureDetector(_view.Context, new ViewGestureListener(this));
            _gestureDetector.IsLongpressEnabled = true;

            _view.Clickable = true;
            _view.LongClickable = true;
                        
            UpdateSyncCanExecute();
            UpdateCommandParameter();
            UpdateLongCommandParameter();
            UpdateIsEnabled();

            _view.Touch += _view_Touch;            
        }

        protected override void OnDetachedOverride()
        {
            if (!IsDisposed) {
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

            _gestureDetector?.Dispose();
            _gestureDetector = null;

            _view = null;

            base.OnDetachedOverride();
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (!IsSupportedByApi)
                return;

            if (IsDisposed) {
                return;
            }

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

        void _view_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            _gestureDetector.OnTouchEvent(e.Event);

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
                return !label.TextColor.IsDefault || !label.BackgroundColor.IsDefault;
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

                _effect._command.Execute(_effect._commandParameter ?? _effect.Element);

                return false;
            }
        }
    }
}

