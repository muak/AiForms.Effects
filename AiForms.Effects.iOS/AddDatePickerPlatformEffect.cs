using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.iOS;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(AddDatePickerPlatformEffect), nameof(AddDatePicker))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class AddDatePickerPlatformEffect : PlatformEffect
    {
        UIView _view;
        UIDatePicker _picker;
        NoCaretField _entry;
        ICommand _command;
        NSDate _preSelectedDate;


        protected override void OnAttached()
        {
            _view = Control ?? Container;

            CreatePicker();
            UpdateDate();
            UpdateMaxDate();
            UpdateMinDate();
            UpdateCommand();
        }

        protected override void OnDetached()
        {
            _entry.RemoveFromSuperview();
            _entry.Dispose();
            _picker.Dispose();
            _preSelectedDate.Dispose();

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AddDatePicker.DateProperty.PropertyName) {
                UpdateDate();
            }
            else if (e.PropertyName == AddDatePicker.CommandProperty.PropertyName) {
                UpdateCommand();
            }
            else if (e.PropertyName == AddDatePicker.MaxDateProperty.PropertyName) {
                UpdateMaxDate();
            }
            else if (e.PropertyName == AddDatePicker.MinDateProperty.PropertyName) {
                UpdateMinDate();
            }

        }

        void CreatePicker()
        {
            _entry = new NoCaretField();
            _entry.BorderStyle = UITextBorderStyle.None;
            _entry.BackgroundColor = UIColor.Clear;
            _view.AddSubview(_entry);

            _entry.TranslatesAutoresizingMaskIntoConstraints = false;

            _entry.TopAnchor.ConstraintEqualTo(_view.TopAnchor).Active = true;
            _entry.LeftAnchor.ConstraintEqualTo(_view.LeftAnchor).Active = true;
            _entry.BottomAnchor.ConstraintEqualTo(_view.BottomAnchor).Active = true;
            _entry.RightAnchor.ConstraintEqualTo(_view.RightAnchor).Active = true;
            _entry.WidthAnchor.ConstraintEqualTo(_view.WidthAnchor).Active = true;
            _entry.HeightAnchor.ConstraintEqualTo(_view.HeightAnchor).Active = true;

            _view.UserInteractionEnabled = true;
            _view.SendSubviewToBack(_entry);

            _picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new Foundation.NSTimeZone("UTC") };
            if (UIDevice.CurrentDevice.CheckSystemVersion(14, 0))
            {
                _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
            }

            var todayText = AddDatePicker.GetTodayText(Element);

            var width = UIScreen.MainScreen.Bounds.Width;
            var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) => {
                _entry.ResignFirstResponder();
                _picker.Date = _preSelectedDate;
            });

            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => {
                _entry.ResignFirstResponder();
                DoneDate();
                _command?.Execute(_picker.Date.ToDateTime().Date);
            });

            if(!string.IsNullOrEmpty(todayText)){
                var labelButton = new UIBarButtonItem(todayText, UIBarButtonItemStyle.Plain, (sender, e) => {
                    SetToday();
                });
                var fixspacer = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 20 };
                toolbar.SetItems(new[] { cancelButton, spacer, labelButton, fixspacer, doneButton }, false);
            }
            else{
                toolbar.SetItems(new[] { cancelButton, spacer, doneButton }, false);
            }

            _entry.InputView = _picker;
            _entry.InputAccessoryView = toolbar;
        }

        void DoneDate()
        {
            var date = _picker.Date.ToDateTime().Date;
            AddDatePicker.SetDate(Element, date);
            _preSelectedDate = _picker.Date;
        }

        void SetToday()
        {
            if(_picker.MinimumDate.ToDateTime() <= DateTime.Today && _picker.MaximumDate.ToDateTime() >= DateTime.Today){
                _picker.SetDate(DateTime.Today.ToNSDate(), true);
            }
        }

        void UpdateDate()
        {
            var date = AddDatePicker.GetDate(Element).ToNSDate();
            _picker.SetDate(date, false);
            _preSelectedDate = date;
        }

        void UpdateCommand()
        {
            _command = AddDatePicker.GetCommand(Element);
        }

        void UpdateMaxDate()
        {
            _picker.MaximumDate = AddDatePicker.GetMaxDate(Element).ToNSDate();
        }

        void UpdateMinDate()
        {
            _picker.MinimumDate = AddDatePicker.GetMinDate(Element).ToNSDate();
        }
    }
}
