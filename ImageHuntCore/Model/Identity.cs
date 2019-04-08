using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ImageHuntCore.Model
{
    public class Identity : IdentityUser
    {
        public string TelegramUser { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppUserId { get; set; }

        public string Role { get; set; }
    }
}
