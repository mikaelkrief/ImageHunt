using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageHuntBotCore;
using ImageHuntWebServiceClient.Responses;

namespace ImageHuntValidator
{
    public class ImageHuntValidatorState : IState
    {
        public GameResponse Game { get; set; }
        public string CultureInfo { get; set; }
    }
}
