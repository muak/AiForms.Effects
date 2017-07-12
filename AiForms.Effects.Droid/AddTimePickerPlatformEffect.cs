using System;
using Xamarin.Forms.Platform.Android;
using Android.App;
using AiForms.Effects;
using Xamarin.Forms;
using AiForms.Effects.Droid;
using Android.Text.Format;
using Android.Widget;
using System.Windows.Input;

[assembly: ExportEffect(typeof(AddTimePickerPlatformEffect), nameof(AddTimePicker))]
namespace AiForms.Effects.Droid
{
    public class AddTimePickerPlatformEffect : PlatformEffect
    {
        Android.Views.View _view;
        TimePickerDialog _dialog;
        ICommand _command;
        string _title;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            _view.Click += _view_Click;

            UpdateTitle();
            UpdateCommand();
        }

        protected override void OnDetached()
        {
            var renderer = Container as IVisualElementRenderer;
            if (renderer?.Element != null) {
                _view.Click -= _view_Click;
            }
            if (_dialog != null) {
                _dialog.Dispose();
                _dialog = null;
            }
            _view = null;
            _command = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AddTimePicker.TitleProperty.PropertyName) {
                UpdateTitle();
            }
            else if (e.PropertyName == AddTimePicker.CommandProperty.PropertyName) {
                UpdateCommand();
            }
        }

        void _view_Click(object sender, EventArgs e)
        {
            CreateDialog();
        }

        void CreateDialog()
        {
            var time = AddTimePicker.GetTime(Element);
            if (_dialog == null) {
                bool is24HourFormat = DateFormat.Is24HourFormat(Container.Context);
                _dialog = new TimePickerDialog(Container.Context, TimeSelected, time.Hours, time.Minutes, is24HourFormat);

                var title = new TextView(Container.Context);

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
