using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
{
    public class MessagesProcessing
    {
        public List<Conversation> Convesations = new List<Conversation>();

        public bool UserMessages() {
            var dbo = new CarBuddyDataContext();
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
                                Message.TimeStamp = message.Date;
                                Message.FromUID = message.From;
                                Message.ToUID = message.To;
                                MessagesAr.Add(Message);
                            }
                        }
                        conver.UserConversation = MessagesAr;
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

        public bool SendMessage(int fromUID,int toUID, String text, int SysCode=0)
        {
            var dbo = new CarBuddyDataContext();
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

        public static bool SendSysMessage(int fromUID, int toUID, String text, int SysCode = 0)
        {
            var dbo = new CarBuddyDataContext();
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