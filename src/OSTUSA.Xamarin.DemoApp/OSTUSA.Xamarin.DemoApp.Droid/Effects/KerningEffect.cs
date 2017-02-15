using System;
using System.Linq;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Fx = OSTUSA.XamarinDemo.DemoApp.Effects;

[assembly: ExportEffect(typeof(OSTUSA.XamarinDemo.DemoApp.Droid.Effects.KerningEffect), "KerningEffect")]
namespace OSTUSA.XamarinDemo.DemoApp.Droid.Effects
{
    public class KerningEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
                // before Lollipop (5.0), setLetterSpacing() was not supported by Android
                return;
            
            var effect = (Fx.KerningEffect)Element.Effects.FirstOrDefault(x => x is Fx.KerningEffect);
            if (effect == null)
                return;

            var textView = Control as TextView;
            if (textView == null)
                return;

            var label = Element as Label;
            if (label == null)
                return;

            textView.LetterSpacing = (float)(effect.CharacterSpacing / label.FontSize);
        }

        protected override void OnDetached()
        {

        }
    }
}

