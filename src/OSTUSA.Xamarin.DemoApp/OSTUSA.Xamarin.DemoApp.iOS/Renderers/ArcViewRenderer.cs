using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Drawing;
using UIKit;
using CoreGraphics;
using OSTUSA.XamarinDemo.DemoApp.Views;

[assembly: ExportRenderer(typeof(ArcView), typeof(OSTUSA.XamarinDemo.DemoApp.iOS.Renderers.ArcViewRenderer))]
namespace OSTUSA.XamarinDemo.DemoApp.iOS.Renderers
{
    public class ArcViewRenderer : VisualElementRenderer<ArcView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ArcView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                SetIsVisible();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            NativeView.SetNeedsDisplay();

            if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
                SetIsVisible();
        }

        private void SetIsVisible()
        {
            Hidden = !Element.IsVisible;
        }

        protected RectangleF AdjustForThickness(CGRect rect)
        {
            var x = rect.X + Element.Padding.Left;
            var y = rect.Y + Element.Padding.Top;
            var width = rect.Width - Element.Padding.HorizontalThickness;
            var height = rect.Height - Element.Padding.VerticalThickness;
            return new RectangleF((float)x, (float)y, (float)width, (float)height);
        }

        public override void Draw(CGRect rect)
        {
            var currentContext = UIGraphics.GetCurrentContext();

            var properRect = AdjustForThickness(rect);

            var centerX = properRect.X + (properRect.Width / 2);
            var centerY = properRect.Y + (properRect.Height / 2);
            var radius = properRect.Width / 2;

            var radians = (float)(Math.PI * 2 * (Element.Degrees / 360));
            currentContext.AddArc(centerX, centerY, radius, 0, radians, false);

            System.Diagnostics.Debug.WriteLine($"Drawing arc from 0 to {radians} for {Element.Degrees} degrees");

            currentContext.SetStrokeColor(Element.StrokeColor.ToCGColor());
            currentContext.SetLineCap(CGLineCap.Round);
            currentContext.SetLineWidth(Element.StrokeWidth);

            currentContext.ReplacePathWithStrokedPath();

            currentContext.DrawPath(CGPathDrawingMode.Stroke);
        }
    }
}

