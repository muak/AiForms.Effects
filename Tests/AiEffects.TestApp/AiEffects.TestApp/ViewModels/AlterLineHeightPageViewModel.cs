using Prism.Commands;
using Prism.Mvvm;

namespace AiEffects.TestApp.ViewModels
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
            LabelText = @"Set the line height (LineHeight / LineSpacing). Specify how many times the current FontSize is to be the height of the line by magnification.
If you specify 1.5, if FontSize is 10, 15 is the height of the line. In the case of Label, when widening the line spacing, also adjust the height of View.
However, if Height Request is specified in advance, it will be given priority and the height will be fixed. The height of the Editor is not variable.";

//            LabelText = @"行の高さ（LineHeight/LineSpacing）を設定します。現在のFontSizeの何倍を行の高さにするかを倍率で指定します。
//1.5と指定すればFontSizeが10の場合、15が行の高さとなります。Labelの場合は、行間を広げた場合にViewの高さも合わせて可変します。
//ただしHeightRequestがあらかじめ指定されている場合はそちらが優先され高さは固定されます。Editorは高さは可変しません。";

            EffectOn = false;
        }
    }
}
