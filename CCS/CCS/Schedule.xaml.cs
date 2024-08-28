using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CCS
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Schedule : ContentPage
	{
		public Schedule ()
		{
			InitializeComponent ();
            LoadSchedules();
            //ScheduleGrid.BindingContext = Shared.ModelView.UserSchedule.Default;
            

        }

        async void LoadSchedules() {
            await Shared.ViewModel.UserSchedule.Load();
            ScheduleGrid.BindingContext = Shared.ModelView.UserSchedule.Default;
        }

        private async void UpdateSchedule(object sender, EventArgs e)
        {
            await Shared.ViewModel.UserSchedule.Update();
        }
    }
}