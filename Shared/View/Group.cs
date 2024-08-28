using System;
using System.Collections.Generic;
using System.Text;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
#if WPF
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
#endif
#endif


namespace Shared.View
{
    class Group
    {
#if XAMARIN

#else
        public static void ShowGroupsVisibility(List<Model.UserOrganization> OrgList)
        {
            ModelView.UserOrganizations.Default.VisibilityList.Items.Clear();
            foreach (var org in OrgList)
            {
#if NETFX_CORE
                 var newOrg = new ToggleSwitch();


                newOrg.VerticalContentAlignment = VerticalAlignment.Stretch;
                newOrg.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                //newOrg.Style = Application.Current.FindResource("AreaOfView") as Style;
                //else newOrg.Content = org.Name;
                newOrg.IsOn = org.IsVisible;
                newOrg.OnContent = org.Name + "\n" + ConvertMessages.Message("Show");
                newOrg.OffContent = org.Name + "\n" + ConvertMessages.Message("Hide");

                newOrg.Loaded += (ev, ar) =>
                {
                    newOrg.Toggled += (evx, arx) =>
                    {
                        if (((ToggleSwitch)evx).IsOn == true) ViewModel.Organization.ChangeVisibility(org.TeamID, true, org.IsOrganization);
                        else ViewModel.Organization.ChangeVisibility(org.TeamID, false, org.IsOrganization);
                    };
                };

                // var s=org as ToggleButton();
                ModelView.UserOrganizations.Default.VisibilityList.Items.Add(newOrg);
#endif
            }
        }
#endif
        }
}
