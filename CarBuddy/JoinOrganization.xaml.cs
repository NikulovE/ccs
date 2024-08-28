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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CCS.WinStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JoinOrganization : Page
    {
        public JoinOrganization()
        {
            this.InitializeComponent();
            JoinOrgGrid.DataContext = Shared.ModelView.UIBinding.Default;
            Shared.ModelView.UIBinding.Default.OutPut = "";
        }

        private void ContinueIntro(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.GoBack();
            }
            catch (Exception) {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
            }
        }

        private async void StartIntroJoinOrg(object sender, RoutedEventArgs e)
        {
            if (await Shared.ViewModel.Organization.StartJoin(IntroOrgBox.Text.Replace(" ", "")))
            {
                Shared.View.General.HideElement(OrgJoinIntro);
                Shared.View.General.ShowElement(OrgJoinConfrimationStack);
            }
            else
            {
                OrgRegistration.Visibility = Visibility.Visible;
            }
        }


        private async void ConfirmIntroOrgJoin(object sender, RoutedEventArgs e)
        {
            if (await Shared.ViewModel.Organization.CompleteJoiner(IntroOrgBox.Text.Split('@')[1], Passbox.Password.Replace(" ", "")))
            {
                Shared.View.General.HideElement(ManualJoinOrgStackPanel);
                Shared.View.General.ShowElement(OrgJoinComplete);
                Shared.View.General.ShowElement(SuccessStoryLabel);
                await Shared.ViewModel.Organization.LoadOrganizations();
                //await Shared.ViewModel.Organization.LoadOffices();
            };
        }

        private void RegisterOrganizations(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(RegisterOrganization));
        }
    }
    }
