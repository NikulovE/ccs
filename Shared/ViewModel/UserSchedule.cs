using Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;


#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
#endif

#if WPF
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.ServiceModel.Channels;
using System.Windows;
using CCS.Classic;
using System.Drawing.Printing;
#endif

namespace Shared.ViewModel
{
    class UserSchedule
    {
        public static async Task<bool> Update(int ScheduleID)
        {

            Shared.View.General.inLoading();
            var query = await Model.Requests.UpdateSchedule(ModelView.UserSchedule.Default.First(req => req.ScheduleID == ScheduleID));
            Shared.View.General.outLoading();
            if (query.Item1 == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static async Task<bool> Load(ItemsControl WeeklyStack)
        {

            Shared.View.General.inLoading();
            var schedules = await Model.Requests.LoadSchedule();
            WeeklyStack.Items.Clear();
#if WPF
            WeeklyStack.Items.Refresh();
#endif
            Shared.View.General.outLoading();
            if (schedules.Item1 == true)
            {
                ModelView.UserSchedule.Default = schedules.Item2;
                foreach (var schedul in ModelView.UserSchedule.Default)
                {
                    WeeklyStack.Items.Add(AddWeeklyScheduleToStack(schedul));
                }

                var addschedule = new Button();
                addschedule.Margin = new Thickness(10);
                addschedule.Content= ConvertMessages.Message("AddSchedule");
                WeeklyStack.Items.Add(addschedule);
                addschedule.HorizontalAlignment = HorizontalAlignment.Left;
                addschedule.Click += async (ev, ar) =>
                {
                    var newschedule=await Model.Requests.AddSchedule();
                    if (newschedule.Item1 == true)
                    {
                        ModelView.UserSchedule.Default.Add(newschedule.Item2);
                        var but=WeeklyStack.Items.IndexOf(addschedule);
                        WeeklyStack.Items.Insert(but, AddWeeklyScheduleToStack(newschedule.Item2));
                        
                    }

                };
                return true;
            }
            else
            {
                return false;
            }
        }

        public static StackPanel AddWeeklyScheduleToStack(WeeklySchedule schedul) {
            var weeklyline = new StackPanel();
            weeklyline.Tag = schedul.ScheduleID;
            weeklyline.Margin = new Thickness(5);

            var ThisWeeklyScheduleIsActual = new RadioButton { GroupName = "WeeklySchedulesStack", Content = "Use this schedule", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5) };
            
            ThisWeeklyScheduleIsActual.Checked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).IsEnabled = true;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            ThisWeeklyScheduleIsActual.Unchecked += (ev, ar) =>
            {
                try
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).IsEnabled = false;
                    //await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                }
                catch { }
            };
            ThisWeeklyScheduleIsActual.IsChecked = schedul.IsEnabled;

            weeklyline.Children.Add(ThisWeeklyScheduleIsActual);
            weeklyline.Orientation = Orientation.Horizontal;

            ////Helper
            /////////////////////////////Whole week
            var Icons= new StackPanel();
            Icons.VerticalAlignment = VerticalAlignment.Bottom;
            //Icons.Visibility = Visibility.Collapsed;
            Icons.Margin = new Thickness(5);
            //var mybu = new Button();
            //mybu.HorizontalAlignment = HorizontalAlignment.Center;
            
            //Wholeweekname.Content = ConvertMessages.Message("wholeweek");


            var TimeToHomeIcon = new TextBlock();
            TimeToHomeIcon.FontFamily = Shared.View.General.SetSegoeMDLFont();
            TimeToHomeIcon.Margin = new Thickness(0, 0, 0, 5);
            TimeToHomeIcon.Text = "\uEA8A";
#if WPF
            TimeToHomeIcon.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkIcon = new TextBlock();
            TimeToWorkIcon.FontFamily = Shared.View.General.SetSegoeMDLFont();
            TimeToWorkIcon.Margin = new Thickness(0, 0, 0, 13);
            TimeToWorkIcon.Text = "\uEB4E";
#if WPF
            TimeToWorkIcon.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            //Icons.Children.Add(mybu);
            Icons.Children.Add(TimeToWorkIcon);
            Icons.Children.Add(TimeToHomeIcon);

