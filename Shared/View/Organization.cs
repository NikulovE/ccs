
using System;
using System.Collections.Generic;
using System.Text;







#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Maps;
#else
using Bing.Maps;
#endif

#else

#if WPF
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Controls;
using System.Windows;
#endif
#if XAMARIN
using Xamarin.Forms;
using Xamarin.Forms.Maps;
#endif
#endif

namespace Shared.View
{
    class Organization
    {
#if XAMARIN
        public static void ClearGrid(Grid rootGrid) { }
#else
        public static void ClearGrid(Grid rootGrid)
        {
            foreach (var el in rootGrid.Children)
            {
                View.General.HideElement((el as FrameworkElement));
            }
        }
#endif
#if XAMARIN
#else
        public static void SelectCreateCompany(Grid rootGrid,
                                                TextBlock header,
                                                TextBlock PreffereOrgNameGreeting,
                                                TextBox PrefferedOrgName,

                                                Button SendKeyCompRegistration,
                                                Button SendKeyCompJoiner
                                                )
        {
            Shared.View.General.HideElement(SendKeyCompJoiner);


            Shared.View.General.ShowElement(SendKeyCompRegistration);
            Shared.View.General.ShowElement(rootGrid);
            Shared.View.General.ShowElement(PreffereOrgNameGreeting);
            Shared.View.General.ShowElement(PrefferedOrgName);
            header.Text = ConvertMessages.Message("CreateCompany");

        }

        public static void ShowNextThreeElements(FrameworkElement Greeting,
                                                FrameworkElement Name,
                                                FrameworkElement FirstButton)
        {

            Shared.View.General.ShowElement(Greeting);
            Shared.View.General.ShowElement(Name);
            Shared.View.General.ShowElement(FirstButton);
        }

        public static void HideThreeEElements(FrameworkElement Greeting, FrameworkElement Name, FrameworkElement FirstButton)
        {
            Shared.View.General.HideElement(Greeting);
            Shared.View.General.HideElement(Name);
            Shared.View.General.ShowElement(FirstButton);
        }

