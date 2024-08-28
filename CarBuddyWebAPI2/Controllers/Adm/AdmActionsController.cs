using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;
//using WebAPI2.Models;

namespace WebAPI2.Controllers.Adm
{
    public class AdmActionsController : ApiController
    {
        //public bool Get()
        //{
        //    var dbo = new AppDbDataContext();
        //    var CurrentRoutePoints = dbo.RoutePoints;
        //    foreach (var rp in CurrentRoutePoints)
        //    {
        //        var forUID = rp.UID;
        //        var way = rp.way;

        //        if (dbo.Paths.Any(req => req.UID == forUID && req.way == way))
        //        {
        //            rp.PathID = dbo.Paths.First(req => req.UID == forUID && req.way == way).PathID;

        //        }
        //    }
        //    dbo.SubmitChanges();
        //    return true;
        //    //var dbo = new AppDbDataContext();
        //    //var CurrentRoutePoints = dbo.RoutePoints;
        //    //foreach (var rp in CurrentRoutePoints) {
        //    //    var forUID = rp.UID;
        //    //    var way = rp.way;
        //    //    var NewPath = new Path();
        //    //    NewPath.IsMon = true;
        //    //    NewPath.IsTue = true;
        //    //    NewPath.IsWed = true;
        //    //    NewPath.IsThu = true;
        //    //    NewPath.IsFri = true;
        //    //    NewPath.IsSat = false;
        //    //    NewPath.IsSun = false;

        //    //    NewPath.PathName = "";
        //    //    NewPath.UID = forUID;
        //    //    NewPath.way = way.Value;
                

        //    //    if (!dbo.Paths.Any(req => req.UID == forUID && req.way == way))
        //    //    {
        //    //        dbo.Paths.InsertOnSubmit(NewPath);
        //    //        dbo.SubmitChanges();
        //    //    }              
                
                
                

        //    //}
        //    //return true;
        //}
        //public String Get(int x)
        //{
        //    var dbo = new AppDbDataContext();
        //    var CurrentRoutePoints = dbo.RoutePoints;
        //    foreach (var rp in CurrentRoutePoints)
        //    {
        //        var forUID = rp.UID;
        //        var way = rp.way;

        //        if (dbo.Paths.Any(req => req.UID == forUID && req.way == way))
        //        {
        //            rp.PathID = dbo.Paths.First(req => req.UID == forUID && req.way == way).PathID;
                    
        //        }
        //    }
        //    dbo.SubmitChanges();
        //    return "SHIT";
        //}
    }
}
