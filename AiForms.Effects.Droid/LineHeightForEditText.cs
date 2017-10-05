using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using AView = Android.Views.View;

namespace AiForms.Effects.Droid
{
    public class LineHeightForEditText : IAiEffectDroid
    {
        private Android.Views.ViewGroup _container;
        private EditText _editText;
        private float _orgMultiple;
        private VisualElement _formsElement;
        private float _multiple;

        public LineHeightForEditText(Android.Views.ViewGroup container, AView control, Element element)
        {
            _container = container;
            _editText = control as EditText;
            _orgMultiple = _editText.LineSpacingMultiplier;
            _formsElement = element as VisualElement;
        }

        public void OnDetachedIfNotDisposed()
        {
            _editText.SetLineSpacing(1f, _orgMultiple);
            _editText.AfterTextChanged -= _editText_AfterTextChanged;
        }

        public void OnDetached()
        {
            _editText = null;
            _formsElement = null;
        }

        public void Update()
        {
            _editText.AfterTextChanged -= _editText_AfterTextChanged;
            _multiple = (float)AlterLineHeight.GetMultiple(_formsElement);

            _editText.SetLineSpacing(1f, _multiple);
            _editText.AfterTextChanged += _editText_AfterTextChanged;
        }

        void _editText_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {
            //入力した瞬間に次の行との間隔だけが詰まってしまう問題の対策
            _editText.SetLineSpacing(1.01f, _multiple);
            _editText.SetLineSpacing(1f, _multiple);
        }
    }
}