            weeklyline.Children.Add(Icons);

            /////////////////////////////Whole week
            var WholeWeekStack = new StackPanel();
            WholeWeekStack.Visibility = Visibility.Collapsed;
            WholeWeekStack.Margin = new Thickness(5);
            var Wholeweekname = new Button();
            Wholeweekname.HorizontalAlignment = HorizontalAlignment.Center;
            Wholeweekname.Content = ConvertMessages.Message("wholeweek");
            

            var TimeToHomeWeek = new TimePicker();
            TimeToHomeWeek.Time = schedul.Monday.ToHomeHlp;
#if WPF
            TimeToHomeWeek.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkWeek = new TimePicker();
            TimeToWorkWeek.Time = schedul.Monday.ToWorkHlp;
#if WPF
            TimeToWorkWeek.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            WholeWeekStack.Children.Add(Wholeweekname);
            WholeWeekStack.Children.Add(TimeToWorkWeek);
            WholeWeekStack.Children.Add(TimeToHomeWeek);

            weeklyline.Children.Add(WholeWeekStack);

            /////////////////////////////Monday
            var Mondaydaystack = new StackPanel();
            Mondaydaystack.Margin = new Thickness(5);
            var Mondayname = new ToggleButton();
            Mondayname.HorizontalAlignment = HorizontalAlignment.Center;
            Mondayname.Content = ConvertMessages.Message("monday");
            Mondayname.IsChecked = schedul.Monday.IsEnabled;
            Mondayname.Checked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Monday.IsEnabled = true;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            Mondayname.Unchecked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Monday.IsEnabled = false;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            var TimeToHomeMonday = new TimePicker();
            TimeToHomeMonday.Time = schedul.Monday.ToHomeHlp;
#if WPF
            TimeToHomeMonday.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkMonday = new TimePicker();
            TimeToWorkMonday.Time = schedul.Monday.ToWorkHlp;
#if WPF
            TimeToWorkMonday.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            Mondaydaystack.Children.Add(Mondayname);
            Mondaydaystack.Children.Add(TimeToWorkMonday);
            Mondaydaystack.Children.Add(TimeToHomeMonday);
            
            weeklyline.Children.Add(Mondaydaystack);
            /////////////////////////////Tuesday
            ///////////////////
            var Tuesdaydaystack = new StackPanel();
            Tuesdaydaystack.Margin = new Thickness(5);
            var Tuesdayname = new ToggleButton();
            Tuesdayname.HorizontalAlignment = HorizontalAlignment.Center;
            Tuesdayname.Content = ConvertMessages.Message("tuesday");
            Tuesdayname.IsChecked = schedul.Tuesday.IsEnabled;
            Tuesdayname.Checked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Tuesday.IsEnabled = true;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            Tuesdayname.Unchecked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Tuesday.IsEnabled = false;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            var TimeToHomeTuesday = new TimePicker();
            TimeToHomeTuesday.Time = schedul.Tuesday.ToHomeHlp;
#if WPF
            TimeToHomeTuesday.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkTuesday = new TimePicker();
            TimeToWorkTuesday.Time = schedul.Tuesday.ToWorkHlp;
#if WPF
            TimeToWorkTuesday.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            Tuesdaydaystack.Children.Add(Tuesdayname);
            Tuesdaydaystack.Children.Add(TimeToWorkTuesday);
            Tuesdaydaystack.Children.Add(TimeToHomeTuesday);
            
            weeklyline.Children.Add(Tuesdaydaystack);
            /////////////////////////////////////////
            //////////////////////////////////////
            var Wednesdaydaystack = new StackPanel();
            Wednesdaydaystack.Margin = new Thickness(5);
            var Wednesdayname = new ToggleButton();
            Wednesdayname.HorizontalAlignment = HorizontalAlignment.Center;
            Wednesdayname.Content = ConvertMessages.Message("wednesday");
            Wednesdayname.IsChecked = schedul.Wednesday.IsEnabled;
            Wednesdayname.Checked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Wednesday.IsEnabled = true;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            Wednesdayname.Unchecked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Wednesday.IsEnabled = false;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            var TimeToHomeWednesday = new TimePicker();
            TimeToHomeWednesday.Time = schedul.Wednesday.ToHomeHlp;
#if WPF
            TimeToHomeWednesday.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkWednesday = new TimePicker();
            TimeToWorkWednesday.Time = schedul.Wednesday.ToWorkHlp;
#if WPF
            TimeToWorkWednesday.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            Wednesdaydaystack.Children.Add(Wednesdayname);
            Wednesdaydaystack.Children.Add(TimeToWorkWednesday);
            Wednesdaydaystack.Children.Add(TimeToHomeWednesday);
            