        public static void SelectCreateOffice(Grid rootGrid)
        {
            foreach (var el in rootGrid.Children)
            {
                (el as FrameworkElement).Visibility = Visibility.Collapsed;
            }


        }
#endif
        public static void ShowListOfOrganizations(List<Model.UserOrganization> OrgList)
        {
            ModelView.UserOrganizations.Default.OrganizationsList.Clear();

            foreach (var org in OrgList)
            {

                if (org.IsOrganization == true)
                {
#if XAMARIN
                    // var nextOrg = new GridLayout();
#else
                    var nextOrg = new ListViewItem();
                    nextOrg.Tag = org.TeamID;
                    nextOrg.Content = org.Name;
#endif

#if NETFX_CORE
                    nextOrg.Tapped += (ev, ar) =>
                    {
                        ModelView.UserOrganizations.Default.FullAcitivities = Visibility.Visible;
                        ModelView.UserOrganizations.Default.SelectedOrganization = org.TeamID;
                        ModelView.UserOrganizations.Default.FullAcitivities = Visibility.Visible;
                    };
#endif
#if WPF
                    nextOrg.Selected += (ev, ar) =>
                    {
                        ModelView.UserOrganizations.Default.FullAcitivities = Visibility.Visible;
                        ModelView.UserOrganizations.Default.SelectedOrganization = org.TeamID;
                        ModelView.UserOrganizations.Default.FullAcitivities = Visibility.Visible;
                    };
#endif
#if XAMARIN
                    ModelView.UserOrganizations.Default.OrganizationsList.Add(org);
#else
                    ModelView.UserOrganizations.Default.OrganizationsList.Add(nextOrg);
#endif

                }
            }

        }

#if WINDOWS_APP || WPF
        public static void ShowListOfOffices(List<Model.OfficeOnMap> OfficeList, MapLayer GlobalMap, bool ShowOnlyUserOffice = false)
        {
            ModelView.UserOrganizations.Default.OfficeList.Clear();

            GlobalMap.Children.Clear();
            var mapLayer = new MapLayer();
            foreach (var office in OfficeList)
            {

                var nextOffice = new ComboBoxItem();
                nextOffice.Content = office.Name;
                nextOffice.Tag = office.ID;
                ModelView.UserOrganizations.Default.OfficeList.Add(nextOffice);


                if (Shared.ModelView.UserProfile.Default.OfficeID == office.ID)
                {
                    Shared.ModelView.UIBinding.Default.SelectedOffice = ModelView.UserOrganizations.Default.OfficeList.IndexOf(nextOffice);
                    var OfficeOnMap = Shared.View.MapsSymbols.Office(office.Name, office.ID, ModelView.UserOrganizations.Default.OfficeList.IndexOf(nextOffice));

                    MapLayer.SetPosition(OfficeOnMap, new Model.Location { Latitude = office.Latitude, Longitude = office.Longtitude });

                    GlobalMap.Children.Add(OfficeOnMap);
                    nextOffice.IsSelected = true;
                }
                else if (!ShowOnlyUserOffice)
                {
                    var OfficeOnMap = Shared.View.MapsSymbols.Office(office.Name, office.ID, ModelView.UserOrganizations.Default.OfficeList.IndexOf(nextOffice));
                    MapLayer.SetPosition(OfficeOnMap, new Model.Location { Latitude = office.Latitude, Longitude = office.Longtitude });
                    GlobalMap.Children.Add(OfficeOnMap);

                }
#if WINDOWS_APP
                nextOffice.Tapped += async (ev, ar) =>
                {
                    Shared.ModelView.UserProfile.Default.OfficeID = int.Parse(nextOffice.Tag.ToString());
                    await Shared.ViewModel.UserProfile.Update();
                    try
                    {
                        Shared.Actions.refreshUserOffice();
                    }
                    catch (Exception) { }
                };
#endif
#if WPF
                nextOffice.Selected += async (ev, ar) =>
                {
                    Shared.ModelView.UserProfile.Default.OfficeID = int.Parse(nextOffice.Tag.ToString());
                    await Shared.ViewModel.UserProfile.Update();
                    try
                    {
                        Shared.Actions.refreshUserOffice();
                    }
                    catch (Exception) { }
                };
#endif

            }
            GlobalMap.Children.Add(mapLayer);


        }
    
#else
        public static void ShowListOfOffices(List<Model.OfficeOnMap> OfficeList, bool ShowOnlyUserOffice = false)
        {
            ModelView.UserOrganizations.Default.OfficeList.Clear();
#if XAMARIN
            Shared.ModelView.UserOrganizations.Default.officelst = OfficeList;
#else

#endif

            foreach (var office in OfficeList)
            {

#if XAMARIN
                ModelView.UserOrganizations.Default.OfficeList.Add(office.Name);               
#else
                var nextOffice = new ComboBoxItem();
                nextOffice.Content = office.Name;
                nextOffice.Tag = office.ID;
                ModelView.UserOrganizations.Default.OfficeList.Add(nextOffice);
#endif




                if (Shared.ModelView.UserProfile.Default.OfficeID == office.ID)
                {
#if XAMARIN
                    Shared.ModelView.UIBinding.Default.SelectedOffice = ModelView.UserOrganizations.Default.OfficeList.IndexOf(office.Name);
#else
                    Shared.ModelView.UIBinding.Default.SelectedOffice = ModelView.UserOrganizations.Default.OfficeList.IndexOf(nextOffice);
                    Shared.ModelView.UIBinding.Default.OfficeOnMap = new Model.Location { Latitude = office.Latitude, Longitude = office.Longtitude };
#endif
#if XAMARIN

#else

                    nextOffice.IsSelected = true;
#endif

                }
                else if (!ShowOnlyUserOffice)
                {


                }
#if NETFX_CORE
                nextOffice.Tapped += async (ev, ar) =>
                {
                    Shared.ModelView.UserProfile.Default.OfficeID = int.Parse(nextOffice.Tag.ToString());
                    await Shared.ViewModel.UserProfile.Update();
                    try
                    {
                        Shared.Actions.refreshUserOffice();
                    }
                    catch (Exception) { }
                };
#else
#endif

            }


        }

#endif


    }
}
