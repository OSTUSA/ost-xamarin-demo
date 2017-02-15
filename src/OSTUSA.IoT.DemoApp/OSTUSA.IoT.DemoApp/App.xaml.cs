﻿using Xamarin.Forms;
using OSTUSA.IoT.DemoApp.Bootstrapping;

namespace OSTUSA.IoT.DemoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();
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