            weeklyline.Children.Add(Wednesdaydaystack);
            //////////
            //////////
            var Thursdaydaystack = new StackPanel();
            Thursdaydaystack.Margin = new Thickness(5);
            var Thursdayname = new ToggleButton();
            Thursdayname.HorizontalAlignment = HorizontalAlignment.Center;
            Thursdayname.Content = ConvertMessages.Message("thursday");
            Thursdayname.IsChecked = schedul.Thursday.IsEnabled;
            Thursdayname.Checked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Thursday.IsEnabled = true;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            Thursdayname.Unchecked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Thursday.IsEnabled = false;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            var TimeToHomeThursday = new TimePicker();
            TimeToHomeThursday.Time = schedul.Thursday.ToHomeHlp;
#if WPF
            TimeToHomeThursday.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkThursday = new TimePicker();
            TimeToWorkThursday.Time = schedul.Thursday.ToWorkHlp;
#if WPF
            TimeToWorkThursday.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            Thursdaydaystack.Children.Add(Thursdayname);
            Thursdaydaystack.Children.Add(TimeToWorkThursday);
            Thursdaydaystack.Children.Add(TimeToHomeThursday);
           
            weeklyline.Children.Add(Thursdaydaystack);

            //////////////////////
            ///////////////////
            var Fridaydaystack = new StackPanel();
            Fridaydaystack.Margin = new Thickness(5);
            var Fridayname = new ToggleButton();
            Fridayname.HorizontalAlignment = HorizontalAlignment.Center;
            Fridayname.Content = ConvertMessages.Message("friday");
            Fridayname.IsChecked = schedul.Friday.IsEnabled;
            Fridayname.Checked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Friday.IsEnabled = true;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            Fridayname.Unchecked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Friday.IsEnabled = false;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            var TimeToHomeFriday = new TimePicker();
            TimeToHomeFriday.Time = schedul.Friday.ToHomeHlp;
#if WPF
            TimeToHomeFriday.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkFriday = new TimePicker();
            TimeToWorkFriday.Time = schedul.Friday.ToWorkHlp;
#if WPF
            TimeToWorkFriday.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            Fridaydaystack.Children.Add(Fridayname);
            Fridaydaystack.Children.Add(TimeToWorkFriday);
            Fridaydaystack.Children.Add(TimeToHomeFriday);
            
            weeklyline.Children.Add(Fridaydaystack);
            //////////////////
            //////////////////
            var Saturdaydaystack = new StackPanel();
            Saturdaydaystack.Margin = new Thickness(5);
            var Saturdayname = new ToggleButton();
            Saturdayname.HorizontalAlignment = HorizontalAlignment.Center;
            Saturdayname.Content = ConvertMessages.Message("saturday");
            Saturdayname.IsChecked = schedul.Saturday.IsEnabled;
            Saturdayname.Checked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Saturday.IsEnabled = true;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            Saturdayname.Unchecked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Saturday.IsEnabled = false;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            var TimeToHomeSaturday = new TimePicker();
            TimeToHomeSaturday.Time = schedul.Saturday.ToHomeHlp;
#if WPF
            TimeToHomeSaturday.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkSaturday = new TimePicker();
            TimeToWorkSaturday.Time = schedul.Saturday.ToWorkHlp;
#if WPF
            TimeToWorkSaturday.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            Saturdaydaystack.Children.Add(Saturdayname);
            Saturdaydaystack.Children.Add(TimeToWorkSaturday);
            Saturdaydaystack.Children.Add(TimeToHomeSaturday);
            
