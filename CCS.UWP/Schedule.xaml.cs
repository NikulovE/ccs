using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CCS.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Schedule : Page
    {
        public Schedule()
        {
            this.InitializeComponent();
            
            
        }


        bool ScheduleLoaded = false;
        //private async void UpdateSchedule(object sender, RoutedEventArgs e)
        //{
        //    if(ScheduleLoaded) await Shared.ViewModel.UserSchedule.Update();
        //}

        //private async void UpdateSchedule(object sender, TimePickerValueChangedEventArgs e)
        //{
        //    if (ScheduleLoaded)  await Shared.ViewModel.UserSchedule.Update();
        //}

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await Shared.ViewModel.UserSchedule.Load(WeeklySchedulesStack);
            ScheduleLoaded = true;
            ScheduleGrid.DataContext = Shared.ModelView.UserSchedule.Default;
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
        }

    }
}
