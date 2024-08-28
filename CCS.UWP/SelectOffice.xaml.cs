using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SelectOffice : Page
    {
        public SelectOffice()
        {
            this.InitializeComponent();
        }

        public async void RefreshOffices()
        {
            await Shared.ViewModel.Organization.LoadOffices();

        }

        private async void LeaveSelectedOrganization(object sender, RoutedEventArgs e)
        {
            Shared.ViewModel.Organization.LeaveOrganization();
            await Shared.ViewModel.Organization.LoadOffices();
        }

        private void SelectCreateOffice(object sender, RoutedEventArgs e)
        {

        }

        private async void OfficesLoadd(object sender, RoutedEventArgs e)
        {
            await Shared.ViewModel.Organization.LoadOffices();
            ((ComboBox)sender).SelectedIndex = Shared.ModelView.UIBinding.Default.SelectedOffice;
        }

        private void JoinOrganization(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(JoinOrganization));
        }

        private void Init(object sender, RoutedEventArgs e)
        {

            Shared.ModelView.UserOrganizations.Default.FullAcitivities = Visibility.Collapsed;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await Shared.ViewModel.Organization.LoadOrganizations();
            OrganizationGrid.DataContext = Shared.ModelView.UserOrganizations.Default;

        }

        private void RegisterOrganization(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(RegisterOrganization));
        }

        private void ContinueIntro(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.GoBack();
        }
    }
}
