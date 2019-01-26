using System;

namespace ImageHuntWebServiceClient.Request
{
    [Flags]
    public enum IncludeAction
    {
        All,
        Picture,
        Positions,
        ReplyQuestion,
        HiddenNode,
    }
}