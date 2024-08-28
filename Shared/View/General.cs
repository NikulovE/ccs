
using System;
using System.Collections.Generic;
using System.Text;




#if NETFX_CORE
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
#else
#if WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
#endif


#if XAMARIN
using Xamarin.Forms;
#endif
#endif


namespace Shared.View
{
    class General
    {
#if XAMARIN
#else
        public static FontFamily SetSegoeMDLFont()
        {

#if WPF
            return new FontFamily(new Uri("pack://application:,,,/"), "./resources/#Segoe MDL2 Assets");
#else

            return new FontFamily("Segoe MDL2 Assets");
#endif



        }

#endif

        public static void DisableButton(Button But)
        {

            But.IsEnabled=false;
        }

        public static void EnableButton(Button But)
        {
            But.IsEnabled = true;
        }
        

        public static void inLoading()
        {
            ModelView.UIBinding.Default.isOnLoading = true;
            ModelView.UIBinding.Default.OutPut = "";
        }

        public static void outLoading()
        {
            ModelView.UIBinding.Default.isOnLoading = false;
        }
#if NETFX_CORE
        public static void ShowNextRegistrationGrid(Grid FromGrid, EventHandler<object> NextAction)
#else
#if XAMARIN
        public static void ShowNextRegistrationGrid(Grid FromGrid, Action NextAction)
#else
        public static void ShowNextRegistrationGrid(Grid FromGrid, EventHandler NextAction)
#endif
#endif
        {
            ModelView.UIBinding.Default.OutPut = "";
            Shared.Animations.DropFromRight(FromGrid, 300, 0, 4000, true, NextAction);


        }
#if NETFX_CORE
        public static void ShowPreviousRegistrationGrid(Grid FromGrid, EventHandler<object> NextAction)
#else
#if XAMARIN
        public static void ShowPreviousRegistrationGrid(Grid FromGrid, Action NextAction)
#else
        public static void ShowPreviousRegistrationGrid(Grid FromGrid, EventHandler NextAction)
#endif
#endif
        {
            ModelView.UIBinding.Default.OutPut = "";
            Shared.Animations.DropFromLeft(FromGrid, 300, 0, 4000, true, NextAction);


        }

#if XAMARIN
        public static void HideLeftMenu(int MenuWithAnimation, List<Grid> LeftMenu) { }
#else
        public static void HideLeftMenu(Grid MenuWithAnimation, List<FrameworkElement> LeftMenu){



            ModelView.UIBinding.Default.OutPut = "";
            View.General.ShowElement(MenuWithAnimation);
            Shared.Animations.DropFromRight(MenuWithAnimation, 300, -300, 0, true);
            View.General.HideElements(LeftMenu);

        }
#endif

#if XAMARIN
        public static void ShowLeftMenu(int MenuWithAnimation, List<Grid> LeftMenu) {

        }

#else
        public static void ShowLeftMenu(Grid MenuWithAnimation, List<FrameworkElement> LeftMenu)

        {
            ModelView.UIBinding.Default.OutPut = "";
            View.General.HideElements(LeftMenu);
            View.General.ShowElement(MenuWithAnimation);
            Shared.Animations.DropFromLeft(MenuWithAnimation, 150, -300, 0);

        }
#endif
#if XAMARIN
        public static void ShowElement(int Element) {
#else
         public static void ShowElement(FrameworkElement Element){
#endif

#if XAMARIN
            //Element.IsVisible = true;
#else
            Element.Visibility = Visibility.Visible;
#endif
        }
#if XAMARIN
        public static void HideElement(Grid Element)
#else
        public static void HideElement(FrameworkElement Element)
#endif
        {
#if XAMARIN
            Element.IsVisible = false;
#else
            Element.Visibility = Visibility.Collapsed;
#endif
        }
#if XAMARIN
        public static void CleanValue(Label Txt) { 
#else
        public static void CleanValue(TextBlock Txt){
#endif
       
            Txt.Text = "";
        }
#if XAMARIN
#else
        public static void CleanValue(PasswordBox Txt){
            Txt.Password = "";
         }
#endif


#if XAMARIN
        public static void CleanValue(Entry Txt) { 
#else
        public static void CleanValue(TextBox Txt){
#endif
            Txt.Text = "";
        }
#if XAMARIN
        public static void ShowElements(List<Grid> elements) { }
#else
        public static void ShowElements(List<FrameworkElement> elements)

        {
            foreach (var el in elements)
            {
                View.General.ShowElement(el);

            }
        }
#endif
#if XAMARIN
        public static void HideElements(List<Grid> elements) { }
#else
        public static void HideElements(List<FrameworkElement> elements)

        {
            foreach (var el in elements)
            {
                View.General.HideElement(el);
            }
        }
#endif

#if XAMARIN
        public static void UnCheckButton(List<Switch> elements, Switch ExceptButton)
        {
            foreach (var el in elements)
            {

                if (el != ExceptButton) el.IsToggled = false;
            }
        }
#endif
#if WINDOWS || NETFX_CORE || WPF
        public static void UnCheckButton(List<ToggleButton> elements, ToggleButton ExceptButton)
        {
            foreach (var el in elements)
            {

                if (el != ExceptButton) el.IsChecked = false;
        }
        



    }
#endif
        }
}
