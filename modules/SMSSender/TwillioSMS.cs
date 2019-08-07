using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace LiseuseSMS
{
    public class TwillioSMS
    {
        private string accountSid = "AC2680e5b46e3a453296f31ef94f61638b";
        private string authToken = "3f4923a19144d71e01b713f487403e86";
        private string numFrom = "+33644602617"; // num from 
        public string numTo { get; set; } = "+33673090461"; // num to
        private Sqlite dB;

        public TwillioSMS()
        {
            TwilioClient.Init(accountSid, authToken);
            dB = new Sqlite();
        }

        public string sendSMS(string Message) {
            var message = MessageResource.Create(
                body: Message,
                from: new Twilio.Types.PhoneNumber(numFrom),
                to: new Twilio.Types.PhoneNumber(numTo)
                );
            


           return message.Sid; 
         }

    }

}
