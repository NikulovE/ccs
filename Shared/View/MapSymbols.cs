using System;
using System.Collections.Generic;
using System.Text;
#if NETFX_CORE
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls.Maps;
#else
using Bing.Maps;
#endif

using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Popups;
using Windows.Foundation;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;
#else

#if WPF
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Media;
using System.Windows.Shapes;
#endif

#if XAMARIN
using Xamarin.Forms.Maps;
#endif

#endif

namespace Shared.View
{
    class MapsSymbols
    {
#if XAMARIN

        public static async void ShowCompanions(Map Maplayer, List<Model.OnMapPoint> Points)
        {

            {
                foreach (var point in Points)
                {
                    var Pos = new Position(point.Latitude, point.Longtitude);
                    var PointOnMap = new Pin { Position = Pos };
                    
                    if (point.IsHome)
                    {
                        PointOnMap.Clicked += async (ev, ar) =>
                        {
                            await ViewModel.Trips.Send(point.UID, 0, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome);
                        };

                        PointOnMap.Label = "Home. Click to send invite";
                        PointOnMap.Address = await CompanionTooltipAsync(point.UID);

                        Maplayer.Pins.Add(PointOnMap);
                    }
                    else
                    {
                        PointOnMap.Clicked += async (ev, ar) =>
                        {
                            await ViewModel.Trips.Send(point.UID, point.PointID, Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome);
                        };
                        PointOnMap.Label = "Click to send invite";
                        PointOnMap.Address = await CompanionTooltipAsync(point.UID);
                        Maplayer.Pins.Add(PointOnMap);
                    }
                }
            }
        }

        static async System.Threading.Tasks.Task<string> CompanionTooltipAsync(int UID)
        {
            var user = await ViewModel.Companions.GetCompanionInfo(UID);
            var ToolTipPanel = "";
            var Name = user.FirstName + " " + user.LastName+" ";
            ToolTipPanel += Name;

            if (!Shared.ModelView.UserProfile.Default.IsDriver)
            {
                var CarInfo = user.Brand + " " + user.Model + " | " + ConvertMessages.Message("Places") + ":" + user.Places.ToString()+' ';
                ToolTipPanel+=CarInfo;
            }
            var Payable = PaymentConverter(user.Payment);
            ToolTipPanel+=Payable;
            return ToolTipPanel;

        }
#else
        public static FrameworkElement Office(String OfficeName, int OfficeID = 0, int IndexInBox = -1)
        {

            var Office = new StackPanel();
            //if(set)Office.Margin = new Thickness(-10, -75, 0, 0);
            //else Office.Margin = new Thickness(-12, -19, 0, 0);
            Office.Orientation = Orientation.Horizontal;

            var CombinedSymbols = new Grid();
            CombinedSymbols.Width = 23;
            CombinedSymbols.Height = 23;
            var OfficeSymbol = new TextBlock();

            OfficeSymbol.Text = "\uEB4E";
            OfficeSymbol.FontFamily = General.SetSegoeMDLFont();
            OfficeSymbol.Foreground = new SolidColorBrush(Colors.White);
            OfficeSymbol.HorizontalAlignment = HorizontalAlignment.Center;
            OfficeSymbol.VerticalAlignment = VerticalAlignment.Center;

            var El = new Ellipse();
            El.Stroke = new SolidColorBrush(Colors.White);
            El.Fill = new SolidColorBrush(Colors.Orange);

            CombinedSymbols.Children.Add(El);
            CombinedSymbols.Children.Add(OfficeSymbol);



            Office.Children.Add(CombinedSymbols);

            var officeName = new TextBlock();
            officeName.FontSize = 16;
            officeName.FontWeight = FontWeights.ExtraBold;
            officeName.Margin = new Thickness(3, 0, 0, 0);
            officeName.Text = OfficeName;
            officeName.VerticalAlignment = VerticalAlignment.Center;
            Office.Children.Add(officeName);
            if (OfficeID != 0)
            {

#if NETFX_CORE
                var cMenu = new PopupMenu();
                cMenu.Commands.Add(new UICommand(ConvertMessages.Message("ComplaintWrongOffice"), (command) =>
                {
                    ViewModel.Complaint.WrongOffice(OfficeID);

                }));

                Office.PointerPressed += async (a, rt) =>
                {
                    try
                    {
                        Shared.ModelView.UserProfile.Default.OfficeID = OfficeID;
                        await Shared.ViewModel.UserProfile.Update();
                        await cMenu.ShowForSelectionAsync(GetElementRect(CombinedSymbols));
                        Shared.ModelView.UIBinding.Default.SelectedOffice = IndexInBox;
                        Actions.checkWizard();
                        Actions.refreshUserOffice();
                    }
                    catch (Exception) { }
                };

#endif
#if WPF
                Office.PreviewMouseDown += async (ev, ar) =>
                {
                    if (ModelView.UserProfile.Default.OfficeID != OfficeID)
                    {
                        ModelView.UserProfile.Default.OfficeID = OfficeID;
                        await ViewModel.UserProfile.Update();
                        ModelView.UIBinding.Default.SelectedOffice = IndexInBox;
                        Actions.checkWizard();
                    }
                };
                ContextMenu cMenu = new ContextMenu();


                MenuItem WrongOfficeCompliant = new MenuItem();
                WrongOfficeCompliant.Header = ConvertMessages.Message("ComplaintWrongOffice");
                WrongOfficeCompliant.Click += (obj, args) =>
                {
                    ViewModel.Complaint.WrongOffice(OfficeID);
                };
                cMenu.Items.Add(WrongOfficeCompliant);
                Office.ContextMenu = cMenu;
#endif

            }
            return Office;
        }

