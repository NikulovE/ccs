using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Text;
using System.Windows;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using AppWeb.Models;

namespace AppWeb.Models
{
    class Requests
    {
        public static HttpClient PrepareRequest()
        {

            HttpClient client = new HttpClient();
#if DEBUG
            client.BaseAddress = new Uri("http://localhost:59524/");
#else
            client.BaseAddress = new Uri("http://api.commutecarsharing.ru");
#endif
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static async Task<Tuple<bool, string>> StartRegistration(String Email)
        {

            try
            {
                var content = new StringContent(JsonEngine.Serialize(Email), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await PrepareRequest().PutAsync("api/RegistrationProfile", content);
                string json = null;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    return JsonEngine.Deserialize<Tuple<bool, String>>(json);
                }
                else
                {
                    return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
                }

            }
            catch
            {
                return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
            }
        }

        public static async Task<Tuple<bool, string>> RestoreRegistration(String Email)
        {
            try
            {
                var content = new StringContent(JsonEngine.Serialize(Email), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await PrepareRequest().PostAsync("api/RestoreRegistration", content);
                string json = null;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    return JsonEngine.Deserialize<Tuple<bool, String>>(json);
                }
                else
                {
                    return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
                }

            }
            catch
            {
                return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
            }
        }

        public static async Task<Tuple<bool, string>> ConfirmRegistration(int UID, String Answer)
        {
            try
            {
                var content = new StringContent(JsonEngine.Serialize(Answer), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await PrepareRequest().PostAsync("api/RegistrationProfile/ConfirmRegistration?UID=" + UID, content);
                string json = null;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    return JsonEngine.Deserialize<Tuple<bool, String>>(json);
                }
                else
                {
                    return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
                }

            }
            catch
            {
                return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
            }

        }

        //public static async Task<Tuple<bool, string>> RestoreSessionKey(String EncryptedRequest)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(EncryptedRequest), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/RestoreProfile/RestoreSessionKey?UID=" + LocalStorage.UID, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }

        //}

        //public static async Task<Tuple<bool, string>> Logon(String EncryptedRequest)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(EncryptedRequest), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Profile/Logon?SessionID=" + LocalStorage.SessionID, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }


        //}

        //public static async Task<Tuple<bool, UserProfile>> LoadProfile()
        //{
        //    try
        //    {
        //        var s = General.CreateSign();
        //        var p = Uri.EscapeDataString(s);
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/profile/LoadProfile?SessionID=" + LocalStorage.SessionID + "&Sign=" + p);

        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, UserProfile>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, UserProfile>(false, new UserProfile());
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, UserProfile>(false, new UserProfile());
        //    }
        //}







        //public static async Task<Tuple<bool, string>> UpdateProfile(UserProfile UserProfile)
        //{
        //    try
        //    {
        //        var sign = General.CreateSign();
        //        var urlsign = Uri.EscapeDataString(sign);
        //        var content = new StringContent(JsonEngine.Serialize(UserProfile), Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/UpdateProfile/UpdateProfile?SessionID=" + LocalStorage.SessionID + "&Sign=" + urlsign, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}

        //public static async Task<Tuple<bool, String>> ChangeDriverMode()
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(ModelView.UserProfile.Default.IsDriver), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Profile/ChangeDriverMode?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }

        //}

        //public static async Task<Tuple<bool, UserCar>> LoadCar()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/car/LoadUserCar?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()));

        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, UserCar>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, UserCar>(false, new UserCar());
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, UserCar>(false, new UserCar());
        //    }


        //}

        //public static async Task<Tuple<bool, string>> UpdateCar(UserCar UserCar)
        //{
        //    try
        //    {

        //        var content = new StringContent(JsonEngine.Serialize(UserCar), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/car/UpdateCar?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }


        //}

        //public static async Task<Tuple<bool, string>> StartCompanyRegisration(String email, String name)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(email), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PutAsync("api/OrgRegistration/SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&Name=" + name, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}

        //public static async Task<Tuple<bool, string>> ConfirmCompanyRegistration(String PasswordFromMail, String CompanyDomain)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(General.EncryptString(PasswordFromMail)), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/OrgRegistration/SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&domain=" + CompanyDomain, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }

        //}
        //public static async Task<Tuple<bool, string>> RetrievBingMapsKey()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Maps/BingMapsAPIKey?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()));
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}






        //public static async Task<Tuple<bool, string>> StartJoinToOrganization(String UserEmail)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(UserEmail), Encoding.UTF8, "application/json");
        //        var sign = General.CreateSign();
        //        var urlsign = Uri.EscapeDataString(sign);
        //        HttpResponseMessage response = await PrepareRequest().PutAsync("api/OrganizationMember/StartJoinToOrganization?SessionID=" + LocalStorage.SessionID + "&Sign=" + urlsign, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}


