using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Drawing;
using UIKit;
using CoreGraphics;
using OSTUSA.IoT.DemoApp.Views;

[assembly: ExportRenderer(typeof(ArcView), typeof(OSTUSA.IoT.DemoApp.iOS.Renderers.ArcViewRenderer))]
namespace OSTUSA.IoT.DemoApp.iOS.Renderers
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

        public override void Draw(CoreGraphics.CGRect rect)
        {
            // http://stackoverflow.com/questions/11783114/draw-outer-half-circle-with-gradient-using-core-graphics-in-ios

            var centerX = rect.X + (rect.Width / 2);
            var centerY = rect.Y + (rect.Height / 2);
            var radius = rect.Width / 2;
            var startAngle = (float)(Element.Rotation / 180d * Math.PI);
            var endAngle = startAngle + (float)(Element.Degrees / 180d * Math.PI);

            var sr = Element.StrokeColor.R;
            var sg = Element.StrokeColor.G;
            var sb = Element.StrokeColor.B;

            var tailColor = Element.StrokeTailColor == Color.Default ? Element.StrokeColor : Element.StrokeTailColor;
            var er = tailColor.R;
            var eg = tailColor.G;
            var eb = tailColor.B;

            Func<float, UIColor> color = x =>
            {
                var r = (nfloat)(x * sr + (1 - x) * er);
                var g = (nfloat)(x * sg + (1 - x) * eg);
                var b = (nfloat)(x * sb + (1 - x) * eb);
                    
                return new UIColor(r, g, b, 1f);
            };

            var context = UIGraphics.GetCurrentContext();
            DrawGradientInContext(
                context,
                startAngle,
                endAngle,
                (float)(radius - Element.StrokeWidth),
                (float)radius,
                color,
                32,
                new CGPoint(centerX, centerY)
            );
        }

        private CGPoint GetPointForTrapezoidWithAngle(float angle, float radius, CGPoint center)
        {
            return new CGPoint(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
        }

        private void DrawGradientInContext(
            CGContext ctx,
            float startAngle,
            float endAngle,
            float innerRadius,
            float outerRadius,
            Func<float, UIColor> colorBlock,
            int subdivCount,
            CGPoint center,
            float scale = 1f
        )
        {
            if (startAngle == endAngle)
                return;
            
            float angleDelta = (endAngle - startAngle) / subdivCount;
            float fractionDelta = 1.0f / subdivCount;

            CGPoint p0, p1, p2, p3;
            float currentAngle = startAngle;
            p0 = GetPointForTrapezoidWithAngle(currentAngle, innerRadius, center);
            p3 = GetPointForTrapezoidWithAngle(currentAngle, outerRadius, center);

            ctx.SetLineWidth(1);

            for (var i = 0; i < subdivCount; i++)
            {
                var fraction = (float)i / subdivCount;
                currentAngle = startAngle + fraction * (endAngle - startAngle);
                using (var trapezoid = new CGPath())
                {
                    p1 = GetPointForTrapezoidWithAngle(currentAngle + angleDelta, innerRadius, center);
                    p2 = GetPointForTrapezoidWithAngle(currentAngle + angleDelta, outerRadius, center);

                    trapezoid.MoveToPoint(p0);
                    trapezoid.AddLineToPoint(p1);
                    trapezoid.AddLineToPoint(p2);
                    trapezoid.AddLineToPoint(p3);
                    trapezoid.CloseSubpath();

                    ctx.AddPath(trapezoid);

                    var color = colorBlock(fraction).CGColor;
                    ctx.SetFillColor(color);
                    ctx.SetStrokeColor(color);
                    ctx.SetMiterLimit(0);
                    ctx.SetLineCap(CGLineCap.Square);

                    ctx.DrawPath(CGPathDrawingMode.FillStroke);
                }

                p0 = p1;
                p3 = p2;
            }
        }
    }
}

