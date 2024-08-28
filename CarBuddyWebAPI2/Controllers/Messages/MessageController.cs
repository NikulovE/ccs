using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers.Messages
{
    public class MessageController : ApiController
    {
        [HttpGet]
        public Tuple<bool, List<Conversation>> LoadMessages(int SessionID, string Sign)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var messages = new MessagesProcessing();
                if (messages.UserMessages())
                {
                    try
                    {
                        return new Tuple<bool, List<Conversation>>(true, messages.Convesations);
                    }
                    catch
                    {
                        return new Tuple<bool, List<Conversation>>(false, new List<Conversation>());
                    }
                }
                else return new Tuple<bool, List<Conversation>>(false, new List<Conversation>());
            }
            else
            {
                return new Tuple<bool, List<Conversation>>(false, new List<Conversation>());
            }
        }
        [HttpPost]
        public async Task<Tuple<bool, string>> SendMessage(int SessionID, string Sign, int OpponentUID, [FromBody]string Text)
        {
            if(OpponentUID==0) return new Tuple<bool, String>(true, "x24001");
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                if (Text.Length > 300) return new Tuple<bool, String>(false, "x24003");
                var messages = new MessagesProcessing();
                if (await messages.SendMessage(App.UID, OpponentUID, Text))
                {
                    try
                    {
                        return new Tuple<bool, String>(true, "x24001");
                    }
                    catch
                    {
                        return new Tuple<bool, String>(false, "x24002");
                    }
                }
                else return new Tuple<bool, String>(false, "x24002");

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }
}
