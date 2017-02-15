using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Util;
using Android.Graphics;
using OSTUSA.XamarinDemo.DemoApp.Views;

[assembly: ExportRenderer(typeof(ArcView), typeof(OSTUSA.XamarinDemo.DemoApp.Renderers.ArcViewRenderer))]
namespace OSTUSA.XamarinDemo.DemoApp.Droid.Renderers
{
    public class ArcViewRenderer : ViewRenderer<ArcView, Arc>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ArcView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            SetNativeControl(new Arc(Resources.DisplayMetrics.Density, Context, Element));
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.Invalidate();
        }
    }

    public class Arc : Android.Views.View
    {
        private readonly ArcView _arcView;

        // Pixel density
        private readonly float density;

        // We need to make sure we account for the padding changes
        public new int Width
        {
            get { return base.Width - (int)(Resize(_arcView.Padding.HorizontalThickness)); }
        }

        public new int Height
        {
            get { return base.Height - (int)(Resize(_arcView.Padding.VerticalThickness)); }
        }

        public Arc(float density, Context context, ArcView arcView) : base(context)
        {
            _arcView = arcView;
            this.density = density;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // We need to account for offsetting the coordinates based on the padding
            var x = GetX() + Resize(_arcView.Padding.Left);
            var y = GetY() + Resize(_arcView.Padding.Top);

            var strokePaint = new Paint(PaintFlags.AntiAlias);
            strokePaint.SetStyle(Paint.Style.Stroke);
            strokePaint.StrokeWidth = Resize(_arcView.StrokeWidth);
            strokePaint.StrokeCap = Paint.Cap.Square;
            strokePaint.Color = _arcView.StrokeColor.ToAndroid();

            if (_arcView.StrokeTailColor != Xamarin.Forms.Color.Default)
            {
                var gradient = new SweepGradient(Width / 2, Height / 2, _arcView.StrokeTailColor.ToAndroid(), _arcView.StrokeColor.ToAndroid());

                var gradientMatrix = new Matrix();
                gradientMatrix.PreRotate((float)_arcView.Rotation - _arcView.Degrees, Width / 2, Height / 2);
                gradient.SetLocalMatrix(gradientMatrix);

                strokePaint.SetShader(gradient);
            }

            canvas.DrawArc(new RectF(x, y, x + Width, y + Height), (float)_arcView.Rotation, -_arcView.Degrees, false, strokePaint);
        }

        // Helper functions for dealing with pizel density
        private float Resize(float input)
        {
            return input * density;
        }

        private float Resize(double input)
        {
            return Resize((float)input);
        }
    }
}


