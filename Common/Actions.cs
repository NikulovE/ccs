using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    class Actions
    {

        public static Action showTripPointonMap;
        public static Action showTripOffers;
        public static Action refreshOffices;
        public static Action refreshOrganizations;
        public static Action refreshMessages;
        public static Action initializeMap;
        public static Action InitializeGPS;
        public static Action<int> SentMessage;
        public static Action<int> UpdateTrip;
#if XAMARIN
        public static Action refreshTrips;
        public static Action loadHome;


#else
        public static Action checkWizard;
        


        public static Action refreshUserOffice;
        public static Action refreshRoutePoints;

#endif

#if ANDROID
        public static Action loadHome;
        public static Action HomeLoaded;
        public static Action showConfirmationGrid;
        public static Action showFillingProfileGrid;
        public static Action showRegistrationGrid;
        public static Action showMainGrid;
        public static Action showSelectRoleGrid;

        public static Action refreshTrips;
        public static Action refreshSchedules;
        public static Action rewrite;

        public static Action RegisterInAzureHub;
        //public static Action RegisterInAzureHub;
#endif
    }
}
