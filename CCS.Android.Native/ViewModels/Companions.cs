using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Android.Gms.Maps;
using Android.Widget;

namespace Shared.ViewModel
{
    class Companions
    {

        public static async void FindCompanions(GoogleMap maplayer, bool isWayToHome, DateTime date, ProgressBar progressBar, TextView Output)        {

            Shared.View.General.inLoading(progressBar,Output);
            var Request = await Model.Requests.StartFindCompanions(0.0, 0.0, isWayToHome, date);
            Shared.View.General.outLoading(progressBar);
            if (Request.Item1 == true)            {

                maplayer.Clear();
                View.MapsSymbols.ShowCompanions(maplayer, Request.Item2);
            }
        }

        public static async Task<Model.UserCompanion> GetCompanionInfo(int CompanionID)
        {
            if (Shared.Model.General.CompanionsCache.Any(req => req.UID == CompanionID))
            {
                return Shared.Model.General.CompanionsCache.First(req => req.UID == CompanionID);
            }
            else
            {
                //Shared.View.General.inLoading(progressBar, Output);
                var Request = await Model.Requests.GetUserInfo(CompanionID);
                //Shared.View.General.outLoading(progressBar);
                if (Request.Item1 == true)
                {
                    Shared.Model.General.CompanionsCache.Add(Request.Item2);
                    return Request.Item2;
                }
                else return new Model.UserCompanion();
            }
        }
    }
}
