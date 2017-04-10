using System;

using Xamarin.Forms;
using OSTUSA.XamarinDemo.Core.ViewModels;
using System.Windows.Input;
using OSTUSA.XamarinDemo.DemoApp.ViewModels;
using OSTUSA.XamarinDemo.Services.Azure;
using System.Threading.Tasks;

namespace OSTUSA.XamarinDemo.DemoApp.PageModels
{
    public class MainPageModel : PageModel
    {
        private const string ThingId = "myraspberrypi";
        private const int IntervalSeconds = 30;

        #region bindables

        #region temperature

        private double _temperatureMin = 15;
        public double TemperatureMin
        {
            get { return _temperatureMin; }
            set { SetProperty(ref _temperatureMin, value); }
        }

        private double _temperatureMax = 27;
        public double TemperatureMax
        {
            get { return _temperatureMax; }
            set { SetProperty(ref _temperatureMax, value); }
        }

        private double _temperatureValue;
        public double TemperatureValue
        {
            get { return _temperatureValue; }
            set { SetProperty(ref _temperatureValue, value); }
        }

        #endregion

        #region humidity

        private double _humidityMin = 0;
        public double HumidityMin
        {
            get { return _humidityMin; }
            set { SetProperty(ref _humidityMin, value); }
        }

        private double _humidityMax = 100;
        public double HumidityMax
        {
            get { return _humidityMax; }
            set { SetProperty(ref _humidityMax, value); }
        }

        private double _humidityValue;
        public double HumidityValue
        {
            get { return _humidityValue; }
            set { SetProperty(ref _humidityValue, value); }
        }

        #endregion

        #region pressure

        private double _pressureMin = 940;
        public double PressureMin
        {
            get { return _pressureMin; }
            set { SetProperty(ref _pressureMin, value); }
        }

        private double _pressureMax = 1020;
        public double PressureMax
        {
            get { return _pressureMax; }
            set { SetProperty(ref _pressureMax, value); }
        }

        private double _pressureValue;
        public double PressureValue
        {
            get { return _pressureValue; }
            set { SetProperty(ref _pressureValue, value); }
        }

        #endregion

        public ICommand Refresh { get; private set; }

        #endregion

        #region constructor

        private readonly IThingsService _thingsService;

        public MainPageModel(
            IThingsService thingsService
        )
        {
            _thingsService = thingsService;

            Refresh = new Command(Perform_Refresh);
        }

        #endregion

        #region command implementations

        private void Perform_Refresh()
        {
            _thingsService.GetThingTwinAsync(ThingId).ContinueWith(twinTask =>
            {
                var twin = twinTask.Result;

                TemperatureValue = Convert.ToDouble(twin.Reported["temperature"]);
                HumidityValue = Convert.ToDouble(twin.Reported["humidity"]);
                PressureValue = Convert.ToDouble(twin.Reported["pressure"]);
            });
        }

        private bool OnInterval()
        {
            Refresh.Execute(null);
            return true;
        }

        #endregion

        #region lifecycle

        public override void OnAppearing(object sender, EventArgs e)
        {
            base.OnAppearing(sender, e);

            Device.StartTimer(TimeSpan.FromSeconds(IntervalSeconds), OnInterval);
            Refresh.Execute(null);
        }

        public override void OnDisappearing(object sender, EventArgs e)
        {
            base.OnDisappearing(sender, e);
        }

        #endregion
    }
}

