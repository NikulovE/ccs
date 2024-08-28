using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ViewModel
{
    class Messages
    {
        public static async Task<bool> Load(ProgressBar progressBar, TextView output)
        {

            Shared.View.General.inLoading(progressBar, output);
            var query = await Model.Requests.LoadMessages();
            Shared.View.General.outLoading(progressBar);
            if (query.Item1 == true)
            {
                Shared.ModelView.UIBinding.Default.Messages = query.Item2;

                return true;
            }
            else
            {
                return false;
            }

        }

        public static async Task<bool> Send(ProgressBar progressBar, TextView output, int toUID, String Text)
        {
            Shared.View.General.inLoading(progressBar, output);
            var query = await Model.Requests.SendMessage(toUID, Text);
            Shared.View.General.outLoading(progressBar);
            if (query.Item1 == true)
            {
                Shared.Actions.refreshMessages();
                try
                {
                    Shared.Actions.SentMessage(toUID);
                }
                catch { }
                return true;
            }
            else
            {
                output.Text = ConvertMessages.Message(query.Item2);
                return false;
            }
        }
    }
}
