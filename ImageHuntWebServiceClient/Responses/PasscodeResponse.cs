﻿namespace ImageHuntWebServiceClient.Responses
{
    public class PasscodeResponse
    {
        public int Id { get; set; }
        public RedeemStatus RedeemStatus { get; set; }
        public string Pass { get; set; }
        public int Points { get; set; }
    }
}