            weeklyline.Children.Add(Saturdaydaystack);
            ///////////////////////////
            var Sundaydaystack = new StackPanel();
            Sundaydaystack.Margin = new Thickness(5);
            var Sundayname = new ToggleButton();
            Sundayname.HorizontalAlignment = HorizontalAlignment.Center;
            Sundayname.Content = ConvertMessages.Message("sunday");
            Sundayname.IsChecked = schedul.Sunday.IsEnabled;
            Sundayname.Checked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Sunday.IsEnabled = true;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            Sundayname.Unchecked += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Sunday.IsEnabled = false;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            var TimeToHomeSunday = new TimePicker();
            TimeToHomeSunday.Time = schedul.Sunday.ToHomeHlp;
#if WPF
            TimeToHomeSunday.ToolTip = ConvertMessages.Message("DepartureToHome");
#endif

            var TimeToWorkSunday = new TimePicker();
            TimeToWorkSunday.Time = schedul.Sunday.ToWorkHlp;
#if WPF
            TimeToWorkSunday.ToolTip = ConvertMessages.Message("DepartureToWork");
#endif


            Sundaydaystack.Children.Add(Sundayname);
            Sundaydaystack.Children.Add(TimeToWorkSunday);
            Sundaydaystack.Children.Add(TimeToHomeSunday);
            

            weeklyline.Children.Add(Sundaydaystack);


            var ToHomeAt = new List<TimeSpan>();
            var ToWorkAt = new List<TimeSpan>();
            ToHomeAt.Add(schedul.Monday.ToHomeHlp);
            ToHomeAt.Add(schedul.Tuesday.ToHomeHlp);
            ToHomeAt.Add(schedul.Wednesday.ToHomeHlp);
            ToHomeAt.Add(schedul.Thursday.ToHomeHlp);
            ToHomeAt.Add(schedul.Friday.ToHomeHlp);
            ToHomeAt.Add(schedul.Saturday.ToHomeHlp);
            ToHomeAt.Add(schedul.Sunday.ToHomeHlp);

            ToWorkAt.Add(schedul.Monday.ToWorkHlp);
            ToWorkAt.Add(schedul.Tuesday.ToWorkHlp);
            ToWorkAt.Add(schedul.Wednesday.ToWorkHlp);
            ToWorkAt.Add(schedul.Thursday.ToWorkHlp);
            ToWorkAt.Add(schedul.Friday.ToWorkHlp);
            ToWorkAt.Add(schedul.Saturday.ToWorkHlp);
            ToWorkAt.Add(schedul.Sunday.ToWorkHlp);

            var ToHomeFlag = ToHomeAt.Select(r => r.Ticks).Distinct().Count();
            var ToWorkfag = ToWorkAt.Select(r => r.Ticks).Distinct().Count();

            if (ToHomeFlag == 1 && ToWorkfag == 1) {
                Mondaydaystack.Visibility = Visibility.Collapsed;
                Tuesdaydaystack.Visibility = Visibility.Collapsed;
                Wednesdaydaystack.Visibility = Visibility.Collapsed;
                Thursdaydaystack.Visibility = Visibility.Collapsed;
                Fridaydaystack.Visibility = Visibility.Collapsed;
                Saturdaydaystack.Visibility = Visibility.Collapsed;
                Sundaydaystack.Visibility = Visibility.Collapsed;

                WholeWeekStack.Visibility = Visibility.Visible;
            }

