using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Fx = OSTUSA.IoT.DemoApp.Effects;

[assembly: ExportEffect(typeof(OSTUSA.IoT.DemoApp.iOS.Effects.CornerEffect), "CornerEffect")]
namespace OSTUSA.IoT.DemoApp.iOS.Effects
{
    public class CornerEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var effect = (Fx.CornerEffect)Element.Effects.FirstOrDefault(e => e is Fx.CornerEffect);
                var view = Control ?? Container;
                if (effect != null && view != null)
                {
                    view.Layer.CornerRadius = (float)effect.Radius;
                    view.Layer.MasksToBounds = !Element.Effects.Any(x => x is Fx.ShadowEffect);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: {0}", ex.Message);
            }
        }

        protected override void OnDetached()
        {

        }
    }
}