        public static FrameworkElement NewOffice()
        {

            var Office = new StackPanel();
            //if(set)Office.Margin = new Thickness(-10, -75, 0, 0);
            //else Office.Margin = new Thickness(-12, -19, 0, 0);
            Office.Orientation = Orientation.Horizontal;

            var CombinedSymbols = new Grid();
            CombinedSymbols.Width = 23;
            CombinedSymbols.Height = 23;
            var OfficeSymbol = new TextBlock();

            OfficeSymbol.Text = "\uEB4E";
            OfficeSymbol.FontFamily = General.SetSegoeMDLFont();
            OfficeSymbol.Foreground = new SolidColorBrush(Colors.White);
            OfficeSymbol.HorizontalAlignment = HorizontalAlignment.Center;
            OfficeSymbol.VerticalAlignment = VerticalAlignment.Center;

            var El = new Ellipse();
            El.Stroke = new SolidColorBrush(Colors.White);
            El.Fill = new SolidColorBrush(Colors.Orange);

            CombinedSymbols.Children.Add(El);
            CombinedSymbols.Children.Add(OfficeSymbol);



            Office.Children.Add(CombinedSymbols);

            var officeName = new TextBlock();
            //officeName.DataContext = ModelView.UserOrganizations.Default;
            officeName.FontSize = 16;
            officeName.FontWeight = FontWeights.ExtraBold;
            officeName.Margin = new Thickness(3, 0, 0, 0);
            //officeName.Text = OfficeName;


            var binding = new Binding();
            binding.Path = new PropertyPath("NewOfficeName");
            binding.Source = Shared.ModelView.UserOrganizations.Default;
            //binding.NotifyOnTargetUpdated = true;

            BindingOperations.SetBinding(officeName, TextBlock.TextProperty, binding);

            //officeName.Effect =
            //new DropShadowEffect
            //{
            //    Color = new Color { A = 255, R = 255, G = 255, B = 255 },
            //    Direction = 320,
            //    ShadowDepth = 1,
            //    Opacity = 1
            //};
            officeName.VerticalAlignment = VerticalAlignment.Center;
            Office.Children.Add(officeName);
            return Office;
        }

