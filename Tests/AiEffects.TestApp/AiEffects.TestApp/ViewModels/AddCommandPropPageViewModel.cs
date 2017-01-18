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

        public AddCommandPropPageViewModel()
        {
            EffectOn = false;
            CommandParameterText = "Hoge";
            EffectColor = Color.FromHex("#50FFFF00");
        }
    }
}
