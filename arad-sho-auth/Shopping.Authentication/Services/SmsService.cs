using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SmsIrRestful;

namespace Shopping.Authentication.Services
{
    public class SmsService : IIdentityMessageService
    {

        public Task SendAsync(IdentityMessage message)
        {
            var token = new Token().GetToken("7512463e2c1552e68a34463e", "s@n@@rv@ndp@y@");
            var ultraFastSend = new UltraFastSend
            {
                Mobile = long.Parse(message.Destination),
                TemplateId = 7958,
                ParameterArray = new List<UltraFastParameters>()
                        {
                            new UltraFastParameters()
                            {
                                Parameter = "VerificationCode" ,
                                ParameterValue = message.Body
                            }
                        }.ToArray()
            };
            var ultraFastSendResponse = new UltraFast().Send(token, ultraFastSend);
            return Task.CompletedTask;
        }
    }
}