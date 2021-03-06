﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImageHuntWebServiceClient.Request;
using ImageHuntWebServiceClient.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImageHuntWebServiceClient.WebServices
{
    public class PasscodeWebService : AbstractWebService, IPasscodeWebService
    {
        private readonly ILogger<PasscodeWebService> _logger;

        public PasscodeWebService(HttpClient httpClient, ILogger<PasscodeWebService> logger) : base(httpClient, logger)
        {
            _logger = logger;
        }

        public async Task<PasscodeResponse> RedeemPasscode(int gameId, string userName, string passcode)
        {
            var passcodeRequest = new PasscodeRedeemRequest()
            {
                GameId = gameId,
                UserName = userName,
                Pass = passcode
            };
            using (var content = new StringContent(JsonConvert.SerializeObject(passcodeRequest), Encoding.UTF8,
                "application/json"))
            {
                try
                {
                    var result = await PatchAsync<PasscodeResponse>($"{HttpClient.BaseAddress}api/Passcode", content);

                    return result;

                }
                catch (KeyNotFoundException e)
                {
                    _logger.LogError(e, "In gameId={0} the player {1} was not found", gameId, userName);
                    return new PasscodeResponse(){RedeemStatus = RedeemStatus.UserNotFound};
                }
            }
        }


    }
}