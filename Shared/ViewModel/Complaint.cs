using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModel
{
    class Complaint
    {
        public static async void WrongOffice(int Office)
        {
            Shared.View.General.inLoading();
            var apiflow = await Model.Requests.NewComplaint(1, Office);
            Shared.View.General.outLoading();
            if (apiflow.Item1 == true)
            {
                Actions.refreshOffices();
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(apiflow.Item2);
            }
        }
    }
}
