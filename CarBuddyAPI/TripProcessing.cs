using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBuddyAPI
{
    public class TripProcessing
    {
        public int PassengerUID;
        public int DriverUID;
        public bool isFromDriver;
        public bool isToHome;
        double latitude;
        double longtitude;

        public List<TripOffer> Offers = new List<TripOffer>();

        public bool SendOffer(int toUID, int RouteID,DateTime atTime)
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                try
                {
                    bool isUserDriver = dbo.Users.First(req => req.UID == App.UID).isDriver.Value;
                    var NewTrip = new Trip();
                    if (isUserDriver)
                    {
                        DriverUID = App.UID;
                        PassengerUID = toUID;
                        isFromDriver = true;

                    }
                    else
                    {
                        PassengerUID = App.UID;
                        DriverUID = toUID;
                        isFromDriver = false;
                    }

                    NewTrip.isFromDriver = isFromDriver;
                    NewTrip.PassengerUID = PassengerUID;
                    NewTrip.DriverUID = DriverUID;
                    NewTrip.isToHome = isToHome;


                    NewTrip.isConfirmed = false;
                    var CurrentDayOfWeek = atTime.DayOfWeek;
                    var CurrentDate = atTime.Date;
                    TimeSpan FriendStartTime = new TimeSpan();
                    switch (CurrentDayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            FriendStartTime = isToHome ? dbo.Schedules.First(req => req.UID == toUID).MonToHome : dbo.Schedules.First(req => req.UID == toUID).MonToWork;
                            break;
                        case DayOfWeek.Tuesday:
                            FriendStartTime = isToHome ? dbo.Schedules.First(req => req.UID == toUID).TueToHome : dbo.Schedules.First(req => req.UID == toUID).TueToWork;
                            break;
                        case DayOfWeek.Wednesday:
                            FriendStartTime = isToHome ? dbo.Schedules.First(req => req.UID == toUID).WedToHome : dbo.Schedules.First(req => req.UID == toUID).WedToWork;
                            break;
                        case DayOfWeek.Thursday:
                            FriendStartTime = isToHome ? dbo.Schedules.First(req => req.UID == toUID).ThuToHome : dbo.Schedules.First(req => req.UID == toUID).ThuToWork;
                            break;
                        case DayOfWeek.Friday:
                            FriendStartTime = isToHome ? dbo.Schedules.First(req => req.UID == toUID).FriToHome : dbo.Schedules.First(req => req.UID == toUID).FriToWork;
                            break;
                        case DayOfWeek.Saturday:
                            FriendStartTime = isToHome ? dbo.Schedules.First(req => req.UID == toUID).SatToHome : dbo.Schedules.First(req => req.UID == toUID).SatToWork;
                            break;
                        case DayOfWeek.Sunday:
                            FriendStartTime = isToHome ? dbo.Schedules.First(req => req.UID == toUID).SunToHome : dbo.Schedules.First(req => req.UID == toUID).SunToWork;
                            break;
                    }
                    var StartTime = atTime.TimeOfDay < FriendStartTime ? CurrentDate + FriendStartTime : CurrentDate.AddDays(1) + FriendStartTime;
                    NewTrip.PlannedStartTime = StartTime;
                    if (RouteID == 0)
                    {
                        var HomeLocation = dbo.Homes.First(req => req.UID == toUID);
                        latitude = HomeLocation.latitude;
                        longtitude = HomeLocation.longtitude;
                    }
                    else
                    {
                        var RoutePointLocation = dbo.RoutePoints.First(req => req.UID == toUID && req.RoutePointId== RouteID);
                        latitude = RoutePointLocation.latitude;
                        longtitude = RoutePointLocation.longtitude;
                    }
                    NewTrip.Latitude = latitude;
                    NewTrip.Longtitude = longtitude;
                    NewTrip.isRepeat = false;
                    try
                    {
                        var previous = dbo.Trips.Where(req => req.DriverUID == DriverUID && req.PassengerUID == PassengerUID && req.PlannedStartTime == StartTime && req.isToHome == isToHome && req.isFromDriver == isFromDriver);
                        dbo.Trips.DeleteAllOnSubmit(previous);
                    }
                    catch (Exception)
                    {

                    }
                    dbo.Trips.InsertOnSubmit(NewTrip);
                    dbo.SubmitChanges();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally {
                    MessagesProcessing.SendSysMessage(App.UID, toUID, "", 1);
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

        public bool LoadOffers(DateTime CurrentUserTime) {
            var dbo = new CarBuddyDataContext();
            try
            {
                try
                {
                    var trips = dbo.Trips.Where(req => (req.DriverUID == App.UID || req.PassengerUID == App.UID) && req.PlannedStartTime > CurrentUserTime  );
                    bool isUserDriver = dbo.Users.First(req => req.UID == App.UID).isDriver.Value;
                    foreach (var trip in trips) {
                        var NextOffer = new TripOffer();
                        //NextOffer.Driver = trip.Driver.FirstName + " " + trip.Driver.LastName;
                        NextOffer.Companion = isUserDriver ? trip.Passenger.FirstName + " " + trip.Passenger.LastName : trip.Driver.FirstName + " " + trip.Driver.LastName;
                        NextOffer.CompanionID = isUserDriver ? trip.PassengerUID : trip.DriverUID;
                        NextOffer.Latitude = trip.Latitude;
                        NextOffer.Longtitude = trip.Longtitude;
                        NextOffer.OfferID = trip.Id;
                        NextOffer.StartTime = trip.PlannedStartTime;
                        NextOffer.IsToHome = trip.isToHome;
                        NextOffer.Confirmed = trip.isConfirmed;
                        NextOffer.IsCanBeAccepted = trip.isFromDriver == isUserDriver  ? false : true;
                        if (trip.isConfirmed) NextOffer.IsCanBeAccepted = false;
                        Offers.Add(NextOffer);
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

        public bool AcceptOffer(int OfferID)
        {
            
            var dbo = new CarBuddyDataContext();
            try
            {
                try
                {

                    var trip = dbo.Trips.First(req => req.Id == OfferID);
                    bool isUserDriver = dbo.Users.First(req => req.UID == App.UID).isDriver.Value;
                    if (isUserDriver && trip.DriverUID != App.UID) return false;
                    if (!isUserDriver && trip.PassengerUID != App.UID) return false;
                    if (trip.isConfirmed) return true;
                    int companion = isUserDriver ? trip.PassengerUID : trip.DriverUID;
                    trip.isConfirmed = true;
                    dbo.SubmitChanges();
                    MessagesProcessing.SendSysMessage(App.UID, companion, "", 5);
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

        public bool RejectOffer(int OfferID)
        {
            var dbo = new CarBuddyDataContext();
            try
            {
                try
                {
                    var trip = dbo.Trips.First(req => req.Id == OfferID);
                    bool isUserDriver = dbo.Users.First(req => req.UID == App.UID).isDriver.Value;
                    if (isUserDriver && trip.DriverUID != App.UID) return false;
                    if (!isUserDriver && trip.PassengerUID != App.UID) return false;
                    dbo.Trips.DeleteOnSubmit(trip);
                    int companion = isUserDriver ? trip.PassengerUID : trip.DriverUID;
                    MessagesProcessing.SendSysMessage(App.UID, companion, "", 3);
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

        public static bool CancelEveryOffer() {
            var dbo = new CarBuddyDataContext();
            try
            {
                var userTrips = dbo.Trips.Where(req => (req.DriverUID == App.UID || req.PassengerUID == App.UID));
                dbo.Trips.DeleteAllOnSubmit(userTrips);
                dbo.SubmitChanges();
                return true;
            }
            catch {
                return false;
            }
            finally {
                dbo.Connection.Close();
            }
        }

    }
}