using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebAPI2.Controllers;

namespace WebAPI2.Models
{
    public class MessagesProcessing
    {
        public List<Conversation> Convesations = new List<Conversation>();

        public bool UserMessages() {
            var dbo = new AppDbDataContext();
            try
            {
                try
                {
                    var Messages = dbo.Messages.Where(req => req.To == App.UID || req.From == App.UID).OrderByDescending(reqx => reqx.Date).Take(25);//.Reverse();

                    foreach (var message in Messages)
                    {
                        var conver = new Conversation();
                        conver.WithUID = message.From != App.UID ? message.From : message.To;
                        conver.With = message.From != App.UID ? message.FromUser.FirstName + " " + message.FromUser.LastName : message.ToUser.FirstName + " " + message.ToUser.LastName;
                        if(!Convesations.Any(req=>req.WithUID==conver.WithUID))  Convesations.Add(conver);
                    }

                    foreach (var conver in Convesations){
                        var MessagesAr = new List<UserMessage>();
                        foreach (var message in Messages)
                        {
                            if (message.From == conver.WithUID || message.To == conver.WithUID)
                            {
                                var Message = new UserMessage();
                                Message.From = message.FromUser.FirstName + " " + message.FromUser.LastName;
                                Message.To = message.ToUser.FirstName + " " + message.ToUser.LastName;
                                Message.SysCode = message.Code.Value;
                                Message.MessageText = message.Text;
                                Message.TimeStamp = message.Date.Ticks;
                                Message.FromUID = message.From;
                                Message.ToUID = message.To;
                                MessagesAr.Add(Message);
                            }
                        }
                        conver.UserConversation = MessagesAr;
                    }
                    if (Convesations.Count() == 0) {
                        var x = new Conversation();
                        var Message = new UserMessage();
                        
                        Message.From = "System";
                        //Message.To =
                        Message.SysCode = 0;
                        Message.MessageText = "Send invite to start conversation";
                        Message.TimeStamp = DateTime.UtcNow.Ticks;
                        Message.FromUID = 0;
                        Message.ToUID = App.UID;
                        x.UserConversation.Add(Message);
                        Convesations.Add(x);
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;

                }

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                dbo.Connection.Close();
            }

        }

        public async Task<bool> SendMessage(int fromUID,int toUID, String text, int SysCode=0)
        {
            var dbo = new AppDbDataContext();
            try
            {
                try
                {
                    var NewMessage = new Message();
                    NewMessage.To = toUID;
                    NewMessage.From = fromUID;
                    NewMessage.Text = text;
                    NewMessage.Code = (byte)SysCode;
                    NewMessage.Date = DateTime.UtcNow;
                    dbo.Messages.InsertOnSubmit(NewMessage);
                    dbo.SubmitChanges();
                    try
                    {
                       
                        //try
                        //{
                            //var push = new NotificationsController();
                        
                        //}
                        //catch { };
                        //Shared.SendingMail.SendEmail(mailbox, fromname + " sent message for you", fromname + " sent for you (Commute Car Sharing)");
                    }
                    catch { }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                //var mailbox = dbo.Registrations.First(req => req.UID == toUID).Mail;
                var from = dbo.Users.First(req => req.UID == fromUID);
                var fromname = from.FirstName + " " + from.LastName;
                //var s = new NotificationsController();
                await NotificationsController.SendMessage(toUID, fromname + " sent message for you");
                dbo.Connection.Close();
            }

        }

        public static async Task<bool> SendSysMessage(int fromUID, int toUID, String text, int SysCode = 0)
        {
            var dbo = new AppDbDataContext();
            try
            {
                try
                {
                    var NewMessage = new Message();
                    NewMessage.To = toUID;
                    NewMessage.From = fromUID;
                    NewMessage.Text = text;
                    NewMessage.Code = (byte)SysCode;
                    NewMessage.Date = DateTime.UtcNow;
                    dbo.Messages.InsertOnSubmit(NewMessage);
                    dbo.SubmitChanges();
                    try {
                        if (SysCode != 0)
                        {
                            //var mailbox = dbo.Registrations.First(req => req.UID == toUID).Mail;
                            var from = dbo.Users.First(req => req.UID == fromUID);
                            var fromname = from.FirstName + " " + from.LastName;
                            switch (SysCode) {
                                case 1:
                                    await NotificationsController.SendMessage(toUID, fromname + " sent offer for you");
                                    break;
                                case 5:
                                    await NotificationsController.SendMessage(toUID, fromname + " accepted your offer");
                                    break;
                                case 3:
                                    await NotificationsController.SendMessage(toUID, fromname + " declined your offer");
                                    break;
                                default:
                                    await NotificationsController.SendMessage(toUID, fromname + " has an update for you");
                                    break;
                            }
                            //if(SysCode==1)   await NotificationsController.SendMessage(toUID, fromname + " has an update for you");
                            //Shared.SendingMail.SendEmail(mailbox, fromname + " has an update for you\nPlease, check application", fromname + " sent message for you (Commute Car Sharing)");
                        }
                    }
                    catch { }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                dbo.Connection.Close();
            }

        }
    }
}