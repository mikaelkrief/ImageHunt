using System.ComponentModel.DataAnnotations;

namespace ImageHuntWebServiceClient.Request
{
    public class LoginRequest
    {
      [Required]
      public string UserName { get; set; }

      [Required]
      public string Password { get; set; }

  }
}