        public static FrameworkElement TripPoint(Model.TripOffer Trip)
        {

            var TripPoint = new StackPanel();
            //if(set)Office.Margin = new Thickness(-10, -75, 0, 0);
            //else Office.Margin = new Thickness(-12, -19, 0, 0);
            TripPoint.Orientation = Orientation.Horizontal;

            var CombinedSymbols = new Grid
            {
                Width = 23,
                Height = 23
            };
            var TripFinalSymbol = new TextBlock
            {
                Text = "\uEB4B",
                Foreground = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            TripFinalSymbol.FontFamily = General.SetSegoeMDLFont();
            var El = new Ellipse
            {
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.AliceBlue)
            };

            CombinedSymbols.Children.Add(El);
            CombinedSymbols.Children.Add(TripFinalSymbol);

            TripPoint.Loaded += async (ev, ar) =>
            {
                var companionInfo = await ViewModel.Companions.GetCompanionInfo(Trip.CompanionID);
                var Tooltip = new ToolTip();
                Tooltip.Content = companionInfo.FirstName + " " + companionInfo.LastName + '\n' +
                    ConvertMessages.Message("Phone") + ":" + companionInfo.Phone + '\n' +
                    PaymentConverter(companionInfo.Payment) + '\n' +
                    companionInfo.Brand + " " + companionInfo.Model + " " + companionInfo.GovNumber
                + '\n' + new DateTime(Trip.StartTime).ToString();
                ToolTipService.SetToolTip(TripPoint, Tooltip);


            };


            TripPoint.Children.Add(CombinedSymbols);

            return TripPoint;
        }


#if WINDOWS_UWP
        public static FrameworkElement RoutePoint(bool RoutePointType, MapControl Map, SolidColorBrush Brush)
#else
        public static FrameworkElement RoutePoint(bool RoutePointType, MapLayer Map, Brush Brush)
#endif
        {
            var CombinedSymbols = new Grid
            {
                Margin = new Thickness(-5, -5, 0, 0),
                Width = 25,
                Height = 25
            };


            var Arrow = new TextBlock();
            //Arrow.FontWeight = FontWeights.ExtraBold;
            Arrow.Foreground = new SolidColorBrush(Colors.White);
            Arrow.HorizontalAlignment = HorizontalAlignment.Center;
            Arrow.VerticalAlignment = VerticalAlignment.Center;
            Arrow.FontFamily = General.SetSegoeMDLFont();
            
            if (RoutePointType == false) { Arrow.Text = "\uE7EA"; }
            if (RoutePointType == true) { Arrow.Text = "\uEA62"; }
            Arrow.Margin = new Thickness(0, 11, 0, 0);

            var RoutePointSymbol = new TextBlock();
            //RoutePointSymbol.FontWeight = FontWeights.ExtraBold;
            RoutePointSymbol.Foreground = new SolidColorBrush(Colors.White);
            RoutePointSymbol.HorizontalAlignment = HorizontalAlignment.Center;
            RoutePointSymbol.VerticalAlignment = VerticalAlignment.Top;
            RoutePointSymbol.Margin = new Thickness(0, 4, 0, 0);
            RoutePointSymbol.FontFamily = General.SetSegoeMDLFont();
            if (RoutePointType == false) { RoutePointSymbol.Text = "\uE821"; }
            if (RoutePointType == true) { RoutePointSymbol.Text = "\uE80F"; }

            var El = new Ellipse();
            El.Stroke = new SolidColorBrush(Colors.White);
            El.Fill = Brush;

            CombinedSymbols.Children.Add(El);
            CombinedSymbols.Children.Add(Arrow);
            CombinedSymbols.Children.Add(RoutePointSymbol);

#if NETFX_CORE
            var cMenu = new PopupMenu();

            cMenu.Commands.Add(new UICommand(ConvertMessages.Message("Remove"), async (command) =>

            {

                if (await ViewModel.RoutePoints.Change(int.Parse(CombinedSymbols.Tag.ToString()), -1))
                {
                    Map.Children.Remove(CombinedSymbols);
                }

            }));


            CombinedSymbols.PointerPressed += async (a, rt) =>
            {
                await cMenu.ShowForSelectionAsync(GetElementRect(CombinedSymbols));
            };

#endif
#if WPF
            ContextMenu cMenu = new ContextMenu();


            MenuItem remove = new MenuItem();
            remove.Header = ConvertMessages.Message("Remove");
            remove.Click += async (obj, args) =>
            {
                try
                {
                    if (await ViewModel.RoutePoints.Change(int.Parse(CombinedSymbols.Tag.ToString()), -1))
                    {
                        Map.Children.Remove(CombinedSymbols);
                    }
                }
                catch { }

            };


            cMenu.Items.Add(remove);
            CombinedSymbols.PreviewMouseDown += (obj, arg) =>
            {
                cMenu.IsOpen = true;
            };
            CombinedSymbols.ContextMenu = cMenu;
#endif
            return CombinedSymbols;
        }

#if NETFX_CORE
        public static Rect GetElementRect(FrameworkElement element)