        //public static async Task<Tuple<bool, string>> ConfirmJoinToOrganization(String PasswordFromMail, String CompanyDomain)
        //{

        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(General.EncryptString(PasswordFromMail)), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/OrganizationMember/ConfirmJoinOrganization?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&domain=" + CompanyDomain, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }


        //}

        //public static async Task<Tuple<bool, string>> LeaveOrganization(int OrgID)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(OrgID), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/OrganizationE/LeaveOrganization?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}


        //public static async Task<Tuple<bool, List<UserGroup>>> LoadOrganizations()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Organization/LoadOrganizations?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()));
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, List<UserGroup>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, List<UserGroup>>(false, new List<UserGroup>());
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, List<UserGroup>>(false, new List<UserGroup>());
        //    }
        //}

        //public static async Task<Tuple<bool, List<OfficeOnMap>>> LoadOffices()
        //{
        //    try
        //    {
        //        var s = ModelView.UIBinding.Default.CurrentCenter.Longitude;
        //        var content = new StringContent(JsonEngine.Serialize(new Tuple<double, double>(ModelView.UIBinding.Default.CurrentCenter.Longitude, ModelView.UIBinding.Default.CurrentCenter.Latitude)), Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Office/LoadOffices?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, List<OfficeOnMap>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, List<OfficeOnMap>>(false, new List<OfficeOnMap>());
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, List<OfficeOnMap>>(false, new List<OfficeOnMap>());
        //    }

        //}

        //public static async Task<Tuple<bool, String>> CreateOffice(int TeamID, string name, double longtitude, double latitude)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(new Tuple<int, string, double, double>(TeamID, name, longtitude, latitude)), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PutAsync("api/Office/CreateOffice?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}

        //public static async Task<Tuple<bool, String>> ChangeGroupVisibility(int ID, bool isVisible, bool isOrganization)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(new Tuple<bool, bool, int>(isVisible, isOrganization, ID)), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Organization/ChangeVisibility?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}



        //public static async Task<Tuple<bool, String>> SetHome(double longtitude, double latitude)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(new Location { Longitude = longtitude, Latitude = latitude }), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PutAsync("api/Home/SetHome?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}

        //public static async Task<Tuple<bool, double, double>> LoadHome()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Home/LoadHome?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()));
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, double, double>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, double, double>(false, 0, 0);
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, double, double>(false, 0, 0);
        //    }
        //}

        //public static async Task<Tuple<bool, int>> SaveRoutePoint(double longtitude, double latitude, bool way, int PathID, WeekActuality Actuality)
        //{
        //    try
        //    {

        //        var content = new StringContent(JsonEngine.Serialize(new RP { Latitude = latitude, Longtitude = longtitude, IsToHome = way, Actuality = Actuality }), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PutAsync("api/Route/SaveRoutePoint?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, int>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, int>(false, 0);
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, int>(false, 0);
        //    }

        //}

        //public static async Task<Tuple<bool, List<Path>>> LoadUserRoutePoints()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Route/LoadRoutePoints?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()));
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, List<Path>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, List<Path>>(false, new List<Path>());
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, List<Path>>(false, new List<Path>());
        //    }


        //}

        //public static async Task<Tuple<bool, List<OnMapPoint>>> StartFindCompanions(double longtitude, double latitude, bool Way, DateTime date)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(new CompanionsRequest { Latitude = latitude, Longtitude = longtitude, IsToHome = Way, Date = date }), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Companions/FindingCompanions?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, List<OnMapPoint>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, List<OnMapPoint>>(false, new List<OnMapPoint>());
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, List<OnMapPoint>>(false, new List<OnMapPoint>());
        //    }


        //}

        //public static async Task<Tuple<bool, String>> NewComplaint(int SysCode, int UniqID)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(UniqID), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/OfficeComplaint/NewComplaint?SessionID=" + LocalStorage.SessionID + "&SysCode=" + SysCode, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}

        //public static async Task<Tuple<bool, String>> ChangeRoutePoint(int SysCode, int RouteID)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(RouteID), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Route/ChangeRoutePoint?SessionID=" + LocalStorage.SessionID + "&SysCode=" + SysCode + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }

        //}

        //public static async Task<Tuple<bool, List<string>>> GetCarBrands()
        //{

        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Car");
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, List<String>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, List<string>>(false, new List<string>());
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, List<string>>(false, new List<string>());
        //    }


