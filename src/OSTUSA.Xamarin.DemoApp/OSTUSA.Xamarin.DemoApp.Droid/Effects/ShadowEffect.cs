using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Support.V4.Content;
using Android.Graphics.Drawables;
using Android.Graphics;
using Org.Apache.Http;
using Android.Graphics.Drawables.Shapes;
using Android.Content.Res;
using Fx = OSTUSA.XamarinDemo.DemoApp.Effects;

[assembly: ExportEffect(typeof(OSTUSA.XamarinDemo.DemoApp.Droid.Effects.ShadowEffect), "ShadowEffect")]
namespace OSTUSA.XamarinDemo.DemoApp.Droid.Effects
{
    public class ShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                // todo
                ////var control = Control as Android.Widget.TextView;
                //var effect = (OSTUSA.XamarinDemo.DemoApp.Effects.ShadowEffect)Element.Effects.FirstOrDefault(e => e is OSTUSA.XamarinDemo.DemoApp.Effects.ShadowEffect);
                //if (effect != null)
                //{
                //    Container.Background = ContextCompat.GetDrawable(Container.Context, Resource.Drawable.atmo_shadow);

                //    var view = Element as View;
                //    if (view != null)
                //    {
                //        var backgroundColor = view.BackgroundColor;
                //        if (backgroundColor == default(Xamarin.Forms.Color))
                //            backgroundColor = Xamarin.Forms.Color.White;

                //        var layerDrawable = Container.Background as LayerDrawable;
                //        var gradient = layerDrawable.FindDrawableByLayerId(Resource.Id.atmo_shadow_foreground) as GradientDrawable;

                //        gradient.SetColor(backgroundColor.ToAndroid().ToArgb());
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {

        }
    }
}

