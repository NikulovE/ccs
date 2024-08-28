using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#else
#if WPF
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
#endif
#endif

namespace Shared.View
{
    class PathControl
    {
#if XAMARIN
#else

        public static void ShowPathControl(ItemsControl PathControl)
        {
            var stack = new List<FrameworkElement>();
            foreach (var path in Shared.ModelView.UIBinding.Default.Routes)
            {
                var Path = new StackPanel { Orientation = Orientation.Horizontal };
#if WPF
                var PathSelector = new RadioButton { GroupName = "RoutePoints", ToolTip = ConvertMessages.Message("WriteRoute") + " " + path.PathID, Tag = path.PathID };
#else
                var PathSelector = new RadioButton { GroupName = "RoutePoints", Content = ConvertMessages.Message("WriteRoute") + " " + path.PathID, Tag = path.PathID };
#endif
                PathSelector.Loaded += (ev, ar) =>
                {
                    PathSelector.IsChecked = true;
                };
                PathSelector.Checked += (ev, ar) =>
                {
                    Shared.ModelView.UIBinding.Default.SelectedPath = path.PathID;
                };

                bool textchanged = false;
                var PathName = new TextBox { Width = 80, Text = path.DirectionName
#if WPF
                    ,ToolTip= ConvertMessages.Message("DirectionName") 
#endif
                    ,VerticalAlignment =VerticalAlignment.Center, TextAlignment=TextAlignment.Left, Margin = new Thickness(5, 0, 5, 0), Padding=new Thickness(5)
#if WPF
                    ,Cursor = System.Windows.Input.Cursors.Arrow 
#endif
                    };
                PathName.TextChanged += (ev, ar) => {
                    if (PathName.Text != path.DirectionName)
                    {
                        textchanged = true;
                    }
                };
#if WPF
                PathName.MouseLeave += async (ex, arx) =>
                {
                    if (textchanged) {
                       await Shared.ViewModel.RoutePoints.ChangePath(path.PathID, 20, false, PathName.Text);
                        Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == path.PathID).DirectionName = PathName.Text;
                    }
                };
#endif


                var PathWay = new Button();
                PathWay.FontFamily = General.SetSegoeMDLFont();
                var Sym = new Grid();


                var Arrow = new TextBlock();
                Arrow.HorizontalAlignment = HorizontalAlignment.Center;
                Arrow.VerticalAlignment = VerticalAlignment.Center;
                Arrow.FontFamily = General.SetSegoeMDLFont();
                Arrow.Text = "\uEA62";
                Arrow.Margin = new Thickness(0, 10, 0, 0);


                var RoutePointSymbol = new TextBlock();
                RoutePointSymbol.HorizontalAlignment = HorizontalAlignment.Center;
                RoutePointSymbol.VerticalAlignment = VerticalAlignment.Top;
                RoutePointSymbol.FontFamily = General.SetSegoeMDLFont();
                if (path.IsToHome == false) { RoutePointSymbol.Text = "\uEB4E"; }
                if (path.IsToHome == true) { RoutePointSymbol.Text = "\uEA8A"; }

#if WPF
                PathWay.Cursor = System.Windows.Input.Cursors.Arrow;
                PathWay.Margin = new Thickness(5, 0, 5, 0);
                PathWay.Padding = new Thickness(8, 5, 8, 5);
                PathSelector.Cursor = System.Windows.Input.Cursors.Arrow;
                PathSelector.VerticalAlignment = VerticalAlignment.Center;
#endif

                Sym.Children.Add(Arrow);
                Sym.Children.Add(RoutePointSymbol);


                PathWay.Content = Sym;

                PathWay.Click += async (ev, ar) =>
                {
                    path.IsToHome = !path.IsToHome;
                    Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == path.PathID).IsToHome = path.IsToHome;
                    if (path.IsToHome == false) {                   
                        await Shared.ViewModel.RoutePoints.ChangePath(path.PathID, 0);
                        RoutePointSymbol.Text = "\uEB4E";
                    }
                    if (path.IsToHome == true) {
                        await Shared.ViewModel.RoutePoints.ChangePath(path.PathID, 10);
                        RoutePointSymbol.Text = "\uEA8A";

                    }
                };

