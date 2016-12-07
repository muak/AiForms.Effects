using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;

namespace AiEffects.Sample.ViewModels
{
    public class AddNumberPickerPageViewModel : BindableBase
    {
        private int _LabelNumber;
        public int LabelNumber {
            get { return _LabelNumber; }
            set { SetProperty(ref _LabelNumber, value); }
        }

        private int _BoxNumber;
        public int BoxNumber {
            get { return _BoxNumber; }
            set { SetProperty(ref _BoxNumber, value); }
        }

        private int _StackNumber;
        public int StackNumber {
            get { return _StackNumber; }
            set { SetProperty(ref _StackNumber, value); }
        }

        private int _ButtonNumber;
        public int ButtonNumber {
            get { return _ButtonNumber; }
            set { SetProperty(ref _ButtonNumber, value); }
        }

        private int _ImageNumber;
        public int ImageNumber {
            get { return _ImageNumber; }
            set { SetProperty(ref _ImageNumber, value); }
        }

        private bool _EffectOn;
        public bool EffectOn {
            get { return _EffectOn; }
            set { SetProperty(ref _EffectOn, value); }
        }

        private DelegateCommand<object> _SelectedCommand;
        public DelegateCommand<object> SelectedCommand {
            get {
                return _SelectedCommand = _SelectedCommand ?? new DelegateCommand<object>(async (o) => {
                    await _pageDialog.DisplayAlertAsync("", ((int)o).ToString() + " is selected", "OK");
                });
            }
        }

        private IPageDialogService _pageDialog;

        public AddNumberPickerPageViewModel(IPageDialogService dlg)
        {
            _pageDialog = dlg;

            LabelNumber = 100;
            BoxNumber = 90;
            StackNumber = 50;
            ButtonNumber = 30;
            ImageNumber = 0;
            EffectOn = true;
        }
    }
}
