using System;

using Xamarin.Forms;
using OSTUSA.IoT.Core.ViewModels;
using System.Windows.Input;

namespace OSTUSA.IoT.DemoApp.PageModels
{
    public class MainPageModel : PageModel
    {
        #region bindables
        
        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            set { SetProperty(ref _temperature, value); }
        }
        
        private double _humidity;
        public double Humidity
        {
            get { return _humidity; }
            set { SetProperty(ref _humidity, value); }
        }
        
        private double _pressure;
        public double Pressure
        {
            get { return _pressure; }
            set { SetProperty(ref _pressure, value); }
        }
        
        public ICommand Refresh { get; private set; }

        #endregion

        public MainPageModel()
        {
            Temperature = 21.93;
            Humidity = 31.404324008388;
            Pressure = 981.460407164082;
        }
        
        #region command implementations
        
        #endregion
        
        #region lifecycle
        
        public override void OnAppearing(object sender, EventArgs e)
        {
            base.OnAppearing(sender, e);
        }
        
        public override void OnDisappearing(object sender, EventArgs e)
        {
            base.OnDisappearing(sender, e);
        }
        
        #endregion
    }
}

