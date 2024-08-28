using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
//using Android.Views;
using Android.Widget;
using Shared;
using static Android.Widget.GridLayout;

namespace CCS.Android.Native
{
    [Activity(Label = "Chat")]
    public class ChatActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Chat);
            Shared.Actions.refreshMessages = LoadMessages;
            InitializeUI();
            LoadMessages();
        }

        private void InitializeUI()
        {
            
        }


        private void LoadMessages() {

            RunOnUiThread(async () =>
            {
                var output = FindViewById<TextView>(Resource.Id.SystemOut);
                var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
                if (await Shared.ViewModel.Messages.Load(progressBar, output))
                {
                    ShowMessages();
                    //ShowMe(output,progressBar,messagesstack);
                }
            });
        }

        private void ShowMessages() {
            var output = FindViewById<TextView>(Resource.Id.SystemOut);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            var messagesstack = FindViewById<ScrollView>(Resource.Id.Messages);
            messagesstack.RemoveAllViews();
            var imgViewParams = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.WrapContent);
            var ConversationStack = new LinearLayout(this);
            foreach (var Converstation in Shared.ModelView.UIBinding.Default.Messages) {
                
                ConversationStack.LayoutParameters = imgViewParams;
                ConversationStack.Orientation = Orientation.Vertical;
                var ConverationWIth = Converstation.With;




                TextView Label = new TextView(this);
                Label.TextSize = 19;
                Label.Text = ConverationWIth;

                ConversationStack.AddView(Label);

                var SendMessageStack = new GridLayout(this);
                SendMessageStack.ColumnCount = 2;
                //SendMessageStack.LayoutParameters = imgViewParams;

                GridLayout.LayoutParams param2 = new GridLayout.LayoutParams();
                param2.ColumnSpec = GridLayout.InvokeSpec(0);
                param2.SetGravity(GravityFlags.Fill);
                var NewMessage = new EditText(this);
                //NewMessage.Gravity = GravityFlags.Fill;
                NewMessage.LayoutParameters = param2;
                var Send = new Button(this);
                Send.LayoutParameters = new LinearLayout.LayoutParams(100, 100);
                Send.Text = ">";
                GridLayout.LayoutParams param = new GridLayout.LayoutParams();
                param.ColumnSpec = GridLayout.InvokeSpec(1);
                Send.LayoutParameters = param;

                SendMessageStack.AddView(NewMessage);
                SendMessageStack.AddView(Send);
                Send.Click += async (ev, ar) =>
                {
                    Send.Enabled = false;
                    if (await Shared.ViewModel.Messages.Send(progressBar, output, Converstation.WithUID, NewMessage.Text))
                    {
                        NewMessage.Text = "";
                        Send.Enabled = true;
                    }
                    else {
                        Send.Enabled = true;
                    }
                };


                ConversationStack.AddView(SendMessageStack);

                //var MessagesStack = new ScrollView(this);
                foreach (var mess in Converstation.UserConversation) {
                    var imgViewParamss = new LinearLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.WrapContent);
                    imgViewParamss.SetMargins(30, 0, 0, 0);
                    //ar u = new LayoutParams();
                    var Who = new TextView(this);
                    Who.SetTypeface(null, TypefaceStyle.Bold);
                    Who.Text = mess.From;
                    Who.LayoutParameters = imgViewParamss;

                    var Text = new TextView(this);
                    Text.Text = mess.MessageText;

                    if (mess.SysCode != 0)
                    {
                        Text.Text = SystemMess(mess.SysCode);
                    }
                    Text.LayoutParameters = imgViewParamss;
                    var When = new TextView(this);
                    var ts = new DateTime(mess.TimeStamp).ToLocalTime();
                    When.Text = ts.ToShortDateString() + " " + ts.ToShortTimeString();
                    var imgViewParamsss = new LinearLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.WrapContent);
                    imgViewParamsss.SetMargins(30, 0, 0, 10);
                    When.LayoutParameters = imgViewParamsss;

                    ConversationStack.AddView(Who);
                    ConversationStack.AddView(Text);
                    ConversationStack.AddView(When);
                }

                
                
            }
            messagesstack.AddView(ConversationStack);
        }

        String SystemMess(int value) {
            var code = (int)value;
                switch (code) {
                    default:
                        return ConvertMessages.Message("x51000");
                    case 0:
                        return ConvertMessages.Message("x51000");
                    case 1:
                        return ConvertMessages.Message("x51001");
                    case 2:
                        return ConvertMessages.Message("x51002");
                    case 3:
                        return ConvertMessages.Message("x51003");
                    case 4:
                        return ConvertMessages.Message("x51004");
                    case 5:
                        return ConvertMessages.Message("x51005");
                    case 6:
                        return ConvertMessages.Message("x51006");
                    case 7:
                        return ConvertMessages.Message("x51007");
                    case 8:
                        return ConvertMessages.Message("x51008");
                }
        }
    }
}