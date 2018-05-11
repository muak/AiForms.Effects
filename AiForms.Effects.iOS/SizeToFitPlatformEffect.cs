using System;
using System.Drawing;
using AiForms.Effects;
using AiForms.Effects.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Metal;

[assembly: ExportEffect(typeof(SizeToFitPlatformEffect), nameof(SizeToFit))]
namespace AiForms.Effects.iOS
{
    public class SizeToFitPlatformEffect : PlatformEffect
    {
        UILabel _view;
        nfloat _orgFontSize;

        protected override void OnAttached()
        {
            _view = Control as UILabel;
            _orgFontSize = _view.Font.PointSize;

            UpdateFitFont();
        }

        protected override void OnDetached()
        {
            _view.Font = UIFont.FromName(_view.Font.Name, _orgFontSize);
            var render = Platform.GetRenderer(Element as Label) as LabelRenderer;
            render?.LayoutSubviews();
            _view = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == VisualElement.HeightProperty.PropertyName ||
                args.PropertyName == VisualElement.WidthProperty.PropertyName ||
                args.PropertyName == Label.TextProperty.PropertyName || 
                args.PropertyName == SizeToFit.CanExpandProperty.PropertyName)
            {
                UpdateFitFont();
            }
            else if(args.PropertyName == Label.FontProperty.PropertyName){
                _orgFontSize = _view.Font.PointSize;
                UpdateFitFont();
            }
        }

        void UpdateFitFont()
        {
            var formsView = Element as View;
            if(formsView.Width < 0 || formsView.Height < 0){
                return;
            }

            var height = MeasureTextSize(_view.Text, formsView.Width, _orgFontSize, _view.Font.Name);

            var fontSize = _orgFontSize;

            if (SizeToFit.GetCanExpand(Element) && height < formsView.Height)
            {
                while (height < formsView.Height)
                {
                    fontSize += 0.5f;
                    height = MeasureTextSize(_view.Text, formsView.Width, fontSize, _view.Font.Name);
                }
            }

            while (height > formsView.Height && fontSize > 0)
            {
                fontSize -= 0.5f;
                height = MeasureTextSize(_view.Text, formsView.Width, fontSize, _view.Font.Name);
            }

            _view.Font = UIFont.FromName(_view.Font.Name, fontSize);

            var render = Platform.GetRenderer(formsView) as LabelRenderer;
            render.LayoutSubviews();
        }

        public double MeasureTextSize(string text, double width, double fontSize, string fontName = null)
        {
            var nsText = new NSString(text);
            var boundSize = new SizeF((float)width, float.MaxValue);
            var options = NSStringDrawingOptions.UsesFontLeading | NSStringDrawingOptions.UsesLineFragmentOrigin;

            if (fontName == null)
            {
                fontName = "HelveticaNeue";
            }

            var attributes = new UIStringAttributes
            {
                Font = UIFont.FromName(fontName, (float)fontSize)
            };

            var sizeF = nsText.GetBoundingRect(boundSize, options, attributes, null).Size;

            return (double)sizeF.Height;// + 5;
        }
    }
}
