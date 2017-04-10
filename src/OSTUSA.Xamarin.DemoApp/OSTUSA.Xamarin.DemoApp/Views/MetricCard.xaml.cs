using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;

namespace OSTUSA.XamarinDemo.DemoApp.Views
{
    public partial class MetricCard : ContentView
    {
        private bool _isEditing;

        public MetricCard()
        {
            InitializeComponent();
        }

        public string Headline
        {
            get { return _headlineLabel.Text; }
            set { _headlineLabel.Text = value; }
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(double), typeof(GaugeView), 0d, propertyChanged: ValuePropertyChanged);
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty MinValueProperty = BindableProperty.Create("MinValue", typeof(double), typeof(GaugeView), 0d, BindingMode.TwoWay, propertyChanged: MinValuePropertyChanged);
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create("MaxValue", typeof(double), typeof(GaugeView), 0d, BindingMode.TwoWay, propertyChanged: MaxValuePropertyChanged);
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        private static void ValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var metricCard = bindable as MetricCard;
            if (metricCard == null)
                return;

            metricCard._valueLabel.Text = metricCard.Value.ToString();
        }

        private static void MinValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var metricCard = bindable as MetricCard;
            if (metricCard == null)
                return;

            metricCard._minLabel.Text = metricCard.MinValue.ToString();
            metricCard._minEntry.Text = metricCard.MinValue.ToString();
        }

        private static void MaxValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var metricCard = bindable as MetricCard;
            if (metricCard == null)
                return;

            metricCard._maxLabel.Text = metricCard.MaxValue.ToString();
            metricCard._maxEntry.Text = metricCard.MaxValue.ToString();
        }

        private bool SaveValues()
        {
            double min, max;
            if (!double.TryParse(_minEntry.Text, out min) || !double.TryParse(_maxEntry.Text, out max))
                return false;

            if (min >= max)
                return false;

            MinValue = min;
            MaxValue = max;

            return true;
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (!_isEditing || SaveValues())
            {
                _isEditing = !_isEditing;

                _minLabel.IsVisible = !_isEditing;
                _minEntry.IsVisible = _isEditing;

                _maxLabel.IsVisible = !_isEditing;
                _maxEntry.IsVisible = _isEditing;

                _toggleButton.Image = (FileImageSource)(_isEditing ? ImageSource.FromFile("Save.png") : ImageSource.FromFile("Edit.png"));
            }
        }
    }
}
