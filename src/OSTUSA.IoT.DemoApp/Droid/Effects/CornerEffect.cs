using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Fx = OSTUSA.IoT.DemoApp.Effects;

[assembly: ExportEffect(typeof(OSTUSA.IoT.DemoApp.Droid.Effects.CornerEffect), "CornerEffect")]
namespace OSTUSA.IoT.DemoApp.Droid.Effects
{
    public class CornerEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            SetGradient();

            Element.PropertyChanged += Element_PropertyChanged;
        }

        protected override void OnDetached()
        {

        }

        private void SetGradient()
        {
            try
            {
                var view = Control ?? Container;

                var effect = (Fx.CornerEffect)Element.Effects.FirstOrDefault(e => e is Fx.CornerEffect);
                if (effect != null && view != null)
                {
                    var gradient = view.Background as GradientDrawable;
                    if (gradient == null)
                    {
                        var bgColor = view.Background as ColorDrawable;
                        if (bgColor == null) //todo: improve exception handling
                            return;
                        
                        gradient = new GradientDrawable();
                        gradient.SetColor(bgColor.Color);

                        if (bgColor.Alpha == 0)
                        {
                            // setting the corner radius on a transparent background does not seem to work
                            // setting the corner radius on a background that is white does keep the radius when changing colors
                            gradient.SetColor(Color.White.ToAndroid());
                        }

                        view.SetBackground(gradient);
                    }

                    var displayMetrics = Android.App.Application.Context.Resources.DisplayMetrics;
                    var scaled = (float)(displayMetrics.Density * effect.Radius);

                    gradient.SetCornerRadius(scaled);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: {0}", ex.Message);
            }
        }

        private void Element_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetGradient();
        }
    }
}

