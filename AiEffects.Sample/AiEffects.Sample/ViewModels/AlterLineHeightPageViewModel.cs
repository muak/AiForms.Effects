using Prism.Commands;
using Prism.Mvvm;

namespace AiEffects.Sample.ViewModels
{
    public class AlterLineHeightPageViewModel : BindableBase
    {
        private string _LabelText;
        public string LabelText {
            get { return _LabelText; }
            set { SetProperty(ref _LabelText, value); }
        }

        private bool _EffectOn;
        public bool EffectOn {
            get { return _EffectOn; }
            set { SetProperty(ref _EffectOn, value); }
        }

        private DelegateCommand _EffectCommand;
        public DelegateCommand EffectCommand {
            get {
                return _EffectCommand = _EffectCommand ?? new DelegateCommand(() => {
                    EffectOn = !EffectOn;
                });
            }
        }

        public AlterLineHeightPageViewModel()
        {
            LabelText = @"行の高さ（LineHeight/LineSpacing）を設定します。現在のFontSizeの何倍を行の高さにするかを倍率で指定します。
1.5と指定すればFontSizeが10の場合、15が行の高さとなります。Labelの場合は、行間を広げた場合にViewの高さも合わせて可変します。
ただしHeightRequestがあらかじめ指定されている場合はそちらが優先され高さは固定されます。Editorは高さは可変しません。";

            EffectOn = false;
        }
    }
}
