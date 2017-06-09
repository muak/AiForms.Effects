using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;

namespace AiEffects.TestApp.ViewModels
{
    public class AddCommandPageViewModel : BindableBase, IDestructible, INavigationAware
    {
        private bool _isSoundEffect;
        public bool IsSoundEffect {
            get { return _isSoundEffect; }
            set { 
                SetProperty(ref _isSoundEffect, value); 
            }
        }

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

        private bool _IsExecutedLong;
        public bool IsExecutedLong {
            get { return _IsExecutedLong; }
            set { SetProperty(ref _IsExecutedLong, value); }
        }

        private bool _EnableRipple;
        public bool EnableRipple {
            get { return _EnableRipple; }
            set { SetProperty(ref _EnableRipple, value); }
        }

        private string _TestParam;
        public string TestParam {
            get { return _TestParam; }
            set { SetProperty(ref _TestParam, value); }
        }

        private string _TestLongParam;
        public string TestLongParam {
            get { return _TestLongParam; }
            set { SetProperty(ref _TestLongParam, value); }
        }

        private DelegateCommand<object> _EffectCommand;
        public DelegateCommand<object> EffectCommand {
            get { return _EffectCommand; }
            set { SetProperty(ref _EffectCommand, value); }
        }

        private DelegateCommand<object> _LongCommand;
        public DelegateCommand<object> LongCommand {
            get { return _LongCommand; }
            set { SetProperty(ref _LongCommand, value); }
        }


        private DelegateCommand _ResetCommand;
        public DelegateCommand ResetCommand {
            get {
                return _ResetCommand = _ResetCommand ?? new DelegateCommand(() => {
                    IsExecutedCommand = false;
                    IsExecutedLong = false;
                    CommandParameterText = "";
                });
            }
        }

        private DelegateCommand _ChangeEffectColorCommand;
        public DelegateCommand ChangeEffectColorCommand {
            get {
                return _ChangeEffectColorCommand = _ChangeEffectColorCommand ?? new DelegateCommand(() => {
                    EffectColor = Color.FromHex("#FF00FF");
                });
            }
        }

        private DelegateCommand _ChangeParameterCommand;
        public DelegateCommand ChangeParameterCommand {
            get {
                return _ChangeParameterCommand = _ChangeParameterCommand ?? new DelegateCommand(() => {
                    TestParam = "Fuga";
                });
            }
        }

        private DelegateCommand _ChangeLongParamCommand;
        public DelegateCommand ChangeLongParamCommand {
            get {
                return _ChangeLongParamCommand = _ChangeLongParamCommand ?? new DelegateCommand(() => {
                    TestLongParam = "LongFuga";
                });
            }
        }

        private DelegateCommand _ChangeRippleCommand;
        public DelegateCommand ChangeRippleCommand {
            get {
                return _ChangeRippleCommand = _ChangeRippleCommand ?? new DelegateCommand(() => {
                    EnableRipple = !EnableRipple;
                });
            }
        }

        private DelegateCommand _ToggleEffectCommand;
        public DelegateCommand ToggleEffectCommand {
            get {
                return _ToggleEffectCommand = _ToggleEffectCommand ?? new DelegateCommand(() => {
                    EffectOn = !EffectOn;
                });
            }
        }

        private DelegateCommand _ChangeCommand;
        public DelegateCommand ChangeCommand {
            get {
                return _ChangeCommand = _ChangeCommand ?? new DelegateCommand(() => {
                    if (EffectCommand != null) {
                        EffectCommand = null;
                    }
                    else {
                        EffectCommand = new DelegateCommand<object>(ExecCommand);
                    }

                });
            }
        }

        private DelegateCommand _ChangeLongCommand;
        public DelegateCommand ChangeLongCommand {
            get {
                return _ChangeLongCommand = _ChangeLongCommand ?? new DelegateCommand(() => {
                    if (LongCommand != null) {
                        LongCommand = null;
                    }
                    else {
                        LongCommand = new DelegateCommand<object>(ExecLongCommand);
                    }
                });
            }
        }

        private INavigationService Navigation;

        public AddCommandPageViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
            EffectOn = true;
            EffectColor = Color.FromHex("#FFFF00");
            IsExecutedCommand = false;
            IsExecutedLong = false;
            EnableRipple = true;
            TestParam = "Hoge";
            TestLongParam = "LongHoge";

            _EffectCommand = new DelegateCommand<object>(ExecCommand);
            LongCommand = new DelegateCommand<object>(ExecLongCommand);
        }

        void ExecCommand(object p)
        {
            IsExecutedCommand = true;
            CommandParameterText = p.ToString();
        }

        void ExecLongCommand(object p)
        {
            IsExecutedLong = true;
            CommandParameterText = p.ToString();
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