                var RemoveDirection = new Button { Content = "-", Padding=new Thickness(8,0,8,0)
#if WPF
                    , Cursor = System.Windows.Input.Cursors.Arrow 
#endif
                    };
                RemoveDirection.Click += async (ev, ar) =>
                {
                    try
                    {
                        if (await Shared.ViewModel.RoutePoints.ChangePath(path.PathID, 30))
                        {
                            Shared.ModelView.UIBinding.Default.Routes.Remove(path);
                            stack.Remove(Path);
#if WPF
                            PathControl.Items.Refresh();
#endif
                        }
                    }
                    catch { }
                };
                Path.Children.Add(PathSelector);
                Path.Children.Add(PathName);                
                Path.Children.Add(PathWay);
                Path.Children.Add(DayOfWeekActuality(path.PathID, 1, path.IsMon));
                Path.Children.Add(DayOfWeekActuality(path.PathID, 2, path.IsTue));
                Path.Children.Add(DayOfWeekActuality(path.PathID, 3, path.IsWed));
                Path.Children.Add(DayOfWeekActuality(path.PathID, 4, path.IsThu));
                Path.Children.Add(DayOfWeekActuality(path.PathID, 5, path.IsFri));
                Path.Children.Add(DayOfWeekActuality(path.PathID, 6, path.IsSat));
                Path.Children.Add(DayOfWeekActuality(path.PathID, 7, path.IsSun));
                Path.Children.Add(RemoveDirection);
                stack.Add(Path);

            };
            PathControl.ItemsSource = stack;

        }


        private static ToggleButton DayOfWeekActuality(int PathID, int Day, bool isCheked)
        {
            var DayActuality = new ToggleButton();
            DayActuality.IsChecked = isCheked;
            switch (Day)
            {

                case 1:
                    DayActuality.Content = ConvertMessages.Message("Mon");
                    break;
                case 2:
                    DayActuality.Content = ConvertMessages.Message("Tue");
                    break;
                case 3:
                    DayActuality.Content = ConvertMessages.Message("Wed");
                    break;
                case 4:
                    DayActuality.Content = ConvertMessages.Message("Thu");
                    break;
                case 5:
                    DayActuality.Content = ConvertMessages.Message("Fri");
                    break;
                case 6:
                    DayActuality.Content = ConvertMessages.Message("Sat");
                    break;
                case 7:
                    DayActuality.Content = ConvertMessages.Message("Sun");
                    break;


            }
            DayActuality.Checked += async (ev, ar) =>
            {
                await Shared.ViewModel.RoutePoints.ChangePath(PathID, Day, false);
                var selectedpath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == PathID);
                switch (Day)
                {

                    case 1:
                        selectedpath.Actuality.Monday = true;
                        break;
                    case 2:
                        selectedpath.Actuality.Tuesday = true;
                        break;
                    case 3:
                        selectedpath.Actuality.Wednesday = true;
                        break;
                    case 4:
                        selectedpath.Actuality.Thursday = true;
                        break;
                    case 5:
                        selectedpath.Actuality.Friday = true;
                        break;
                    case 6:
                        selectedpath.Actuality.Saturday = true;
                        break;
                    case 7:
                        selectedpath.Actuality.Sunday = true;
                        break;
                }
            };
            DayActuality.Unchecked += async (ev, ar) =>
            {
                await Shared.ViewModel.RoutePoints.ChangePath(PathID, Day * -1, false);
                var selectedpath = Shared.ModelView.UIBinding.Default.Routes.First(req => req.PathID == PathID);
                switch (Day)
                {

                    case 1:
                        selectedpath.Actuality.Monday = false;
                        break;
                    case 2:
                        selectedpath.Actuality.Tuesday = false;
                        break;
                    case 3:
                        selectedpath.Actuality.Wednesday = false;
                        break;
                    case 4:
                        selectedpath.Actuality.Thursday = false;
                        break;
                    case 5:
                        selectedpath.Actuality.Friday = false;
                        break;
                    case 6:
                        selectedpath.Actuality.Saturday = false;
                        break;
                    case 7:
                        selectedpath.Actuality.Sunday = false;
                        break;
                }
            };
#if WPF
            DayActuality.Cursor = System.Windows.Input.Cursors.Arrow;
            DayActuality.Margin = new Thickness(5, 0, 5, 0);
            DayActuality.Padding = new Thickness(5);
#endif
            return DayActuality;



        }
#endif
            }
        }
