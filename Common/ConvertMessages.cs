
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.ApplicationModel.Resources;
#endif
#if ANDROID
using Android.Content.Res;
using Android.Content;

#endif

namespace Shared
{
    class ConvertMessages
    {
        public static String Message(String Code)
        {
#if NETCOREAPP2_0
            return "";
#endif
#if NETFX_CORE
            if (Code == "Internal error") Code = "x50000";
            if (Code == "Server error") Code = "x50000";
            ResourceLoader loader = new ResourceLoader();
            return loader.GetString(Code);
#else
            if (Code == "Internal error") Code = "x50000";
#if XAMARIN || ANDROID
            switch (Code) {
                 case "startat":
                    return "start at: ";
                case "x10001":
                    return "email with key not sent";
                case "x10002":
                    return "Internal error, can not generate entries";
                case "x10003":
                    return "email already is used";
                case "x10005":
                    return "email is not valid";
                case "x10006":
                    return "can not restore access";
                case "x10007":
                    return "don`t use your organization`s mailbox";
                case "x11001":
                    return "UID already created, please try to restore access";
                case "x11002":
                    return "can not generate user session key";
                case "x11003":
                    return "answer is wrong, please try again";
                case "x11004":
                    return "can not find user password";
                case "x11006":
                    return "can not create record in DB";
                case "x11007":
                    return "can not create profile";
                case "x11008":
                    return "can not encypt new Key";
                case "x12001":
                    return "Sign of your messages is not correct";
                case "x12002":
                    return "Users profile contact information has been updated";
                case "x12003":
                    return "Users profile contact information has not been updated";
                case "x12004":
                    return "Inputed values are so long";
                case "x13001":
                    return "User role has been changed";
                case "x13002":
                    return "User role has not been changed";
                case "x14000":
                    return "Organization is already registered or in pending approval";
                case "x14001":
                    return "The key was sent to email";
                case "x14002":
                    return "can not register a new company";
                case "x14003":
                    return "Successfully registered. In pending appoval";
                case "x14004":
                    return "visibility has not been changed";
                case "x14005":
                    return "Organization is not registered";
                case "x14006":
                    return "you are already member of this organization";
                case "x14007":
                    return "can not generate password for you";
                case "x14008":
                    return "You have successfully joined";
                case "x14009":
                    return "Office with mentioned name is already created";
                case "x14010":
                    return "Successfully created";
                case "x14011":
                    return "you are not member of organization";
                case "x15000":
                    return "home has been set";
                case "x15001":
                    return "home has not been set";
                case "x16001":
                    return "route point saved";
                case "x16002":
                    return "route point not saved";
                case "x17000":
                    return "can not change route point state";
                case "x18005":
                    return "can not add new complaint";
                case "x19001":
                    return "Car has not been updated";
                case "x20001":
                    return "You have successfully leaved organization";
                case "x20002":
                    return "Due some errors your are not yet leave orgaztion";
                case "x23004":
                    return "can not generate session key";
                case "x23005":
                    return "can not check user sign";
                case "x24001":
                    return "Message was sent";
                case "x24002":
                    return "Message was not sente";
                case "x24003":
                    return "Message is too long";
                case "x25001":
                    return "Offer sent successfully";
                case "x25002":
                    return "Failed to sent offer";
                case "x26001":
                    return "Trip accepted";
                case "x26002":
                    return "Trip is not accepted";
                case "x27001":
                    return "Trip is rejected";
                case "x27002":
                    return "Trip is not rejected";
                case "x50000":
                    return "Server error";
                case "x50001":
                    return "can not send registration information to mentioned email";
                case "x50002":
                    return "can not confirm your password";
                case "x50003":
                    return "wrong password";
                case "x50004":
                    return "wrong password";
                case "x50005":
                    return "First name is requirede";
                case "x50006":
                    return "Last name is required";
                case "x50007":
                    return "Can not update your profile";
                case "x50008":
                    return "Wrong phone format. Please use only digit.";
                case "x50009":
                    return "can not save your profile";
                case "x50010":
                    return "Internal error. Check internet connection";
                case "x50011":
                    return "Firstly input prefered name of office";
                case "x51000":
                    return "";
                case "x51001":
                    return "Offer sent successfully";
                case "x51002":
                    return "Failed to sent offer";
                case "x51003":
                    return "Offer declined";
                case "x51004":
                    return "Offer changed";
                case "x51005":
                    return "Offer accepted";
                case "x51006":
                    return "";
                default:
                    return Code;
            }

#endif
#if WPF
#if NETCOREAPP2_0
            return "";
#else
            var resourceManager = CCS.Classic.Properties.Resources.ResourceManager;
            return resourceManager.GetString(Code);
#endif
#endif


#endif

        }
    }
}
