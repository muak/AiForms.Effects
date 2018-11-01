using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(NumberPickerPlatform), nameof(AddNumberPicker))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class NumberPickerPlatform : PlatformEffect
    {
        private UIPickerView _picker;
        private NumberPickerSource _model;
        private NoCaretField _entry;
        private UIView _view;
        private NSLayoutConstraint[] _constraint;
        private UILabel _title;
        private ICommand _command;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            CreatePicker();

            UpdateList();
            UpdateNumber();
            UpdateTitle();
            UpdateCommand();
        }

        protected override void OnDetached()
        {
            _view.RemoveConstraints(_constraint);
            _entry.RemoveFromSuperview();
            _entry.Dispose();
            _model.Dispose();
            _title.Dispose();
            _picker.Dispose();

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AddNumberPicker.MaxProperty.PropertyName) {
                UpdateList();
            }
            else if (e.PropertyName == AddNumberPicker.MinProperty.PropertyName) {
                UpdateList();
            }
            else if (e.PropertyName == AddNumberPicker.NumberProperty.PropertyName) {
                UpdateNumber();
            }
            else if (e.PropertyName == AddNumberPicker.TitleProperty.PropertyName) {
                UpdateTitle();
            }
            else if (e.PropertyName == AddNumberPicker.CommandProperty.PropertyName) {
                UpdateCommand();
            }
        }

        void CreatePicker()
        {
            _entry = new NoCaretField();
            _entry.BorderStyle = UITextBorderStyle.None;
            _entry.BackgroundColor = UIColor.Clear;
            _view.AddSubview(_entry);

            //_view.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            _entry.TranslatesAutoresizingMaskIntoConstraints = false;

            _constraint = CreateConstraint(_view, _entry);

            _view.UserInteractionEnabled = true;
            _view.AddConstraints(_constraint);
            _view.SendSubviewToBack(_entry);

            _picker = new UIPickerView();

            var width = UIScreen.MainScreen.Bounds.Width;
            var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };

            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) => {
                _entry.ResignFirstResponder();
                Select(_model.PreSelectedItem);
            });

            _title = new UILabel();
            var labelButton = new UIBarButtonItem(_title);

            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => {
                var s = (NumberPickerSource)_picker.Model;
                UpdatePickerFromModel(s);
                _entry.ResignFirstResponder();
                _command?.Execute(s.SelectedItem);
            });

            toolbar.SetItems(new[] { cancelButton, spacer, labelButton, spacer, doneButton }, false);

            _entry.InputView = _picker;
            _entry.InputAccessoryView = toolbar;

            _model = new NumberPickerSource();
            _picker.Model = _model;

        }

        NSLayoutConstraint[] CreateConstraint(UIView parent, UIView child)
        {
            return new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(
                    child,
                    NSLayoutAttribute.Top,
                    NSLayoutRelation.Equal,
                    parent,
                    NSLayoutAttribute.Top,
                    1,
                    0
                ),
                NSLayoutConstraint.Create(
                    child,
                    NSLayoutAttribute.Left,
                    NSLayoutRelation.Equal,
                    parent,
                    NSLayoutAttribute.Left,
                    1,
                    0
                ),
                NSLayoutConstraint.Create(
                    child,
                    NSLayoutAttribute.Right,
                    NSLayoutRelation.Equal,
                    parent,
                    NSLayoutAttribute.Right,
                    1,
                    0
                ),
                NSLayoutConstraint.Create(
                    child,
                    NSLayoutAttribute.Bottom,
                    NSLayoutRelation.Equal,
                    parent,
                    NSLayoutAttribute.Bottom,
                    1,
                    0
                ),
                NSLayoutConstraint.Create(
                    child,
                    NSLayoutAttribute.Width,
                    NSLayoutRelation.Equal,
                    parent,
                    NSLayoutAttribute.Width,
                    1,
                    0
                ),
                NSLayoutConstraint.Create(
                    child,
                    NSLayoutAttribute.Height,
                    NSLayoutRelation.Equal,
                    parent,
                    NSLayoutAttribute.Height,
                    1,
                    0
                ),
            };
        }

        void Select(int number)
        {
            var idx = _model.Items.IndexOf(number);
            if (idx == -1) {
                number = _model.Items[0];
                idx = 0;
            }
            _picker.Select(idx, 0, false);
            _model.SelectedItem = number;
            _model.SelectedIndex = idx;
            _model.PreSelectedItem = number;
        }

        void UpdateList()
        {
            _model.SetNumbers(AddNumberPicker.GetMin(Element), AddNumberPicker.GetMax(Element));
        }

        void UpdateNumber()
        {
            Select(AddNumberPicker.GetNumber(Element));
        }

        void UpdatePickerFromModel(NumberPickerSource s)
        {
            _model.PreSelectedItem = s.SelectedItem;
            AddNumberPicker.SetNumber(Element, s.SelectedItem);
        }

        void UpdateTitle()
        {
            _title.Text = AddNumberPicker.GetTitle(Element);
            _title.SizeToFit();
        }

        void UpdateCommand()
        {
            _command = AddNumberPicker.GetCommand(Element);
        }

    }

}
