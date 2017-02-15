using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OSTUSA.IoT.DemoApp.Views
{
    public partial class GaugeView : ContentView
    {
        public GaugeView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(double), typeof(GaugeView), 0d, propertyChanged: (bindable, oldValue, newValue) => CalculateFillDegrees(bindable));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty MinValueProperty = BindableProperty.Create("MinValue", typeof(double), typeof(GaugeView), 0d, propertyChanged: (bindable, oldValue, newValue) => CalculateFillDegrees(bindable));
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create("MaxValue", typeof(double), typeof(GaugeView), 0d, propertyChanged: (bindable, oldValue, newValue) => CalculateFillDegrees(bindable));
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly BindableProperty ReadingProperty = BindableProperty.Create("Reading", typeof(string), typeof(GaugeView), propertyChanged: (bindable, oldValue, newValue) => SetReading(bindable));
        public string Reading
        {
            get { return (string)GetValue(ReadingProperty); }
            set { SetValue(ReadingProperty, value); }
        }

        private static void CalculateFillDegrees(BindableObject bindable)
        {
            var gaugeView = bindable as GaugeView;
            if (gaugeView == null)
                return;

            if (gaugeView.MaxValue <= gaugeView.MinValue)
                return;

            var percent = (gaugeView.Value - gaugeView.MinValue) / (gaugeView.MaxValue - gaugeView.MinValue);
            System.Diagnostics.Debug.WriteLine($"{gaugeView.Value} in [{gaugeView.MinValue}, {gaugeView.MaxValue}] is {percent}");
            gaugeView._progressArc.Degrees = (float)(percent * 270d);
        }

        private static void SetReading(BindableObject bindable)
        {
            var gaugeView = bindable as GaugeView;
            if (gaugeView == null)
                return;

            gaugeView._readingLabel.Text = gaugeView.Reading;
        }
    }
}
