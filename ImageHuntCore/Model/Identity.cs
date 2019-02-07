using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ImageHuntCore.Model
{
    public class Identity : IdentityUser
    {
        public string TelegramUser { get; set; }
    }
}
