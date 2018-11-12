using System;
using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Android.App;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using APicker = Android.Widget.NumberPicker;

[assembly: ExportEffect(typeof(AddNumberPickerPlatform), nameof(AddNumberPicker))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AddNumberPickerPlatform : AiEffectBase
    {
        private AlertDialog _dialog;
        private Android.Views.View _view;
        private ICommand _command;
        private int _min;
        private int _max;
        private int _number;

        protected override void OnAttachedOverride()
        {
            _view = Control ?? Container;

            _view.Touch += _view_Touch;

            UpdateList();
            UpdateCommand();
        }

        void _view_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up) {
                CreateDialog();
            }
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

            if (e.PropertyName == AddNumberPicker.MaxProperty.PropertyName) {
                UpdateList();
            }
            else if (e.PropertyName == AddNumberPicker.MinProperty.PropertyName) {
                UpdateList();
            }
            else if (e.PropertyName == AddNumberPicker.NumberProperty.PropertyName) {
                UpdateNumber();
            }
            else if (e.PropertyName == AddNumberPicker.CommandProperty.PropertyName) {
                UpdateCommand();
            }
        }

        void CreateDialog()
        {
            if (_dialog != null) return;

            var picker = new APicker(_view.Context);
            picker.MinValue = _min;
            picker.MaxValue = _max;
            picker.Value = _number;

            using (var builder = new AlertDialog.Builder(_view.Context)) {

                builder.SetTitle(AddNumberPicker.GetTitle(Element));

                Android.Widget.FrameLayout parent = new Android.Widget.FrameLayout(_view.Context);
                parent.AddView(picker, new Android.Widget.FrameLayout.LayoutParams(
                        ViewGroup.LayoutParams.WrapContent,
                        ViewGroup.LayoutParams.WrapContent,
                       GravityFlags.Center));
                builder.SetView(parent);

                builder.SetNegativeButton(global::Android.Resource.String.Cancel, (o, args) => { });

                builder.SetPositiveButton(global::Android.Resource.String.Ok, (o, args) => {
                    AddNumberPicker.SetNumber(Element, picker.Value);
                    _command?.Execute(picker.Value);
                });

                _dialog = builder.Create();
            }

            _dialog.SetCanceledOnTouchOutside(true);

            _dialog.DismissEvent += (ss, ee) => {
                _dialog.Dispose();
                _dialog = null;
                picker.RemoveFromParent();
                picker.Dispose();
                picker = null;
            };

            _dialog.Show();
        }

        void UpdateList()
        {
            _min = AddNumberPicker.GetMin(Element);
            _max = AddNumberPicker.GetMax(Element);
            if (_min > _max) {
                throw new ArgumentOutOfRangeException(
                    AddNumberPicker.MaxProperty.PropertyName, "Min must not be larger than Max");
            }
            if (_min < 0) _min = 0;
            if (_max < 0) _max = 0;

            UpdateNumber();
        }
        void UpdateNumber()
        {
            _number = AddNumberPicker.GetNumber(Element);
            if (_number < _min || _number > _max) {
                _number = _min;
            }
        }

        void UpdateCommand()
        {
            _command = AddNumberPicker.GetCommand(Element);
        }
    }
}
