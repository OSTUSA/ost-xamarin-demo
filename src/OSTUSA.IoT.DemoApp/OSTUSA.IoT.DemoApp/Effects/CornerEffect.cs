using System;
using Xamarin.Forms;

namespace OSTUSA.IoT.DemoApp.Effects
{
    public class CornerEffect : RoutingEffect
    {
        public CornerEffect()
            : base("OSTUSA.IoT.DemoApp.Effects.CornerEffect")
        {
            
        }

        public double Radius { get; set; }
    }
}

