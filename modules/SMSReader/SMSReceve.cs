using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Authenticators;

namespace LiseuseSMS
{
    static class  ApiTwillio
    {
        public static Message[] CallApiTwilio()
        {
            string accountSid = "AC2680e5b46e3a453296f31ef94f61638b";
            string authToken = "3f4923a19144d71e01b713f487403e86";


            //Creation du client
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            client.Authenticator = new HttpBasicAuthenticator(accountSid, authToken);

            //request dans les ressources
            var request = new RestRequest("/Accounts/{AccountSid}/Messages.json", Method.GET);
            request.AddUrlSegment("AccountSid", accountSid);

            //REcuperation de la request en json
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var json = JsonConvert.DeserializeObject<Rootobject>(content).messages;


            return json;
        }
     }
}
