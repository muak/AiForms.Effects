using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace AiEffects.TestApp.ViewModels
{
    public class AddCommandPageViewModel : BindableBase, IDestructible, INavigationAware
    {
        private bool _EffectOn;
        public bool EffectOn {
            get { return _EffectOn; }
            set { SetProperty(ref _EffectOn, value); }
        }

        private Color _EffectColor;
        public Color EffectColor {
            get { return _EffectColor; }
            set { SetProperty(ref _EffectColor, value); }
        }

        private bool _IsExecutedCommand;
        public bool IsExecutedCommand {
            get { return _IsExecutedCommand; }
            set { SetProperty(ref _IsExecutedCommand, value); }
        }

        private string _CommandParameterText;
        public string CommandParameterText {
            get { return _CommandParameterText; }
            set { SetProperty(ref _CommandParameterText, value); }
        }

        private DelegateCommand<object> _EffectCommand;
        public DelegateCommand<object> EffectCommand {
            get {
                return _EffectCommand = _EffectCommand ?? new DelegateCommand<object>((p) => {
                    IsExecutedCommand = true;
                    CommandParameterText = p.ToString();
                });
            }
        }

        private DelegateCommand _ResetCommand;
        public DelegateCommand ResetCommand {
            get { return _ResetCommand = _ResetCommand ?? new DelegateCommand(() => {
                IsExecutedCommand = false;
                IsExecutedLong = false;
                CommandParameterText = "";
            }); } 
        }

        private bool _IsExecutedLong;
        public bool IsExecutedLong {
            get { return _IsExecutedLong; }
            set { SetProperty(ref _IsExecutedLong, value); }
        }

        private DelegateCommand<object> _LongCommand;
        public DelegateCommand<object> LongCommand {
            get { return _LongCommand = _LongCommand ?? new DelegateCommand<object>((p) => {
                IsExecutedLong = true;
                CommandParameterText = p.ToString();
            }); }
        }

        private INavigationService Navigation;

        public AddCommandPageViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
            EffectOn = true;
            EffectColor = Color.FromHex("#50FFFF00");
            IsExecutedCommand = false;
            IsExecutedLong = false;
        }

        public void Destroy()
        {

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
