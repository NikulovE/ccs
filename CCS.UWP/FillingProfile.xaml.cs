using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class FillingProfile : Page
    {
        public FillingProfile()
        {
            this.InitializeComponent();
            out1.DataContext = Shared.ModelView.UIBinding.Default;
            NickName.DataContext = Shared.ModelView.UserProfile.Default;
            LoadProfile();
        }

        async void LoadProfile() {
            await Shared.ViewModel.UserProfile.Load();
        }

        private async void CompleteFillingProfile(object sender, RoutedEventArgs e)
        {
            if (FNameBox.Text == "")
            {
                FNameBox.BorderBrush = new SolidColorBrush(Colors.Red);
                Shared.ModelView.UIBinding.Default.OutPut += ConvertMessages.Message("x50005");
                return;
            }
            if (LNameBox.Text == "")
            {
                LNameBox.BorderBrush = new SolidColorBrush(Colors.Red);
                Shared.ModelView.UIBinding.Default.OutPut += ConvertMessages.Message("x50006");
                return;
            }
            if (PhoneBox.Text != "")
            {
                try
                {
                    long.Parse(PhoneBox.Text);
                }
                catch (Exception)
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50008");
                    return;
                }
            }
            if (FNameBox.Text != "" && LNameBox.Text != "")
            {

                if (await Shared.ViewModel.UserProfile.Update())
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(RoleSelector));
                }
                else
                {
                    Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("x50009");
                }
            }
        }


    }
}
