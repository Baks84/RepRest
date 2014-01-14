using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTClient.SugarSync
{
    public class SugarSyncApiMy
    {
        public void Test()
        {
            SugarSyncApi.SugarSyncClient cl = new SugarSyncApi.SugarSyncClient();
            //string tt = cl.GetRefreshToken("kkromkowski@ftechno.pl", "Aniolek69", "/sc/5818343/500_169111437", "NTgxODM0MzEzODY4ODQyODMxODc", "YzBjMjcyMmVlMzZkNDAzODgyNDk2N2EzMWU4OTFlNGY");
            //string a =  cl.GetAccessToken("NTgxODM0MzEzODY4ODQyODMxODc", "YzBjMjcyMmVlMzZkNDAzODgyNDk2N2EzMWU4OTFlNGY", 
            //"https://api.sugarsync.com/app-authorization/A353831383334332f3530305f313731343336383534");
            //var a = cl.GetUserInfo("https://api.sugarsync.com/authorization/KrLveL71f1IG6f9zflsTEOWglPclp0addvW3WWRO9FjaERF2cLEGMQu7XtOOUP0O5KAY5uVrEpaYJJp1cyHLsEvz9FStGMAWU0NtUKPJdZTXB5on8ykV7FgM33SImnckTNVy_m0KH08EqrBmyPh2qE_BY6Xkyv6yzTaazMaWER8.");
            var a=cl.GetContainerInfo("https://api.sugarsync.com/authorization/KrLveL71f1IG6f9zflsTEOWglPclp0addvW3WWRO9FjaERF2cLEGMQu7XtOOUP0O5KAY5uVrEpaYJJp1cyHLsEvz9FStGMAWU0NtUKPJdZTXB5on8ykV7FgM33SImnckTNVy_m0KH08EqrBmyPh2qE_BY6Xkyv6yzTaazMaWER8.",
                "https://api.sugarsync.com/user/5818343/workspaces/contents");
        }
    }
}
