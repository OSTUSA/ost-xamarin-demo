using System;
using OSTUSA.XamarinDemo.Core.ViewModels;
namespace OSTUSA.XamarinDemo.DemoApp.ViewModels
{
    public class MetricViewModel : ViewModel
    {
        #region bindables

        private double _minValue;
        public double MinValue
        {
            get { return _minValue; }
            set { SetProperty(ref _minValue, value); }
        }

        private double _maxValue;
        public double MaxValue
        {
            get { return _maxValue; }
            set { SetProperty(ref _maxValue, value); }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        #endregion
    }
}
