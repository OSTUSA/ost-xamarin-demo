using System;
using System.Linq;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Fx = OSTUSA.IoT.DemoApp.Effects;

[assembly: ExportEffect(typeof(OSTUSA.IoT.DemoApp.iOS.Effects.ShadowEffect), "ShadowEffect")]
namespace OSTUSA.IoT.DemoApp.iOS.Effects
{
    public class ShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var effect = (Fx.ShadowEffect)Element.Effects.FirstOrDefault(e => e is Fx.ShadowEffect);
                var view = Control ?? Container;
                if (effect != null && view != null)
                {
                    view.Layer.MasksToBounds = false;

                    view.Layer.ShadowColor = effect.ShadowColor.ToCGColor();
                    view.Layer.ShadowRadius = effect.ShadowBlur;
                    view.Layer.ShadowOffset = new CGSize(effect.ShadowXOffset, effect.ShadowYOffset);
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