        {

            GeneralTransform buttonTransform = element.TransformToVisual(null);

            Point point = buttonTransform.TransformPoint(new Point());

            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));

        }
#endif
#if WINDOWS_UWP
        public static void ShowCompanions(MapControl Maplayer, List<Model.OnMapPoint> Points)
#else
        public static void ShowCompanions(MapLayer Maplayer, List<Model.OnMapPoint> Points)

#endif


        {
            foreach (var point in Points)
            {
                if (point.IsHome)
                {

                    var CompanionHouseS = CompanionHouse(point.UID);
#if WINDOWS_UWP
                    MapControl.SetLocation(CompanionHouseS, new Model.Location { Latitude = point.Latitude, Longitude = point.Longtitude });
#else
                    MapLayer.SetPosition(CompanionHouseS, new Model.Location { Latitude = point.Latitude, Longitude = point.Longtitude });
#endif

                    Maplayer.Children.Add(CompanionHouseS);
                }
                else
                {
                    var CompanionRoutePointS = CompanionRoutePoint(point.UID, point.PointID);
#if WINDOWS_UWP
                    MapControl.SetLocation(CompanionRoutePointS, new Model.Location { Latitude = point.Latitude, Longitude = point.Longtitude });
#else
                    MapLayer.SetPosition(CompanionRoutePointS, new Model.Location { Latitude = point.Latitude, Longitude = point.Longtitude });
#endif

                    Maplayer.Children.Add(CompanionRoutePointS);
                }
            }
            if (Points.Count == 0) Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message("noresults");
        }

        public static FrameworkElement CompanionHouse(int UID)
        {

            var CombinedSymbols = new Grid();
            CombinedSymbols.Margin = new Thickness(-13, -13.5, 0, 0);
            CombinedSymbols.Width = 25;
            CombinedSymbols.Height = 25;
            var HouseSymbol = new TextBlock();


            HouseSymbol.FontFamily = General.SetSegoeMDLFont();
            HouseSymbol.FontWeight = FontWeights.ExtraBold;

            // CCS.Classic; component / Resources /#Segoe UI Symbol
            HouseSymbol.Foreground = new SolidColorBrush(Colors.White);
            HouseSymbol.HorizontalAlignment = HorizontalAlignment.Center;
            HouseSymbol.VerticalAlignment = VerticalAlignment.Center;

            var El = new Ellipse();
            El.Stroke = new SolidColorBrush(Colors.White);
            El.Fill = new SolidColorBrush(Colors.DodgerBlue);
            HouseSymbol.Text = "\uEA8A";

            CombinedSymbols.Children.Add(El);
            CombinedSymbols.Children.Add(HouseSymbol);


            var Tooltip = new ToolTip();
            Tooltip.Content = "";
            CombinedSymbols.Loaded += async (ev, ar) =>
            {
                //if (UID == 163999797)
                //{
                //    var s = 0;
                //}
                var companion = await ViewModel.Companions.GetCompanionInfo(UID);
                //if (UID == 163999797) {
                //    var s = 0;
                //}
                Tooltip.Content = CompanionTooltip(companion);
                ToolTipService.SetToolTip(CombinedSymbols, Tooltip);

            };

#if NETFX_CORE
            var cMenu = new PopupMenu();
            if (Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome)
            {

                cMenu.Commands.Add(new UICommand(Shared.ModelView.UserProfile.Default.IsDriver ? ConvertMessages.Message("Offer") + " " + ConvertMessages.Message("ToHome") : ConvertMessages.Message("Drive") + " " + ConvertMessages.Message("ToHome"), async (command) =>
                {
                    await ViewModel.Trips.Send(UID, 0, true);

                }));
            }
            else
            {
                cMenu.Commands.Add(new UICommand(Shared.ModelView.UserProfile.Default.IsDriver ? ConvertMessages.Message("Offer") + " " + ConvertMessages.Message("ToWork") : ConvertMessages.Message("Drive") + " " + ConvertMessages.Message("ToWork"), async (command) =>
                {
                    await ViewModel.Trips.Send(UID, 0, false);

                }));
            }

            CombinedSymbols.PointerPressed += async (a, rt) =>
            {
                await cMenu.ShowForSelectionAsync(GetElementRect(CombinedSymbols));
            };
#endif
#if WPF
            ContextMenu cMenu = new ContextMenu();


            MenuItem SendInviteMenu = new MenuItem();

            if (ModelView.UserProfile.Default.IsDriver) SendInviteMenu.Header = ConvertMessages.Message("Offer");
            else SendInviteMenu.Header = ConvertMessages.Message("Drive");


            var toHome = new MenuItem();
            toHome.Header = ConvertMessages.Message("ToHome");
            toHome.Click += async (obj, args) =>
            {
                await ViewModel.Trips.Send(UID, 0, true);
            };

            var toWork = new MenuItem();
            toWork.Header = ConvertMessages.Message("ToWork");
            toWork.Click += async (obj, args) =>
            {
                await ViewModel.Trips.Send(UID, 0, false);
            };
            if (ModelView.UIBinding.Default.isSearchCompanionsToHome) SendInviteMenu.Items.Add(toHome);
            else SendInviteMenu.Items.Add(toWork);

            cMenu.Items.Add(SendInviteMenu);
            CombinedSymbols.PreviewMouseDown += (obj, arg) =>
            {
                cMenu.IsOpen = true;
            };
            CombinedSymbols.ContextMenu = cMenu;
#endif
            return CombinedSymbols;
        }

        public static FrameworkElement CompanionRoutePoint(int UID, int RouteID)
        {
            var CompanionRP = new Grid();
            CompanionRP.Margin = new Thickness(-13, -13.5, 0, 0);
            CompanionRP.Width = 25;
            CompanionRP.Height = 25;
            var RPSymbol = new TextBlock();


            //RPSymbol.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#Segoe UI Symbol");
            RPSymbol.FontWeight = FontWeights.ExtraBold;
            RPSymbol.Foreground = new SolidColorBrush(Colors.White);
            RPSymbol.HorizontalAlignment = HorizontalAlignment.Center;
            RPSymbol.VerticalAlignment = VerticalAlignment.Center;

            var El = new Ellipse();
            El.Stroke = new SolidColorBrush(Colors.White);
            El.Fill = new SolidColorBrush(Colors.DodgerBlue);


            CompanionRP.Children.Add(El);
            CompanionRP.Children.Add(RPSymbol);

            CompanionRP.Tag = new Tuple<int, int>(UID, RouteID);



            var Tooltip = new ToolTip();
            Tooltip.Content = "";
            CompanionRP.Loaded += async (ev, ar) =>
            {
                var companion = await ViewModel.Companions.GetCompanionInfo(UID);
                Tooltip.Content = CompanionTooltip(companion);
                ToolTipService.SetToolTip(CompanionRP, Tooltip);

            };
#if NETFX_CORE
            var cMenu = new PopupMenu();
            if (Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome)
            {

                cMenu.Commands.Add(new UICommand(Shared.ModelView.UserProfile.Default.IsDriver ? ConvertMessages.Message("Offer") + " " + ConvertMessages.Message("ToHome") : ConvertMessages.Message("Drive") + " " + ConvertMessages.Message("ToHome"), async (command) =>
                {
                    await ViewModel.Trips.Send(UID, RouteID, true);

                }));
            }
            else
            {
                cMenu.Commands.Add(new UICommand(Shared.ModelView.UserProfile.Default.IsDriver ? ConvertMessages.Message("Offer") + " " + ConvertMessages.Message("ToWork") : ConvertMessages.Message("Drive") + " " + ConvertMessages.Message("ToWork"), async (command) =>
                {
                    await ViewModel.Trips.Send(UID, RouteID, false);

                }));
            }

            CompanionRP.PointerPressed += async (a, rt) =>
            {
                await cMenu.ShowForSelectionAsync(GetElementRect(CompanionRP));
            };
#endif
#if WPF
            ContextMenu cMenu = new ContextMenu();


            MenuItem SendInviteMenu = new MenuItem();
            SendInviteMenu.Header = ConvertMessages.Message("Drive");


            var toHome = new MenuItem();
            toHome.Header = ConvertMessages.Message("ToHome");
            toHome.Click += async (obj, args) =>
            {
                await ViewModel.Trips.Send(UID, RouteID, true);
            };

            var toWork = new MenuItem();
            toWork.Header = ConvertMessages.Message("ToWork");
            toWork.Click += async (obj, args) =>
            {
                await ViewModel.Trips.Send(UID, RouteID, false);
            };
            if (ModelView.UIBinding.Default.isSearchCompanionsToHome) SendInviteMenu.Items.Add(toHome);
            else SendInviteMenu.Items.Add(toWork);

            cMenu.Items.Add(SendInviteMenu);

            CompanionRP.PreviewMouseDown += (obj, arg) =>
            {
                cMenu.IsOpen = true;
            };
            CompanionRP.ContextMenu = cMenu;
#endif
            return CompanionRP;
        }

        static FrameworkElement CompanionTooltip(Model.UserCompanion user)
        {
            var ToolTipPanel = new StackPanel();
            var Name = new TextBlock { Text = user.FirstName + " " + user.LastName };

            ToolTipPanel.Children.Add(Name);
            if (Shared.ModelView.UIBinding.Default.isSearchCompanionsToHome)
            {
                var Start = new TextBlock { Text = Shared.ConvertMessages.Message("startat") + " " + user.ToHomeHlp };

                ToolTipPanel.Children.Add(Start);
            }
            else {
                var Start = new TextBlock { Text=Shared.ConvertMessages.Message("startat") + " " + user.ToWorkHlp };
                ToolTipPanel.Children.Add(Start);
            }
            
            if (!String.IsNullOrEmpty(user.Brand)) { 
                var CarInfo = new TextBlock { Text = user.Brand + " " + user.Model + " | " + ConvertMessages.Message("Places") + ":" + user.Places.ToString() };
                ToolTipPanel.Children.Add(CarInfo);
            }
            var Payable = new TextBlock();
            Payable.Text = PaymentConverter(user.Payment);
            ToolTipPanel.Children.Add(Payable);
            return ToolTipPanel;

        }

        
#endif
        static String PaymentConverter(int Payment)
        {
            switch (Payment)
            {
                case 0:
                    return ConvertMessages.Message("Pay");
                case 1:
                    return ConvertMessages.Message("FreePay");
                case 2:
                    return ConvertMessages.Message("NotDecided");
                default:
                    return "";
            };

        }
    }




    }

