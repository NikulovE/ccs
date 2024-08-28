using System;
using System.Collections.Generic;
using System.Text;



#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
#else
#if WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
#endif



#endif
#if XAMARIN
using Xamarin.Forms;
#endif

namespace Shared
{
    class Animations
    {
#if NETFX_CORE
        public static void DropFromLeft(FrameworkElement Element, int mseconds, int from, int to,bool hide = false, EventHandler<object> AnimationCompleted =null)
        {
            Element.RenderTransform = new TranslateTransform();

            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = from;
            doubleAnimation.To = to;
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //doubleAnimation.AutoReverse = true;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(mseconds));
            storyboard.Children.Add(doubleAnimation);
            doubleAnimation.EnableDependentAnimation = true;
            Storyboard.SetTarget(doubleAnimation, Element.RenderTransform);
            Storyboard.SetTargetProperty(doubleAnimation, "X");
            storyboard.Begin();
            if(AnimationCompleted!=null) storyboard.Completed += new EventHandler<object>(AnimationCompleted); 
            storyboard.Completed += (ev, ar) => { Element.RenderTransform = null; };
        }


        public static void DropFromRight(FrameworkElement Element, int mseconds, int from, int to, bool hide = false, EventHandler<object> AnimationCompleted = null)
        {
            Element.RenderTransform = new TranslateTransform();

            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = to;
            doubleAnimation.To = from;
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //doubleAnimation.AutoReverse = true;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(mseconds));
            storyboard.Children.Add(doubleAnimation);
            doubleAnimation.EnableDependentAnimation = true;
            Storyboard.SetTarget(doubleAnimation, Element.RenderTransform);
            Storyboard.SetTargetProperty(doubleAnimation, "X");
            storyboard.Begin();
            if (AnimationCompleted != null) storyboard.Completed += new EventHandler<object>(AnimationCompleted);
            storyboard.Completed += (ev, ar) => { Element.RenderTransform = null; };
        }

        public static void DropFromBottom(FrameworkElement Element, int mseconds, int from, int to)
        {
            Element.RenderTransform = new TranslateTransform();

            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = to;
            doubleAnimation.To = from;
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //doubleAnimation.AutoReverse = true;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(mseconds));
            storyboard.Children.Add(doubleAnimation);
            doubleAnimation.EnableDependentAnimation = true;
            Storyboard.SetTarget(doubleAnimation, Element.RenderTransform);
            Storyboard.SetTargetProperty(doubleAnimation, "Y");
            storyboard.Begin();
            storyboard.Completed += (ev, ar) => { Element.RenderTransform = null; };
        }

        public static void DropFromTop(FrameworkElement Element, int mseconds, int from, int to)
        {
            Element.RenderTransform = new TranslateTransform();

            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = from;
            doubleAnimation.To = to;
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //doubleAnimation.AutoReverse = true;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(mseconds));
            storyboard.Children.Add(doubleAnimation);
            doubleAnimation.EnableDependentAnimation = true;
            Storyboard.SetTarget(doubleAnimation, Element.RenderTransform);
            Storyboard.SetTargetProperty(doubleAnimation, "Y");
            storyboard.Begin();
            storyboard.Completed += (ev, ar) => { Element.RenderTransform = null; };
        }


        public static void Opacity(FrameworkElement Element, int mseconds = 200, int from = 1, int to = 0)
        {

            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = from;
            doubleAnimation.To = to;
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            //doubleAnimation.AutoReverse = true;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(mseconds));
            storyboard.Children.Add(doubleAnimation);
            doubleAnimation.EnableDependentAnimation = true;
            Storyboard.SetTarget(doubleAnimation, Element);
            Storyboard.SetTargetProperty(doubleAnimation, "Opacity");
            storyboard.Begin();
        }
#endif
#if WPF
        public static void DropFromLeft(FrameworkElement Element, int mseconds, int from, int to, bool hide = false, EventHandler AnimationCompleted=null)
        {
            var animation = new ThicknessAnimation();
            if (AnimationCompleted != null) animation.Completed += new EventHandler(AnimationCompleted);
            if (hide)
            {
                Element.Visibility = Visibility.Visible;
                animation.Completed += (ev, ar) =>
                {
                    Element.Visibility = Visibility.Collapsed;
                };
            }
            QuarticEase AniFunc = new QuarticEase { EasingMode = EasingMode.EaseIn };
            animation.FillBehavior = FillBehavior.Stop;
            animation.From = new Thickness(from, 0, 0, 0);
            animation.To = new Thickness(to, 0, 0, 0);
            animation.Duration = TimeSpan.FromMilliseconds(mseconds);
            Element.BeginAnimation(FrameworkElement.MarginProperty, animation);
        }


        public static void DropFromRight(FrameworkElement Element, int mseconds, int from, int to, bool hide=false, EventHandler AnimationCompleted = null) { 

            var animation = new ThicknessAnimation();
            if (AnimationCompleted != null) animation.Completed += new EventHandler(AnimationCompleted);
            if (hide)
            {
                Element.Visibility = Visibility.Visible;
                animation.Completed += (ev, ar) =>
                {
                    Element.Visibility = Visibility.Collapsed;
                };
            }
            QuarticEase AniFunc = new QuarticEase { EasingMode = EasingMode.EaseIn };
            animation.FillBehavior = FillBehavior.Stop;
            animation.EasingFunction = AniFunc;
            animation.From = new Thickness(0, 0, from, 0);
            animation.To = new Thickness(0, 0, to, 0);
            animation.Duration = TimeSpan.FromMilliseconds(mseconds);
            Element.BeginAnimation(FrameworkElement.MarginProperty, animation);


        }

        public static void DropFromBottom(FrameworkElement Element, int mseconds, int from, int to)
        {

            var animation = new ThicknessAnimation();
            animation.From = new Thickness(0, 0, 0, from);
            animation.To = new Thickness(0, 0, 0, to);
            animation.Duration = TimeSpan.FromMilliseconds(mseconds);
            Element.BeginAnimation(FrameworkElement.MarginProperty, animation);
        }

        public static void DropFromTop(FrameworkElement Element, int mseconds, int from, int to)
        {
            var animation = new ThicknessAnimation();
            animation.From = new Thickness(0, from, 0, 0);
            animation.To = new Thickness(0, to, 0, 0);
            animation.Duration = TimeSpan.FromMilliseconds(mseconds);
            Element.BeginAnimation(FrameworkElement.MarginProperty, animation);
        }
        public static void Opacity(FrameworkElement Element, int mseconds = 200, int from = 1, int to = 0)
        {
            if(to==1) Element.Visibility=Visibility.Visible;
            var opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = 0;
            opacityAnimation.To = 1;
            opacityAnimation.FillBehavior = FillBehavior.HoldEnd;
            opacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(mseconds));
            Element.BeginAnimation(FrameworkElement.OpacityProperty, opacityAnimation);
            if (to == 0) Element.Visibility = Visibility.Collapsed;
        }

#endif
#if XAMARIN
        public static void DropFromLeft(Grid Element, int mseconds, int from, int to, bool hide = false, Action AnimationCompleted =null)
        {
            AnimationCompleted.Invoke();

        }


        public static void DropFromRight(Grid Element, int mseconds, int from, int to, bool hide=false, Action AnimationCompleted = null)
        {
            AnimationCompleted.Invoke();
        }

        public static void DropFromBottom(Grid Element, int mseconds, int from, int to)
        {

        }

        public static void DropFromTop(Grid Element, int mseconds, int from, int to)
        {

        }

        public static void Opacity(Grid Element, int mseconds = 200, int from = 1, int to = 0)
        {

            

        }
#endif
    }
}

