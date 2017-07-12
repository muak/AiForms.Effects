using System;
using Xamarin.Forms.Platform.Android;
using Android.App;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(AddDatePickerPlatformEffect), nameof(AddDatePicker))]
namespace AiForms.Effects.Droid
{
    public class AddDatePickerPlatformEffect : PlatformEffect
    {
        Android.Views.View _view;
        DatePickerDialog _dialog;
        ICommand _command;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            _view.Click += _view_Click;

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

            if (e.PropertyName == AddDatePicker.CommandProperty.PropertyName) {
                UpdateCommand();
            }
        }

        void _view_Click(object sender, EventArgs e)
        {
            if (_dialog != null) {
                _dialog.Dispose();
            }

            CreateDialog();

            UpdateMinDate();
            UpdateMaxDate();

            _dialog.CancelEvent += OnCancelButtonClicked;

            _dialog.Show();
        }

        void CreateDialog()
        {
            var date = AddDatePicker.GetDate(Element);

            _dialog = new DatePickerDialog(Container.Context, (o, e) => {
                AddDatePicker.SetDate(Element, e.Date);
                _command?.Execute(e.Date);
                _view.ClearFocus();
                _dialog.CancelEvent -= OnCancelButtonClicked;

                _dialog = null;
            }, date.Year, date.Month - 1, date.Day);

            _dialog.SetCanceledOnTouchOutside(true);
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.ClearFocus();
        }


        void UpdateMaxDate()
        {
            if (_dialog != null) {
                //when not to specify 23:59:59,last day can't be selected. 
                _dialog.DatePicker.MaxDate = (long)AddDatePicker.GetMaxDate(Element).AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        void UpdateMinDate()
        {
            if (_dialog != null) {
                _dialog.DatePicker.MinDate = (long)AddDatePicker.GetMinDate(Element).ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        void UpdateCommand()
        {
            _command = AddDatePicker.GetCommand(Element);
        }
    }
}
