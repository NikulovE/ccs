using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
{
    public class FeedbackProcessing
    {
        public static List<AppFeedback> Load() {
            var Result = new List<AppFeedback>();
            try
            {
                var dbo = new CarBuddyDataContext();
                var feedbackgs = dbo.AppFreedBacks;
                foreach (var feedback in feedbackgs)
                {
                    var nextfeedback = new AppFeedback();
                    nextfeedback.DeveloperAnswer = feedback.DeveloperAnswer;
                    nextfeedback.Stars = feedback.Star.Value;
                    nextfeedback.FeedbackText = feedback.Feedback;
                    nextfeedback.UID = feedback.FromUID;
                    var s = dbo.Users.First(req => req.UID == feedback.FromUID);
                    nextfeedback.UserName = s.FirstName + " " + s.LastName;
                    Result.Add(nextfeedback);
                }
            }
            catch (Exception)
            {

            }
            return Result;

        }

        public static bool Save(string Feedback, byte Stars)
        {
            var Result = new List<AppFeedback>();
            try
            {
                var dbo = new CarBuddyDataContext();

                if (Stars > 5) Stars = 5;
                try {
                    var previousfeedback = dbo.AppFreedBacks.First(req => req.FromUID == App.UID);
                    
                    previousfeedback.Star = Stars;
                    previousfeedback.Feedback = Feedback;
                    try
                    {
                        dbo.SubmitChanges();
                        return true;
                    }
                    catch (Exception) {
                        return false;
                    }
                }
                catch (Exception) { }

                var nextfeedback = new AppFreedBack();
                nextfeedback.Star = Stars;
                nextfeedback.Feedback = Feedback.ToString();
                nextfeedback.FromUID = App.UID;
                dbo.AppFreedBacks.InsertOnSubmit(nextfeedback);
                dbo.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            

        }


    }
}