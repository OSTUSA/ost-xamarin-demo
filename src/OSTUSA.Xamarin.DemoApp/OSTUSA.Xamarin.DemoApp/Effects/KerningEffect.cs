using System;
using Xamarin.Forms;
using System.Linq;
namespace OSTUSA.XamarinDemo.DemoApp.Effects
{
    public class KerningEffect : RoutingEffect
    {
        public KerningEffect()
            : base("OSTUSA.XamarinDemo.DemoApp.Effects.KerningEffect")
        {

        }

        public double CharacterSpacing { get; set; }

        public static readonly BindableProperty CharacterSpacingProperty = BindableProperty.CreateAttached("CharacterSpacing", typeof(double), typeof(KerningEffect), 0d, propertyChanged: OnCharacterSpacingPropertyChanged);
        private static void OnCharacterSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var visualElement = bindable as VisualElement;
            if (visualElement == null)
                return;

            var kerningEffect = (KerningEffect)visualElement.Effects.SingleOrDefault(x => x is KerningEffect);
            if (kerningEffect == null)
            {
                kerningEffect = new KerningEffect();
                visualElement.Effects.Add(kerningEffect);
            }

            kerningEffect.CharacterSpacing = (double)bindable.GetValue(CharacterSpacingProperty);
        }
    }
}
