using System;
using System.Collections.Generic;
using System.Text;

namespace ImageHuntWebServiceClient.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Telegram { get; set; }
        public string Role { get; set; }
        public GameResponse[] Games { get; set; }
        public int AppUserId { get; set; }
    }
}
