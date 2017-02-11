using Xamarin.Forms;
using OSTUSA.IoT.DemoApp.Views;
using OSTUSA.IoT.DemoApp.ViewModels;

namespace OSTUSA.IoT.DemoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new RemotePage()
            {
                BindingContext = new RemotePageModel()
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
