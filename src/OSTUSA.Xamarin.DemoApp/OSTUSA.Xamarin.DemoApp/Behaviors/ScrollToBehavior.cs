using System;
using Xamarin.Forms;

namespace OSTUSA.XamarinDemo.DemoApp.Behaviors
{
    public class ScrollToBehavior : Behavior<View>
    {
        private TapGestureRecognizer _tapGestureRecognizer;
        public View AssociatedObject { get; private set; }

        public ScrollView ScrollView { get; set; }
        public Element Target { get; set; }
        public ScrollToPosition Position { get; set; } = ScrollToPosition.Start;

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            AssociatedObject = bindable;

            _tapGestureRecognizer = new TapGestureRecognizer()
            {
                Command = new Command(ScrollToTarget)
            };
            bindable.GestureRecognizers.Add(_tapGestureRecognizer);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            bindable.GestureRecognizers.Remove(_tapGestureRecognizer);
            _tapGestureRecognizer = null;

            AssociatedObject = null;
            
            base.OnDetachingFrom(bindable);
        }

        private void ScrollToTarget()
        {
            if (ScrollView == null || Target == null)
                return;

            ScrollView.ScrollToAsync(Target, Position, true);
        }
    }
}
