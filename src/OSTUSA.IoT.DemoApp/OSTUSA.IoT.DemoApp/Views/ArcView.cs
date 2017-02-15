using System;

using Xamarin.Forms;

namespace OSTUSA.IoT.DemoApp.Views
{
    public class ArcView : View
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), typeof(ArcView), default(Thickness));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create("StrokeWidth", typeof(float), typeof(ArcView), 0f);
        public float StrokeWidth
        {
            get { return (float)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create("StrokeColor", typeof(Color), typeof(ArcView), Color.Default);
        public Color StrokeColor
        {
            get { return (Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public static readonly BindableProperty StrokeTailColorProperty = BindableProperty.Create("StrokeTailColor", typeof(Color), typeof(ArcView), Color.Default);
        public Color StrokeTailColor
        {
            get { return (Color)GetValue(StrokeTailColorProperty); }
            set { SetValue(StrokeTailColorProperty, value); }
        }

        public static readonly BindableProperty DegreesProperty = BindableProperty.Create("Degrees", typeof(float), typeof(ArcView), 0f);
        public float Degrees
        {
            get { return (float)GetValue(DegreesProperty); }
            set { SetValue(DegreesProperty, value); }
        }
    }
}


