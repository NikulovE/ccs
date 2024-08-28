using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ViewModel
{
    class Messages
    {
        public static async Task<bool> Load()
        {

            Shared.View.General.inLoading();
            var query = await Model.Requests.LoadMessages();
            Shared.View.General.outLoading();
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

        public static async Task<bool> Send(int toUID, String Text)
        {
            Shared.View.General.inLoading();
            var query = await Model.Requests.SendMessage(toUID, Text);
            Shared.View.General.outLoading();
            if (query.Item1 == true)
            {
                await Load();
                try
                {
                    Shared.Actions.SentMessage(toUID);
                }
                catch { }
                return true;
            }
            else
            {
                Shared.ModelView.UIBinding.Default.OutPut = ConvertMessages.Message(query.Item2);
                return false;
            }
        }
    }
}
