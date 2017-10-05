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
        private bool _EnableSound;
        public bool EnableSound {
            get { return _EnableSound; }
            set {
                SetProperty(ref _EnableSound, value);
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

        private bool _SyncCanExecute;
        public bool SyncCanExecute {
            get { return _SyncCanExecute; }
            set { SetProperty(ref _SyncCanExecute, value); }
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

        public ReactiveCommand EffectCommand { get; set; }
        public ReactiveCommand LongCommand { get; set; }
        public ReactiveCommand ChangeCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand ChangeLongCommand { get; set; } = new ReactiveCommand();
        public ReactiveProperty<bool> CanExecute { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> CanExecuteLong { get; } = new ReactiveProperty<bool>(false);
        public ReactiveCommand ToggleCanExecute { get; set; } = new ReactiveCommand();

        public AsyncReactiveCommand CanExecuteCommand { get; set; }
        public ReactiveCommand CanExecuteLongCommand { get; set; }
        public ReactiveCommand CanExecuteNullToggle { get; set; } = new ReactiveCommand();
        public ReactiveCommand CanExecuteLongNullToggle { get; set; } = new ReactiveCommand();

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

            EnableSound = true;
            SyncCanExecute = true;

            ToggleCanExecute.Subscribe(_ => {
                CanExecute.Value = !CanExecute.Value;
            });

            IDisposable subCommand = null;
            ChangeCommand.Subscribe(_ => {
                if (EffectCommand != null) {
                    subCommand?.Dispose();
                    EffectCommand = null;
                }
                else {
                    EffectCommand = CanExecute.ToReactiveCommand();
                    subCommand = EffectCommand.Subscribe(ExecCommand);
                }
                OnPropertyChanged(() => this.EffectCommand);
            });

            ChangeCommand.Execute();

            IDisposable subLongCommand = null;
            ChangeLongCommand.Subscribe(_ => {
                if (LongCommand != null) {
                    subLongCommand.Dispose();
                    LongCommand = null;
                }
                else {
                    LongCommand = CanExecute.ToReactiveCommand();
                    subLongCommand = LongCommand.Subscribe(ExecLongCommand);
                }
                OnPropertyChanged(() => this.LongCommand);
            });

            ChangeLongCommand.Execute();



            CanExecuteNullToggle.Subscribe(_ => {
                if (CanExecuteCommand != null) {
                    CanExecuteCommand = null;
                    CommandParameterText = "Command is null";
                }
                else {
                    CanExecuteCommand = CanExecute.ToAsyncReactiveCommand();
                    CanExecuteCommand.Subscribe(async x => {
                        CommandParameterText = "Done Command";
                        await Task.Delay(500);
                    });
                }
                OnPropertyChanged(() => CanExecuteCommand);
            });

            CanExecuteLongNullToggle.Subscribe(_ => {
                if (CanExecuteLongCommand != null) {
                    CanExecuteLongCommand = null;
                    CommandParameterText = "LongCommand is null";
                }
                else {
                    CanExecuteLongCommand = CanExecuteLong.ToReactiveCommand();
                    CanExecuteLongCommand.Subscribe(async x => {
                        CommandParameterText = "Done Long Command";
                        await Task.Delay(500);
                    });
                }
                OnPropertyChanged(() => CanExecuteLongCommand);
            });

            CanExecuteNullToggle.Execute();
            CanExecuteLongNullToggle.Execute();
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
            //EffectOn = false;
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
