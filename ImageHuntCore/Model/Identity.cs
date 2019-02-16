using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ImageHuntCore.Model
{
    public class Identity : IdentityUser
    {
        public string TelegramUser { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppUserId { get; set; }

    }
}
