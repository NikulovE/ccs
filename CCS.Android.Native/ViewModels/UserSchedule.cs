using Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Android.Widget;
using Android.Content;
using Android.App;
using Android.Views;

namespace Shared.ViewModel
{
    class UserSchedule
    {
        public static async Task<bool> Update(int ScheduleID)
        {

            //Shared.View.General.inLoading(progressBar, output);
            var query = await Model.Requests.UpdateSchedule(ModelView.UserSchedule.Default.First(req => req.ScheduleID == ScheduleID));
            //Shared.View.General.outLoading(progressBar);
            if (query.Item1 == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static async Task<bool> Load(ProgressBar progressBar, TextView output, ScrollView WeeklyList, Context context)
        {

            Shared.View.General.inLoading(progressBar, output);
            var schedules = await Model.Requests.LoadSchedule();
            WeeklyList.RemoveAllViews();
            Shared.View.General.outLoading(progressBar);
            var WeeklyStackPanel = new LinearLayout(context);
            WeeklyStackPanel.Orientation = Orientation.Vertical;
            if (schedules.Item1 == true)
            {
                ModelView.UserSchedule.Default = schedules.Item2;
                foreach (var schedul in ModelView.UserSchedule.Default)
                {
                    WeeklyStackPanel.AddView(AddWeeklyScheduleToStack(schedul, context));
                }

                var addschedule = new Button(context);
                addschedule.Text= ConvertMessages.Message("Add Schedule");
                WeeklyStackPanel.AddView(addschedule);
                addschedule.Click += async (ev, ar) =>
                {
                    var newschedule=await Model.Requests.AddSchedule();
                    if (newschedule.Item1 == true)
                    {
                        Shared.ModelView.UserSchedule.Default.Add(newschedule.Item2);
                        WeeklyStackPanel.AddView(AddWeeklyScheduleToStack(newschedule.Item2, context), Shared.ModelView.UserSchedule.Default.Count-1);
                        
                    }

                };
                WeeklyList.AddView(WeeklyStackPanel);
                // ModelView.UserSchedule.Default.PropertyChanged("ToHome");
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Rewrite(ScrollView WeeklyList, Context context) {

            WeeklyList.RemoveAllViews();
            var WeeklyStackPanel = new LinearLayout(context);
            WeeklyStackPanel.Orientation = Orientation.Vertical;
            foreach (var schedul in ModelView.UserSchedule.Default)
            {
                WeeklyStackPanel.AddView(AddWeeklyScheduleToStack(schedul, context));
            }

            var addschedule = new Button(context);
            addschedule.Text = ConvertMessages.Message("Add Schedule");
            WeeklyStackPanel.AddView(addschedule);
            addschedule.Click += async (ev, ar) =>
            {
                var newschedule = await Model.Requests.AddSchedule();
                if (newschedule.Item1 == true)
                {
                    Shared.ModelView.UserSchedule.Default.Add(newschedule.Item2);
                    WeeklyStackPanel.AddView(AddWeeklyScheduleToStack(newschedule.Item2, context), Shared.ModelView.UserSchedule.Default.Count-1);

                }

            };
            WeeklyList.AddView(WeeklyStackPanel);
        }
        

        private async static void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            var picker = (TimePicker)sender;

            if (WeekToHomeChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToHomeHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToHomeHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                var Time = Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToHomeHlp;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Tuesday.ToHomeHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Wednesday.ToHomeHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Thursday.ToHomeHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Friday.ToHomeHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Saturday.ToHomeHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Sunday.ToHomeHlp = Time;
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    WeekToHomeChanged = false;
                }
            }
            if (WeekToWorkChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) >= 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToWorkHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToWorkHlp = new TimeSpan(picker.Hour, picker.Minute, 0);

                var Time = Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToWorkHlp;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Tuesday.ToWorkHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Wednesday.ToWorkHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Thursday.ToWorkHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Friday.ToWorkHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Saturday.ToWorkHlp = Time;
                ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Sunday.ToWorkHlp = Time;
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    WeekToWorkChanged = false;
                }
            }
            if (MondayToHomeChanged) {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToHomeHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToHomeHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID)) {                    
                    Shared.Actions.refreshSchedules();
                    MondayToHomeChanged = false;
                }
            }
            if (MondayToWorkChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) >= 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToWorkHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.ToWorkHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    MondayToWorkChanged = false;
                }
            }
            if (TuesdayToHomeChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Tuesday.ToHomeHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Tuesday.ToHomeHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    TuesdayToHomeChanged = false;
                }
            }
            if (TuesdayToWorkChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Tuesday.ToWorkHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Tuesday.ToWorkHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    TuesdayToWorkChanged = false;
                }
            }
            if (WednesdayToHomeChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Wednesday.ToHomeHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Wednesday.ToHomeHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    WednesdayToHomeChanged = false;
                }
            }
            if (WednesdayToWorkChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Wednesday.ToWorkHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Wednesday.ToWorkHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    WednesdayToWorkChanged = false;
                }
            }
            if (ThursdayToHomeChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Thursday.ToHomeHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Thursday.ToHomeHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    ThursdayToHomeChanged = false;
                }
            }
            if (ThursdayToWorkChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Thursday.ToWorkHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Thursday.ToWorkHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    ThursdayToWorkChanged = false;
                }
            }
            if (FridayToHomeChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Friday.ToHomeHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Friday.ToHomeHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    FridayToHomeChanged = false;
                }
            }
            if (FridayToWorkChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Friday.ToWorkHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Friday.ToWorkHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                    if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    FridayToWorkChanged = false;
                }
            }
            if (SaturdayToHomeChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Saturday.ToHomeHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Saturday.ToHomeHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    SaturdayToHomeChanged = false;
                }
            }
            if (SaturdayToWorkChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Saturday.ToWorkHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Saturday.ToWorkHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    SaturdayToWorkChanged = false;
                }
            }
            if (SundayToHomeChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Sunday.ToHomeHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Sunday.ToHomeHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    SundayToHomeChanged = false;
                }
            }
            if (SundayToWorkChanged)
            {
                if (((int)Android.OS.Build.VERSION.SdkInt) < 23) Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Sunday.ToWorkHlp = new TimeSpan(picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                else Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Sunday.ToWorkHlp = new TimeSpan(picker.Hour, picker.Minute, 0);
                if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                {
                    Shared.Actions.refreshSchedules();
                    SundayToWorkChanged = false;
                }
            }

        }

        static bool WeekToHomeChanged = false;
        static bool WeekToWorkChanged = false;

        static bool MondayToHomeChanged = false;
        static bool MondayToWorkChanged = false;
        static bool TuesdayToHomeChanged = false;
        static bool TuesdayToWorkChanged = false;
        static bool WednesdayToHomeChanged = false;
        static bool WednesdayToWorkChanged = false;
        static bool ThursdayToHomeChanged = false;
        static bool ThursdayToWorkChanged = false;
        static bool FridayToHomeChanged = false;
        static bool FridayToWorkChanged = false;
        static bool SaturdayToHomeChanged = false;
        static bool SaturdayToWorkChanged = false;
        static bool SundayToHomeChanged = false;
        static bool SundayToWorkChanged = false;
        static int SelectedScheduleID = -1;

        public static HorizontalScrollView AddWeeklyScheduleToStack(WeeklySchedule schedul, Context context) {
            
            var NextWeekScroll = new HorizontalScrollView(context);
            try
            {
                var DaysStack = new LinearLayout(context);
                DaysStack.Orientation = Orientation.Horizontal;
                var ThisWeeklyScheduleIsActual = new CheckBox(context);
                var imgViewParamss = new LinearLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.MatchParent);
                ThisWeeklyScheduleIsActual.LayoutParameters = imgViewParamss;
                ThisWeeklyScheduleIsActual.Gravity = Android.Views.GravityFlags.Center;
                //ThisWeeklyScheduleIsActual.SetMargins(30, 0, 0, 0);
                ThisWeeklyScheduleIsActual.Checked = schedul.IsEnabled;
                DaysStack.AddView(ThisWeeklyScheduleIsActual);

                ThisWeeklyScheduleIsActual.CheckedChange += async (ev, ar) =>
                {
                    try
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        Shared.ModelView.UserSchedule.Default.Select(c => { c.IsEnabled = false; return c; }).ToList();
                        Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).IsEnabled = ThisWeeklyScheduleIsActual.Checked;
                        if (await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID))
                        {
                            Shared.Actions.refreshSchedules();
                        }
                    }
                    catch { }
                };
                ////////////////////////////////////
                var WeekStack = new LinearLayout(context);
                WeekStack.Orientation = Orientation.Vertical;
                WeekStack.Visibility = ViewStates.Gone;
                var Week = new Button(context);
                Week.Text = "5/2 week->";

                WeekStack.AddView(Week);

                var WeekToHomeTimeStack = new LinearLayout(context);
                var CurrentWeekToHomeTime = new TextView(context);
                CurrentWeekToHomeTime.Text = schedul.Monday.ToHomeHlp.ToString();
                var SetWeekToHomeTime = new Button(context);
                //SetWeekToHomeTime.Text = "!";
                SetWeekToHomeTime.Click += (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    (new TimePickerDialog(context, TimePickerCallback, schedul.Monday.ToHomeHlp.Hours, schedul.Monday.ToHomeHlp.Minutes, true)).Show();
                    WeekToHomeChanged = true;
                };
                SetWeekToHomeTime.LayoutParameters = imgViewParamss;
                
                SetWeekToHomeTime.LayoutParameters = new LinearLayout.LayoutParams(80,80);
                SetWeekToHomeTime.Gravity = Android.Views.GravityFlags.Center;
                WeekToHomeTimeStack.AddView(CurrentWeekToHomeTime);
                WeekToHomeTimeStack.AddView(SetWeekToHomeTime);

                var WeekToWorkTimeStack = new LinearLayout(context);
                var CurrentWeekToWorkTime = new TextView(context);
                CurrentWeekToWorkTime.Text = schedul.Monday.ToWorkHlp.ToString();
                var SetWeekToWorkTime = new Button(context);
                //SetWeekToWorkTime.Text = "!";

                SetWeekToWorkTime.Click += (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    (new TimePickerDialog(context, TimePickerCallback, schedul.Monday.ToWorkHlp.Hours, schedul.Monday.ToWorkHlp.Minutes, true)).Show();
                    WeekToWorkChanged = true;
                };
                SetWeekToWorkTime.LayoutParameters = imgViewParamss;
                
                SetWeekToWorkTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                SetWeekToWorkTime.Gravity = Android.Views.GravityFlags.Center;
                WeekToWorkTimeStack.AddView(CurrentWeekToWorkTime);
                WeekToWorkTimeStack.AddView(SetWeekToWorkTime);

                WeekStack.AddView(WeekToHomeTimeStack);
                WeekStack.AddView(WeekToWorkTimeStack);


                DaysStack.AddView(WeekStack);

                ////////////////////////////////////////////////////////////////////////////////////////////

                var MondayStack = new LinearLayout(context);
                MondayStack.Orientation = Orientation.Vertical;

                var Monday = new ToggleButton(context);
                Monday.TextOn = "Monday";
                Monday.TextOff = "Monday";
                Monday.Checked = schedul.Monday.IsEnabled;
                Monday.Click += async (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Monday.IsEnabled = Monday.Checked;
                    await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID);
                };
                MondayStack.AddView(Monday);

                var MondayToHomeTimeStack = new LinearLayout(context);
                var CurrentMondayToHomeTime = new TextView(context);
                CurrentMondayToHomeTime.Text = schedul.Monday.ToHomeHlp.ToString();
                var SetMondayToHomeTime = new Button(context);
                //SetMondayToHomeTime.Text = "!";

                SetMondayToHomeTime.LayoutParameters = imgViewParamss;
                SetMondayToHomeTime.Gravity = Android.Views.GravityFlags.Center;
                SetMondayToHomeTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                MondayToHomeTimeStack.AddView(CurrentMondayToHomeTime);
                MondayToHomeTimeStack.AddView(SetMondayToHomeTime);

                var MondayToWorkTimeStack = new LinearLayout(context);
                var CurrentMondayToWorkTime = new TextView(context);
                CurrentMondayToWorkTime.Text = schedul.Monday.ToWorkHlp.ToString();
                var SetMondayToWorkTime = new Button(context);
                //SetMondayToWorkTime.Text = "!";

                
                SetMondayToWorkTime.LayoutParameters = imgViewParamss;
                SetMondayToWorkTime.Gravity = Android.Views.GravityFlags.Center;
                SetMondayToWorkTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                MondayToWorkTimeStack.AddView(CurrentMondayToWorkTime);
                MondayToWorkTimeStack.AddView(SetMondayToWorkTime);

                MondayStack.AddView(MondayToHomeTimeStack);
                MondayStack.AddView(MondayToWorkTimeStack);


                DaysStack.AddView(MondayStack);
                //////////////////////////////////////////////
                var TuesdayStack = new LinearLayout(context);
                TuesdayStack.Orientation = Orientation.Vertical;

                var Tuesday = new ToggleButton(context);
                Tuesday.TextOn = "Tuesday";
                Tuesday.TextOff = "Tuesday";
                TuesdayStack.AddView(Tuesday);
                Tuesday.Checked = schedul.Tuesday.IsEnabled;
                Tuesday.Click += async (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Tuesday.IsEnabled = Tuesday.Checked;
                    await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID);
                };
                var TuesdayToHomeTimeStack = new LinearLayout(context);
                var CurrentTuesdayToHomeTime = new TextView(context);
                CurrentTuesdayToHomeTime.Text = schedul.Tuesday.ToHomeHlp.ToString();
                var SetTuesdayToHomeTime = new Button(context);
                //SetTuesdayToHomeTime.Text = "!";

                SetTuesdayToHomeTime.LayoutParameters = imgViewParamss;
                SetTuesdayToHomeTime.Gravity = Android.Views.GravityFlags.Center;
                SetTuesdayToHomeTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                TuesdayToHomeTimeStack.AddView(CurrentTuesdayToHomeTime);
                TuesdayToHomeTimeStack.AddView(SetTuesdayToHomeTime);

                var TuesdayToWorkTimeStack = new LinearLayout(context);
                var CurrentTuesdayToWorkTime = new TextView(context);
                CurrentTuesdayToWorkTime.Text = schedul.Tuesday.ToWorkHlp.ToString();
                var SetTuesdayToWorkTime = new Button(context);
                //SetTuesdayToWorkTime.Text = "!";


                SetTuesdayToWorkTime.LayoutParameters = imgViewParamss;
                SetTuesdayToWorkTime.Gravity = Android.Views.GravityFlags.Center;
                SetTuesdayToWorkTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                TuesdayToWorkTimeStack.AddView(CurrentTuesdayToWorkTime);
                TuesdayToWorkTimeStack.AddView(SetTuesdayToWorkTime);

                TuesdayStack.AddView(TuesdayToHomeTimeStack);
                TuesdayStack.AddView(TuesdayToWorkTimeStack);


                DaysStack.AddView(TuesdayStack);
                ///////////////////////////////////////////
                var WednesdayStack = new LinearLayout(context);
                WednesdayStack.Orientation = Orientation.Vertical;

                var Wednesday = new ToggleButton(context);
                Wednesday.TextOn = "Wednesday";
                Wednesday.TextOff = "Wednesday";
                WednesdayStack.AddView(Wednesday);
                Wednesday.Checked = schedul.Wednesday.IsEnabled;
                Wednesday.Click += async (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Wednesday.IsEnabled = Wednesday.Checked;
                    await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID);
                };
                var WednesdayToHomeTimeStack = new LinearLayout(context);
                var CurrentWednesdayToHomeTime = new TextView(context);
                CurrentWednesdayToHomeTime.Text = schedul.Wednesday.ToHomeHlp.ToString();
                var SetWednesdayToHomeTime = new Button(context);
                //SetWednesdayToHomeTime.Text = "!";

                SetWednesdayToHomeTime.LayoutParameters = imgViewParamss;
                SetWednesdayToHomeTime.Gravity = Android.Views.GravityFlags.Center;
                SetWednesdayToHomeTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                WednesdayToHomeTimeStack.AddView(CurrentWednesdayToHomeTime);
                WednesdayToHomeTimeStack.AddView(SetWednesdayToHomeTime);

                var WednesdayToWorkTimeStack = new LinearLayout(context);
                var CurrentWednesdayToWorkTime = new TextView(context);
                CurrentWednesdayToWorkTime.Text = schedul.Wednesday.ToWorkHlp.ToString();
                var SetWednesdayToWorkTime = new Button(context);
                //SetWednesdayToWorkTime.Text = "!";


                SetWednesdayToWorkTime.LayoutParameters = imgViewParamss;
                SetWednesdayToWorkTime.Gravity = Android.Views.GravityFlags.Center;
                SetWednesdayToWorkTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                WednesdayToWorkTimeStack.AddView(CurrentWednesdayToWorkTime);
                WednesdayToWorkTimeStack.AddView(SetWednesdayToWorkTime);

                WednesdayStack.AddView(WednesdayToHomeTimeStack);
                WednesdayStack.AddView(WednesdayToWorkTimeStack);


                DaysStack.AddView(WednesdayStack);
                /////////////////////////////////////////////
                var ThursdayStack = new LinearLayout(context);
                ThursdayStack.Orientation = Orientation.Vertical;

                var Thursday = new ToggleButton(context);
                Thursday.TextOn = "Thursday";
                Thursday.TextOff = "Thursday";
                ThursdayStack.AddView(Thursday);
                Thursday.Checked = schedul.Thursday.IsEnabled;
                Thursday.Click += async (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Thursday.IsEnabled = Thursday.Checked;
                    await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID);
                };
                var ThursdayToHomeTimeStack = new LinearLayout(context);
                var CurrentThursdayToHomeTime = new TextView(context);
                CurrentThursdayToHomeTime.Text = schedul.Thursday.ToHomeHlp.ToString();
                var SetThursdayToHomeTime = new Button(context);
                //SetThursdayToHomeTime.Text = "!";

                SetThursdayToHomeTime.LayoutParameters = imgViewParamss;
                SetThursdayToHomeTime.Gravity = Android.Views.GravityFlags.Center;
                SetThursdayToHomeTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                ThursdayToHomeTimeStack.AddView(CurrentThursdayToHomeTime);
                ThursdayToHomeTimeStack.AddView(SetThursdayToHomeTime);

                var ThursdayToWorkTimeStack = new LinearLayout(context);
                var CurrentThursdayToWorkTime = new TextView(context);
                CurrentThursdayToWorkTime.Text = schedul.Thursday.ToWorkHlp.ToString();
                var SetThursdayToWorkTime = new Button(context);
                //SetThursdayToWorkTime.Text = "!";


                SetThursdayToWorkTime.LayoutParameters = imgViewParamss;
                SetThursdayToWorkTime.Gravity = Android.Views.GravityFlags.Center;
                SetThursdayToWorkTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                ThursdayToWorkTimeStack.AddView(CurrentThursdayToWorkTime);
                ThursdayToWorkTimeStack.AddView(SetThursdayToWorkTime);

                ThursdayStack.AddView(ThursdayToHomeTimeStack);
                ThursdayStack.AddView(ThursdayToWorkTimeStack);


                DaysStack.AddView(ThursdayStack);


                ///////////////////
                var FridayStack = new LinearLayout(context);
                FridayStack.Orientation = Orientation.Vertical;

                var Friday = new ToggleButton(context);
                Friday.TextOn = "Friday";
                Friday.TextOff = "Friday";
                FridayStack.AddView(Friday);
                Friday.Checked = schedul.Friday.IsEnabled;
                Friday.Click += async (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Friday.IsEnabled = Friday.Checked;
                    await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID);
                };
                var FridayToHomeTimeStack = new LinearLayout(context);
                var CurrentFridayToHomeTime = new TextView(context);
                CurrentFridayToHomeTime.Text = schedul.Friday.ToHomeHlp.ToString();
                var SetFridayToHomeTime = new Button(context);
                //SetFridayToHomeTime.Text = "!";

                SetFridayToHomeTime.LayoutParameters = imgViewParamss;
                SetFridayToHomeTime.Gravity = Android.Views.GravityFlags.Center;
                SetFridayToHomeTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                FridayToHomeTimeStack.AddView(CurrentFridayToHomeTime);
                FridayToHomeTimeStack.AddView(SetFridayToHomeTime);

                var FridayToWorkTimeStack = new LinearLayout(context);
                var CurrentFridayToWorkTime = new TextView(context);
                CurrentFridayToWorkTime.Text = schedul.Friday.ToWorkHlp.ToString();
                var SetFridayToWorkTime = new Button(context);
                //SetFridayToWorkTime.Text = "!";


                SetFridayToWorkTime.LayoutParameters = imgViewParamss;
                SetFridayToWorkTime.Gravity = Android.Views.GravityFlags.Center;
                SetFridayToWorkTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                FridayToWorkTimeStack.AddView(CurrentFridayToWorkTime);
                FridayToWorkTimeStack.AddView(SetFridayToWorkTime);

                FridayStack.AddView(FridayToHomeTimeStack);
                FridayStack.AddView(FridayToWorkTimeStack);


                DaysStack.AddView(FridayStack);
                ////////////////////////////////////////
                var SaturdayStack = new LinearLayout(context);
                SaturdayStack.Orientation = Orientation.Vertical;

                var Saturday = new ToggleButton(context);
                Saturday.TextOn = "Saturday";
                Saturday.TextOff = "Saturday";
                SaturdayStack.AddView(Saturday);
                Saturday.Checked = schedul.Saturday.IsEnabled;
                Saturday.Click += async (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Saturday.IsEnabled = Saturday.Checked;
                    await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID);
                };
                var SaturdayToHomeTimeStack = new LinearLayout(context);
                var CurrentSaturdayToHomeTime = new TextView(context);
                CurrentSaturdayToHomeTime.Text = schedul.Saturday.ToHomeHlp.ToString();
                var SetSaturdayToHomeTime = new Button(context);
                //SetSaturdayToHomeTime.Text = "!";

                SetSaturdayToHomeTime.LayoutParameters = imgViewParamss;
                SetSaturdayToHomeTime.Gravity = Android.Views.GravityFlags.Center;
                SetSaturdayToHomeTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                SaturdayToHomeTimeStack.AddView(CurrentSaturdayToHomeTime);
                SaturdayToHomeTimeStack.AddView(SetSaturdayToHomeTime);

                var SaturdayToWorkTimeStack = new LinearLayout(context);
                var CurrentSaturdayToWorkTime = new TextView(context);
                CurrentSaturdayToWorkTime.Text = schedul.Saturday.ToWorkHlp.ToString();
                var SetSaturdayToWorkTime = new Button(context);
                //SetSaturdayToWorkTime.Text = "!";

                SetSaturdayToWorkTime.LayoutParameters = imgViewParamss;
                SetSaturdayToWorkTime.Gravity = Android.Views.GravityFlags.Center;
                SetSaturdayToWorkTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                SaturdayToWorkTimeStack.AddView(CurrentSaturdayToWorkTime);
                SaturdayToWorkTimeStack.AddView(SetSaturdayToWorkTime);

                SaturdayStack.AddView(SaturdayToHomeTimeStack);
                SaturdayStack.AddView(SaturdayToWorkTimeStack);


                DaysStack.AddView(SaturdayStack);
                /////////////////////////////
                var SundayStack = new LinearLayout(context);
                SundayStack.Orientation = Orientation.Vertical;

                var Sunday = new ToggleButton(context);
                Sunday.TextOn = "Sunday";
                Sunday.TextOff = "Sunday";
                SundayStack.AddView(Sunday);
                Sunday.Checked = schedul.Sunday.IsEnabled;
                Sunday.Click += async (ev, ar) =>
                {
                    SelectedScheduleID = schedul.ScheduleID;
                    Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID).Sunday.IsEnabled = Sunday.Checked;
                    await Shared.ViewModel.UserSchedule.Update(SelectedScheduleID);
                };
                var SundayToHomeTimeStack = new LinearLayout(context);
                var CurrentSundayToHomeTime = new TextView(context);
                CurrentSundayToHomeTime.Text = schedul.Sunday.ToHomeHlp.ToString();
                var SetSundayToHomeTime = new Button(context);
                //SetSundayToHomeTime.Text = "!";

                SetSundayToHomeTime.LayoutParameters = imgViewParamss;
                SetSundayToHomeTime.Gravity = Android.Views.GravityFlags.Center;
                SetSundayToHomeTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                SundayToHomeTimeStack.AddView(CurrentSundayToHomeTime);
                SundayToHomeTimeStack.AddView(SetSundayToHomeTime);

                var SundayToWorkTimeStack = new LinearLayout(context);
                var CurrentSundayToWorkTime = new TextView(context);
                CurrentSundayToWorkTime.Text = schedul.Sunday.ToWorkHlp.ToString();
                var SetSundayToWorkTime = new Button(context);
                //SetSundayToWorkTime.Text = "!";


                SetSundayToWorkTime.LayoutParameters = imgViewParamss;
                SetSundayToWorkTime.Gravity = Android.Views.GravityFlags.Center;
                SetSundayToWorkTime.LayoutParameters = new LinearLayout.LayoutParams(80, 80);
                SundayToWorkTimeStack.AddView(CurrentSundayToWorkTime);
                SundayToWorkTimeStack.AddView(SetSundayToWorkTime);

                SundayStack.AddView(SundayToHomeTimeStack);
                SundayStack.AddView(SundayToWorkTimeStack);


                DaysStack.AddView(SundayStack);

                var RemoveWeek = new Button(context);
                
                RemoveWeek.Text = "X";
                var imgViewParam = new LinearLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.MatchParent);
                imgViewParam.Gravity= GravityFlags.Center;
                imgViewParam.Width = 90;
                imgViewParam.Height = 90;
                RemoveWeek.LayoutParameters = imgViewParam;
                //RemoveWeek.Gravity = Android.Views.GravityFlags.Center;
               
                RemoveWeek.Click += async (ev, ar) =>
                {
                    NextWeekScroll.Visibility = ViewStates.Gone;
                    SelectedScheduleID = schedul.ScheduleID;
                    var res = await Shared.Model.Requests.DeleteSchedule(SelectedScheduleID);
                    if (res.Item1 == true)
                    {
                        //Shared.Actions.refreshSchedules();
                        try
                        {
                            var thisweek = Shared.ModelView.UserSchedule.Default.First(req => req.ScheduleID == SelectedScheduleID);
                            Shared.ModelView.UserSchedule.Default.Remove(thisweek);
                            NextWeekScroll.Visibility = ViewStates.Gone;

                            //Shared.Actions.refreshSchedules();
                        }
                        catch { }

                    }
                    //else {
                    //    Shared.Actions.refreshSchedules(); 
                    //}
                };
                DaysStack.AddView(RemoveWeek);

                ///////////////////////////////////////////
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

                if (ToHomeFlag == 1 && ToWorkfag == 1)
                {
                    MondayStack.Visibility = ViewStates.Gone;
                    TuesdayStack.Visibility = ViewStates.Gone;
                    WednesdayStack.Visibility = ViewStates.Gone;
                    ThursdayStack.Visibility = ViewStates.Gone;
                    FridayStack.Visibility = ViewStates.Gone;
                    SaturdayStack.Visibility = ViewStates.Gone;
                    SundayStack.Visibility = ViewStates.Gone;

                    WeekStack.Visibility = ViewStates.Visible;
                }
                Week.Click += (orb, arg) =>
                {
                    MondayStack.Visibility = ViewStates.Visible;
                    TuesdayStack.Visibility = ViewStates.Visible;
                    WednesdayStack.Visibility = ViewStates.Visible;
                    ThursdayStack.Visibility = ViewStates.Visible;
                    FridayStack.Visibility = ViewStates.Visible;
                    SaturdayStack.Visibility = ViewStates.Visible;
                    SundayStack.Visibility = ViewStates.Visible;

                    WeekStack.Visibility = ViewStates.Gone;

                    SetMondayToHomeTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Monday.ToHomeHlp.Hours, schedul.Monday.ToHomeHlp.Minutes, true)).Show();
                        MondayToHomeChanged = true;
                    };
                    SetMondayToWorkTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Monday.ToWorkHlp.Hours, schedul.Monday.ToWorkHlp.Minutes, true)).Show();
                        MondayToWorkChanged = true;
                    };
                    SetTuesdayToHomeTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Tuesday.ToHomeHlp.Hours, schedul.Tuesday.ToHomeHlp.Minutes, true)).Show();
                        TuesdayToHomeChanged = true;
                    };
                    SetTuesdayToWorkTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Tuesday.ToWorkHlp.Hours, schedul.Tuesday.ToWorkHlp.Minutes, true)).Show();
                        TuesdayToWorkChanged = true;
                    };
                    SetWednesdayToHomeTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Wednesday.ToHomeHlp.Hours, schedul.Wednesday.ToHomeHlp.Minutes, true)).Show();
                        WednesdayToHomeChanged = true;
                    };
                    SetWednesdayToWorkTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Wednesday.ToWorkHlp.Hours, schedul.Wednesday.ToWorkHlp.Minutes, true)).Show();
                        WednesdayToWorkChanged = true;
                    };
                    SetThursdayToHomeTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Thursday.ToHomeHlp.Hours, schedul.Thursday.ToHomeHlp.Minutes, true)).Show();
                        ThursdayToHomeChanged = true;
                    };
                    SetThursdayToWorkTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Thursday.ToWorkHlp.Hours, schedul.Thursday.ToWorkHlp.Minutes, true)).Show();
                        ThursdayToWorkChanged = true;
                    };
                    SetFridayToHomeTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Friday.ToHomeHlp.Hours, schedul.Friday.ToHomeHlp.Minutes, true)).Show();
                        FridayToHomeChanged = true;
                    };
                    SetFridayToWorkTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Friday.ToWorkHlp.Hours, schedul.Friday.ToWorkHlp.Minutes, true)).Show();
                        FridayToWorkChanged = true;
                    };
                    SetSaturdayToHomeTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Saturday.ToHomeHlp.Hours, schedul.Saturday.ToHomeHlp.Minutes, true)).Show();
                        SaturdayToHomeChanged = true;
                    };

                    SetSaturdayToWorkTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Saturday.ToWorkHlp.Hours, schedul.Saturday.ToWorkHlp.Minutes, true)).Show();
                        SaturdayToWorkChanged = true;
                    };
                    SetSundayToHomeTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Sunday.ToHomeHlp.Hours, schedul.Sunday.ToHomeHlp.Minutes, true)).Show();
                        SundayToHomeChanged = true;
                    };
                    SetSundayToWorkTime.Click += (ev, ar) =>
                    {
                        SelectedScheduleID = schedul.ScheduleID;
                        (new TimePickerDialog(context, TimePickerCallback, schedul.Sunday.ToWorkHlp.Hours, schedul.Sunday.ToWorkHlp.Minutes, true)).Show();
                        SundayToWorkChanged = true;
                    };
                };
                ////
                NextWeekScroll.AddView(DaysStack);
            }
            catch { }
            return NextWeekScroll;


        }
    }
}
