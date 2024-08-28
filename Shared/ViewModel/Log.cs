using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shared.Model;

namespace Shared.ViewModel
{
    class Log
    {
        public static async Task<bool> On()
        {
            try
            {
                var RSAEngine = new Shared.Crypting.RSA();
                var ClientRSAPair = Shared.Crypting.RSA.CreateKeyPair();
                var data = ClientRSAPair.Item2;

                var AESEngine = new Shared.Crypting.AES();
                var EncryptedRequest = AESEngine.Encrypt(data, LocalStorage.SessionKey);
                View.General.inLoading();
                var newSessionKey = await Shared.Model.Requests.Logon(EncryptedRequest);
                View.General.outLoading();
                if (newSessionKey.Item1 == true)
                {
                    var ServerResponse = RSAEngine.Decrypt(newSessionKey.Item2, ClientRSAPair.Item1);
                    //LocalStorage.sa(ServerResponse);
                    Model.LocalStorage.SessionKey = ServerResponse;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
