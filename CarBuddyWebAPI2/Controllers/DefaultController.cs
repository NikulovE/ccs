using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers
{
    public class DefaultController : ApiController
    {
        
        [HttpPost]
        public Tuple<bool, string> InternalOrganizationKey(int SessionID, string Sign, string email, [FromBody]string RSAPublic)
        {
            if (ProfileProcessing.CheckSign(SessionID, Sign))
            {
                var decryptedemail = "";
                var sessionid = 0;
                try
                {
                    var RSAEngine = new Shared.Crypting.RSA();
                    var decrypted = RSAEngine.Decrypt(email, "BwIAAACkAABSU0EyAAgAAAEAAQD5qOFWlF+s48mh+D7+nMKKVMtgd0jiSeum8ZUnDAReke5vdkG95JUwmCbVnfntXl4iR6R4PoyhT4BYT0f0CXZnNrEmjtNl2ZfRqQBoqdbwCfBwjG9pb5LvjSR2PaadDtuujEL5+jTOSmR01/RhsUj8EYPFXemLG5QNgHDbF4oDuuyq+jXVR00LCxqDHZ2xyJeNkCWIVWrafB8Y7+gZ+Ny4Di6wpA1/TRD//4se2RHuXtjF2SfFcdME+a4nfGAlpv5aLsRdHZ8c9juhfPSxtn7DxlqxCmvQmWqWmPVqL4euFivPmqAi1zt6vaxjE+7drTbcNYVzkMjtiZReAB4s+Oa2z9IWTpDiR59BHJSf4TFrJy26mhyy9ukbH/7n6GVZWi9ZOYpTOQNjix1eEFDKJMKck1nJfphA87vymY5avxPWw1eKw3xmTB5xqQ2h4Gemi2WdvcNBVPBh7n+MDUB44MaG8LMZrEmb5vnK0ccy9sI7IIRTtx8orqbxP3rnbt83ZLy3WQkUL7wozcEDoQY+UoeA1hqlw3GVqsr9aJMYi/XN/Kpq+GW06OkCI2HPz0sYtAxytfM/r0xQwtOdVFgdOfe7QUBmaWwSD5JPB4pzS5M+CmLXa58pbbuXIlXVTZ0KHvTkvoLT6ATanFtKCn7KllPWj6qDJVWL+crKpoq7LHKK+CGCqmrQrQTMqcHi0wOwg0cwVTrPs+vSG0nq1FnLmWJ3aB4u8D+Sbx/av2xM4l4SyaDOPoeUuEcl8XaA4LdJcZrFX1/PFuFF3O2MkP/XFnXk+abo3T8nFxDxWiqee+VeHtY9je0QHd2RdUYHr/lpWJMh/meDcCVh+w52/Qw1equKz2pI6MKWQZsseiA+eoXIJZjTOmYgVc9AHCCsPtJffrZ0MPh0Zp7fFlHIaTm4MdHwLo263qM3SWYa9eDicoQVfrHtBPyRhhU5WEWRjc84f2UG61LzcsoXqUVHIkO3Ric8j0tA85MIwuWFva7Zx1l4t5SP+3hXvvdI+l6PJ7sI23X7+1fOZf+IPq5tKCIkELEkwMB6paf8cnrd1CHtgs69s6ZVbHV3/I4C/dGPaPeiW5HKN5RtCPUZdGlTS08DsxtxCWwI7ECdoFwGZg9jjX5j0/qfne+MQpPdWVJpTLfXU+OCAFG7Lbvrw4hjwZs2dRIdtLC9bYbrxKKQfsolZpx5FH2bRlq/LRjRKClMezx6XnBHTp1EkaGcM+I7xu1QS7tFXIshe8EXBgN5KpxJ94fdTvgFmI4xp4bBlhS/BxJM1NHoVeKsYYv62UWIfPspub675y9M5LjCIbppJcQfMnqqcTtVYiFeLMv4hvI61V+Yk6x4iKDJKDwHJsL/Rp9wyfFz3tyV2c9f8KeqcxU2RA49xJ/qSHIfGKBh9MLOPQMnlJIrJHs4vwa12zcUTFkzEfmvrOkKPrj1WeTLCH109jptk8RAQ4blJnoW8PmzXWwj/hxgVXE9P+BtPupfeOaY9LFxDDe2CsVkkRxgfNZjcLKACB6j1ZwZqyhK83VkmgIDRAw=");
                    var val = decrypted.Split('|');
                    decryptedemail = val[0];
                    sessionid = int.Parse(val[1]);
                    if (sessionid != SessionID) return new Tuple<bool, String>(false, "x14007"); //can`t generate joiner flow
                }
                catch (Exception)
                {
                    return new Tuple<bool, String>(false, "x14007"); //can`t generate joiner flow
                }
                var Domain = decryptedemail.Split('@')[1];
                if (OrganizationProcessing.isAlreadyMember(Domain))
                {
                    return new Tuple<bool, String>(false, "x14006"); //already member
                }
                else
                {
                    var newJoiner = new OrganizationProcessing.Join { UserEmail = decryptedemail, Domain = Domain };
                    if (newJoiner.GenerateJoinerRegistrationEntry())
                    {
                        if (General.SendKeyToEmail(decryptedemail, newJoiner.Password))
                        {
                            var aesEngine = new Shared.Crypting.RSA();
                            var response = aesEngine.Encrypt(newJoiner.Password, RSAPublic);
                            return new Tuple<bool, String>(true, response); //email sent success
                        }
                        else
                        {
                            return new Tuple<bool, String>(false, "x10001"); //email sent not success
                        }
                    }
                    else
                    {
                        return new Tuple<bool, String>(false, "x14007"); //can`t generate joiner flow
                    }
                }

            }
            else
            {
                return new Tuple<bool, String>(false, "x12001"); //wrong sign
            }
        }
    }
}
