using System;
using Xamarin.Forms;

namespace OSTUSA.XamarinDemo.DemoApp.Effects
{
    public class ShadowEffect : RoutingEffect
    {
        public ShadowEffect()
            : base("OSTUSA.XamarinDemo.DemoApp.Effects.ShadowEffect")
        {

        }

        public Color ShadowColor { get; set; }

        public int ShadowXOffset { get; set; }

        public int ShadowYOffset { get; set; }

        public int ShadowBlur { get; set; }

        public int ShadowSpread { get; set; }
    }
}

