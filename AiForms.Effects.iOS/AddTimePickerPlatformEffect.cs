using System;
using AiForms.Effects;
using AiForms.Effects.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Windows.Input;

[assembly: ExportEffect(typeof(AddTimePickerPlatformEffect), nameof(AddTimePicker))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class AddTimePickerPlatformEffect : PlatformEffect
    {
        UIDatePicker _picker;
        NoCaretField _entry;
        UIView _view;
        UILabel _title;
        NSDate _preSelectedDate;
        ICommand _command;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            CreatePicker();
            UpdateTime();
            UpdateTitle();
            UpdateCommand();
        }

        protected override void OnDetached()
        {
            _entry.RemoveFromSuperview();
            _entry.Dispose();
            _title.Dispose();
            _picker.Dispose();
            _preSelectedDate.Dispose();

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AddTimePicker.TimeProperty.PropertyName) {
                UpdateTime();
            }
            else if (e.PropertyName == AddTimePicker.TitleProperty.PropertyName) {
                UpdateTitle();
            }
            else if (e.PropertyName == AddTimePicker.CommandProperty.PropertyName) {
                UpdateCommand();
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

            _picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new Foundation.NSTimeZone("UTC") };
            if (UIDevice.CurrentDevice.CheckSystemVersion(14, 0))
            {
                _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
            }

            _title = new UILabel();

            var width = UIScreen.MainScreen.Bounds.Width;
            var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) => {
                _entry.ResignFirstResponder();
                _picker.Date = _preSelectedDate;
            });

            var labelButton = new UIBarButtonItem(_title);
            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => {
                _entry.ResignFirstResponder();
                DoneTime();
                _command?.Execute(_picker.Date.ToDateTime() - new DateTime(1, 1, 1));
            });

            toolbar.SetItems(new[] { cancelButton, spacer, labelButton, spacer, doneButton }, false);

            _entry.InputView = _picker;
            _entry.InputAccessoryView = toolbar;

        }

        void DoneTime()
        {
            var time = _picker.Date.ToDateTime() - new DateTime(1, 1, 1);
            AddTimePicker.SetTime(Element, time);
            _preSelectedDate = _picker.Date;
        }

        void UpdateTime()
        {
            var time = AddTimePicker.GetTime(Element);
            _picker.Date = new DateTime(1, 1, 1).Add(time).ToNSDate();
            _preSelectedDate = _picker.Date;
        }


        void UpdateTitle()
        {
            _title.Text = AddTimePicker.GetTitle(Element);
            _title.SizeToFit();
        }

        void UpdateCommand()
        {
            _command = AddTimePicker.GetCommand(Element);
        }

    }
}
