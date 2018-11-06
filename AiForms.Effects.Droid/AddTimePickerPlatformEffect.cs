using System;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.App;
using Android.Text.Format;
using Android.Widget;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(AddTimePickerPlatformEffect), nameof(AddTimePicker))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AddTimePickerPlatformEffect : AiEffectBase
    {
        Android.Views.View _view;
        TimePickerDialog _dialog;
        ICommand _command;
        string _title;

        protected override void OnAttachedOverride()
        {
            _view = Control ?? Container;

            _view.Touch += _view_Touch;

            UpdateTitle();
            UpdateCommand();
        }

        protected override void OnDetachedOverride()
        {
            if (!IsDisposed) {
                _view.Touch -= _view_Touch;
                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }
            if (_dialog != null) {
                _dialog.Dispose();
                _dialog = null;
            }
            _view = null;
            _command = null;
            System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached completely");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (!IsSupportedByApi)
                return;

            if (IsDisposed) {
                return;
            }

            if (e.PropertyName == AddTimePicker.TitleProperty.PropertyName) {
                UpdateTitle();
            }
            else if (e.PropertyName == AddTimePicker.CommandProperty.PropertyName) {
                UpdateCommand();
            }
        }

        void _view_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event.Action == Android.Views.MotionEventActions.Up) {
                CreateDialog();
            }
        }

        void CreateDialog()
        {
            var time = AddTimePicker.GetTime(Element);
            if (_dialog == null) {
                bool is24HourFormat = DateFormat.Is24HourFormat(_view.Context);
                _dialog = new TimePickerDialog(_view.Context, TimeSelected, time.Hours, time.Minutes, is24HourFormat);

                var title = new TextView(_view.Context);

                if (!string.IsNullOrEmpty(_title)) {
                    title.Gravity = Android.Views.GravityFlags.Center;
                    title.SetPadding(10, 10, 10, 10);
                    title.Text = _title;
                    _dialog.SetCustomTitle(title);
                }

                _dialog.SetCanceledOnTouchOutside(true);

                _dialog.DismissEvent += (ss, ee) => {
                    title.Dispose();
                    _dialog.Dispose();
                    _dialog = null;
                };

                _dialog.Show();
            }
        }

        void TimeSelected(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            var time = new TimeSpan(e.HourOfDay, e.Minute, 0);
            AddTimePicker.SetTime(Element, time);
            _command?.Execute(time);
        }

        void UpdateTitle()
        {
            _title = AddTimePicker.GetTitle(Element);
        }

        void UpdateCommand()
        {
            _command = AddTimePicker.GetCommand(Element);
        }
    }
}
