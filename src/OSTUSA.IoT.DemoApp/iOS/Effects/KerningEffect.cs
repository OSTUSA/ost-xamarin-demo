using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Linq;
using UIKit;
using Foundation;
using Fx = OSTUSA.IoT.DemoApp.Effects;

[assembly: ExportEffect(typeof(Amway.ConEx.Services.Client.Effects.KerningEffect), "KerningEffect")]
namespace Amway.ConEx.Services.Client.Effects
{
    public class KerningEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var effect = (Fx.KerningEffect)Element.Effects.FirstOrDefault(x => x is Fx.KerningEffect);
            if (effect == null)
                return;

            if (Control is UILabel)
            {
                var label = Control as UILabel;

                if (label.AttributedText != null)
                {
                    NSRange effectiveRange;
                    var attr = new NSMutableDictionary(label.AttributedText.GetAttributes(0, out effectiveRange));
                    attr.SetValueForKey(new NSNumber(effect.CharacterSpacing), UIStringAttributeKey.KerningAdjustment);

                    label.AttributedText = new NSAttributedString(label.AttributedText.Value, attr);
                }
            }
            else if (Control is UITextView)
            {
                var textView = Control as UITextView;

                if (textView.AttributedText != null)
                {
                    NSRange effectiveRange;
                    var attr = new NSMutableDictionary(textView.AttributedText.GetAttributes(0, out effectiveRange));
                    attr.SetValueForKey(new NSNumber(effect.CharacterSpacing), UIStringAttributeKey.KerningAdjustment);

                    textView.AttributedText = new NSAttributedString(textView.AttributedText.Value, attr);
                }
            }
        }

        protected override void OnDetached()
        {
            
        }
    }
}