        //}

        //public static async Task<Tuple<bool, List<String>>> GetCarModels(int BrandID)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/car/GetCarModels?CarBrandID=" + BrandID);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, List<String>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, List<string>>(false, new List<string>());
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, List<string>>(false, new List<string>());
        //    }

        //}

        //public static async Task<Tuple<bool, UserCompanion>> GetUserInfo(int CompanionID)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Companion/GetUserInfo?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&SysID=" + CompanionID);

        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, UserCompanion>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, UserCompanion>(false, new UserCompanion());
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, UserCompanion>(false, new UserCompanion());
        //    }



        //}

        //public static async Task<Tuple<bool, WeeklySchedule>> LoadSchedule()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Schedule/LoadSchedule?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()));

        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, WeeklySchedule>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, WeeklySchedule>(false, new WeeklySchedule());
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, WeeklySchedule>(false, new WeeklySchedule());
        //    }


        //}

        //public static async Task<Tuple<bool, String>> UpdateSchedule(WeeklySchedule UserSchedule)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(UserSchedule), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Schedule/UpdateSchedule?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }


        //}

        //public static async Task<Tuple<bool, String>> UpdatePosition(double longtitude, double latitude)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(new Tuple<double, double>(longtitude, latitude)), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PutAsync("api/Location/UpdateLocation?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }

        //}

        //public static async Task<Tuple<bool, Tuple<double, double>>> LoadPosition()
        //{

        //    try
        //    {

        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Location/LoadPreviousLocation?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()));
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, Tuple<double, double>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, Tuple<double, double>>(false, new Tuple<double, double>(0, 0));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, Tuple<double, double>>(false, new Tuple<double, double>(0, 0));
        //    }
        //}

        //public static async Task<Tuple<bool, string>> AutoJoinToOrganization(String UserEmail, String RSAPublic)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(RSAPublic), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Default/InternalOrganizationKey?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&email=" + UserEmail, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}

        //public static async Task<Tuple<bool, List<Conversation>>> LoadMessages()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Message/LoadMessages?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()));
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, List<Conversation>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, List<Conversation>>(false, new List<Conversation>());
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, List<Conversation>>(false, new List<Conversation>());
        //    }

        //}

        //public static async Task<Tuple<bool, String>> SendMessage(int toUID, String Text)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(Text), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Message/SendMessage?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&OpponentUID=" + toUID, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }

        //}


        //public static async Task<Tuple<bool, String>> SendOffer(int toUID, int RouteID, bool isToHome)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(new ShortOffer { ToUID = toUID, RouteID = RouteID, IsToHome = isToHome, AtTime = DateTime.Now }), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PutAsync("api/Trip/SendTripOffer?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()), content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }
        //}


        //public static async Task<Tuple<bool, String>> AcceptOffer(int OfferID)
        //{

        //    try
        //    {
        //        //var content = new StringContent(JsonEngine.Serialize(new ShortOffer { ToUID = toUID, RouteID = RouteID, IsToHome = isToHome, AtTime = DateTime.Now }), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/AcceptTrip/AcceptTrip?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&OfferID=" + OfferID);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }


        //}

        //public static async Task<Tuple<bool, String>> RejectOffer(int OfferID)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/RejectTrip/RejectOffer?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&OfferID=" + OfferID);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }
        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }


        //}

        //public static async Task<Tuple<bool, List<TripOffer>>> LoadOffers()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await PrepareRequest().GetAsync("api/Trip/LoadTrips?SessionID=" + LocalStorage.SessionID + "&Sign=" + Uri.EscapeDataString(General.CreateSign()) + "&CurrentUserTime=" + DateTime.Now);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, List<TripOffer>>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, List<TripOffer>>(false, new List<TripOffer>());
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return new Tuple<bool, List<TripOffer>>(false, new List<TripOffer>());
        //    }


        //}

        //public static async Task<Tuple<bool, String>> ChangePath(int SysCode, int PathID)
        //{
        //    try
        //    {
        //        var content = new StringContent(JsonEngine.Serialize(PathID), Encoding.UTF8, "application/json");

        //        HttpResponseMessage response = await PrepareRequest().PostAsync("api/Path/ChangePath?SessionID=" + LocalStorage.SessionID + "&SysCode=" + SysCode, content);
        //        string json = null;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            return JsonEngine.Deserialize<Tuple<bool, String>>(json);
        //        }
        //        else
        //        {
        //            return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //        }

        //    }
        //    catch
        //    {
        //        return new Tuple<bool, string>(false, ConvertMessages.Message("x50000"));
        //    }

        //}
    }
}