            Wholeweekname.Click += (orb, arg) =>
            {
                Mondaydaystack.Visibility = Visibility.Visible;
                Tuesdaydaystack.Visibility = Visibility.Visible;
                Wednesdaydaystack.Visibility = Visibility.Visible;
                Thursdaydaystack.Visibility = Visibility.Visible;
                Fridaydaystack.Visibility = Visibility.Visible;
                Saturdaydaystack.Visibility = Visibility.Visible;
                Sundaydaystack.Visibility = Visibility.Visible;

                WholeWeekStack.Visibility = Visibility.Collapsed;


                TimeToHomeMonday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Monday.ToHomeHlp = TimeToHomeMonday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToWorkMonday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Monday.ToWorkHlp = TimeToWorkMonday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToHomeTuesday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Tuesday.ToHomeHlp = TimeToHomeTuesday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToWorkTuesday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Tuesday.ToWorkHlp = TimeToWorkTuesday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToHomeWednesday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Wednesday.ToHomeHlp = TimeToHomeWednesday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToWorkWednesday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Wednesday.ToWorkHlp = TimeToWorkWednesday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToHomeThursday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Thursday.ToHomeHlp = TimeToHomeThursday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToWorkThursday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Thursday.ToWorkHlp = TimeToWorkThursday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToHomeFriday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Friday.ToHomeHlp = TimeToHomeFriday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToWorkFriday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Friday.ToWorkHlp = TimeToWorkFriday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToHomeSaturday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Saturday.ToHomeHlp = TimeToHomeSaturday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToWorkSaturday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Saturday.ToWorkHlp = TimeToWorkSaturday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToHomeSunday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Sunday.ToHomeHlp = TimeToHomeSunday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
                TimeToWorkSunday.TimeChanged += async (ev, ar) =>
                {
                    ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Sunday.ToWorkHlp = TimeToWorkSunday.Time;
                    await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
                };
            };

            TimeToHomeWeek.TimeChanged += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Monday.ToHomeHlp = TimeToHomeWeek.Time;
                TimeToHomeMonday.Time= TimeToHomeWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Tuesday.ToHomeHlp = TimeToHomeWeek.Time;
                TimeToHomeTuesday.Time = TimeToHomeWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Wednesday.ToHomeHlp = TimeToHomeWeek.Time;
                TimeToHomeWednesday.Time = TimeToHomeWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Thursday.ToHomeHlp = TimeToHomeWeek.Time;
                TimeToHomeThursday.Time = TimeToHomeWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Friday.ToHomeHlp = TimeToHomeWeek.Time;
                TimeToHomeFriday.Time = TimeToHomeWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Saturday.ToHomeHlp = TimeToHomeWeek.Time;
                TimeToHomeSaturday.Time = TimeToHomeWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Sunday.ToHomeHlp = TimeToHomeWeek.Time;
                TimeToHomeSunday.Time = TimeToHomeWeek.Time;

                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            TimeToWorkWeek.TimeChanged += async (ev, ar) =>
            {
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Monday.ToWorkHlp = TimeToHomeWeek.Time;
                TimeToWorkMonday.Time = TimeToWorkWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Tuesday.ToWorkHlp = TimeToHomeWeek.Time;
                TimeToWorkTuesday.Time = TimeToWorkWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Wednesday.ToWorkHlp = TimeToHomeWeek.Time;
                TimeToWorkWednesday.Time = TimeToWorkWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Thursday.ToWorkHlp = TimeToHomeWeek.Time;
                TimeToWorkThursday.Time = TimeToWorkWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Friday.ToWorkHlp = TimeToHomeWeek.Time;
                TimeToWorkFriday.Time = TimeToWorkWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Saturday.ToWorkHlp = TimeToHomeWeek.Time;
                TimeToWorkSaturday.Time = TimeToWorkWeek.Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == schedul.ScheduleID).Sunday.ToWorkHlp = TimeToHomeWeek.Time;
                TimeToWorkSunday.Time = TimeToWorkWeek.Time;
                await Shared.ViewModel.UserSchedule.Update(schedul.ScheduleID);
            };
            var RemoveSchedule = new Button();
            RemoveSchedule.VerticalAlignment = VerticalAlignment.Center;
            RemoveSchedule.Content= ConvertMessages.Message("DeleteSchedule");
            RemoveSchedule.Click += async (ev, ar) =>
            {
                Shared.View.General.inLoading();
                var res = await Shared.Model.Requests.DeleteSchedule(schedul.ScheduleID);
                Shared.View.General.outLoading();
                if (res.Item1==true)
                {
                    ModelView.UserSchedule.Default.Remove(schedul);
                    weeklyline.Visibility = Visibility.Collapsed;
                    //weeklyline = null;
                }
            };

            weeklyline.Children.Add(RemoveSchedule);
            return weeklyline;
        }
    }
}
