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
    public sealed partial class RegisterOrganization : Page
    {
        public RegisterOrganization()
        {
            this.InitializeComponent();
            RegisterGrid.DataContext = Shared.ModelView.UIBinding.Default;
        }

        private async void StartRegisterOrganization(object sender, RoutedEventArgs e)
        {
            if (await Shared.ViewModel.Organization.Registration(WorkMail.Text.Replace(" ", ""), ExtendOrgPrefferedName.Text)) {
                Shared.View.General.HideElement(OrgJoinIntro);
                Shared.View.General.ShowElement(OrgJoinConfrimationStack);
            }
        }

        private async void ConfirmOrgCreation(object sender, RoutedEventArgs e)
        {
            if (await Shared.ViewModel.Organization.Confirmation(WorkMail.Text.Split('@')[1], Passbox.Password.Replace(" ", "")))
            {
                Shared.View.General.HideElement(ManualJoinOrgStackPanel);
                Shared.View.General.ShowElement(OrgJoinComplete);
                Shared.View.General.ShowElement(SuccessStoryLabel);
                await Shared.ViewModel.Organization.LoadOrganizations();
                await Shared.ViewModel.Organization.LoadOffices();
            };
        }

        private void ContinueIntro(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.GoBack();
        }
    }
}
