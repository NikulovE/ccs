﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class LocationProcessing
    {
        public double longtitude;
        public double latitude;
        public bool UpdateLocation() {
            var dbo = new AppDbDataContext();
            try
            {
                var Location = new CurrentPosition();
                try
                {
                    Location = dbo.CurrentPositions.First(req => req.UID == App.UID);
                }
                catch (Exception)
                {
                    var newLocaiton = new CurrentPosition();
                    newLocaiton.UID = App.UID;
                    newLocaiton.longitude = longtitude;
                    newLocaiton.latitude = latitude;
                    try
                    {
                        dbo.CurrentPositions.InsertOnSubmit(newLocaiton);
                        dbo.SubmitChanges();
                        return true;
                    }
                    catch (Exception) {
                        return false;
                    }
                   
                }
                Location.longitude = longtitude;
                Location.latitude = latitude;
                dbo.SubmitChanges();
                return true;                
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

        public bool LoadLocation()
        {
            var dbo = new AppDbDataContext();
            try
            {
                try
                {
                    var Location = dbo.CurrentPositions.First(req => req.UID == App.UID);
                    longtitude = Location.longitude.Value;
                    latitude = Location.latitude.Value;
                    return true;
                }
                catch (Exception)
                {
                    try
                    {
                        var Location = dbo.Homes.First(req => req.UID == App.UID);
                        longtitude = Location.longtitude;
                        latitude = Location.latitude;
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;

                    }


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