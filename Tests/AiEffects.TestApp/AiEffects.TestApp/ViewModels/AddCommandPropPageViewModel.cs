using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AiEffects.TestApp.ViewModels
{
    public class AddCommandPropPageViewModel : BindableBase
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

        private string _ParameterText;
        public string ParameterText {
            get { return _ParameterText; }
            set { SetProperty(ref _ParameterText, value); }
        }

        private string _CommandParameterText;
        public string CommandParameterText {
            get { return _CommandParameterText; }
            set { SetProperty(ref _CommandParameterText, value); }
        }

        private string _LongParameterText;
        public string LongParameterText {
            get { return _LongParameterText; }
            set { SetProperty(ref _LongParameterText, value); }
        }

        private DelegateCommand _ToggleEffectCommand;
        public DelegateCommand ToggleEffectCommand {
            get { return _ToggleEffectCommand = _ToggleEffectCommand ?? new DelegateCommand(() => {
                EffectOn = !EffectOn;
            }); }
        }

        private DelegateCommand<object> _EffectCommand;
        public DelegateCommand<object> EffectCommand {
            get {
                return _EffectCommand = _EffectCommand ?? new DelegateCommand<object>((p) => {
                    IsExecutedCommand = true;
                    ParameterText = p.ToString();
                });
            }
        }

        private DelegateCommand _ResetCommand;
        public DelegateCommand ResetCommand {
            get {
                return _ResetCommand = _ResetCommand ?? new DelegateCommand(() => {
                    IsExecutedCommand = false;
                    IsExecutedLong = false;
                    ParameterText = "";
                });
            }
        }

        private DelegateCommand _ChangeEffectColorCommand;
        public DelegateCommand ChangeEffectColorCommand {
            get { return _ChangeEffectColorCommand = _ChangeEffectColorCommand ?? new DelegateCommand(() => {
                EffectColor = Color.FromHex("#50FF00FF");
            }); }
        }

        private DelegateCommand _ChangeParameterCommand;
        public DelegateCommand ChangeParameterCommand {
            get { return _ChangeParameterCommand = _ChangeParameterCommand ?? new DelegateCommand(() => {
                CommandParameterText = "Fuga";
            }); }
        }

        private DelegateCommand _ChangeLongParamCommand;
        public DelegateCommand ChangeLongParamCommand {
            get { return _ChangeLongParamCommand = _ChangeLongParamCommand ?? new DelegateCommand(() => {
                LongParameterText = "LongFuga";
            }); }
        }

        private DelegateCommand _ChangeRippleCommand;
        public DelegateCommand ChangeRippleCommand {
            get { return _ChangeRippleCommand = _ChangeRippleCommand ?? new DelegateCommand(() => {
                EnableRipple = !EnableRipple;       
            }); }
        }

        private bool _EnableRipple;
        public bool EnableRipple {
            get { return _EnableRipple; }
            set { SetProperty(ref _EnableRipple, value); }
        }

        private bool _IsExecutedLong;
        public bool IsExecutedLong {
            get { return _IsExecutedLong; }
            set { SetProperty(ref _IsExecutedLong, value); }
        }

        private DelegateCommand<object> _LongCommand;
        public DelegateCommand<object> LongCommand {
            get {
                return _LongCommand = _LongCommand ?? new DelegateCommand<object>((p) => {
                    IsExecutedLong = true;
                    ParameterText = p.ToString();
                });
            }
        }

        public AddCommandPropPageViewModel()
        {
            EffectOn = false;
            CommandParameterText = "Hoge";
            LongParameterText = "LongHoge";
            EffectColor = Color.FromHex("#50FFFF00");
            EnableRipple = false;

        }
    }
}
