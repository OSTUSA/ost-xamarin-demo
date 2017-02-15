using System;
using Xamarin.Forms;

namespace OSTUSA.XamarinDemo.DemoApp.Effects
{
    public class CornerEffect : RoutingEffect
    {
        public CornerEffect()
            : base("OSTUSA.XamarinDemo.DemoApp.Effects.CornerEffect")
        {
            
        }

        public double Radius { get; set; }
    }
}

