using System.Collections.Generic;

namespace ImageHuntWebServiceClient.Responses
{
    public class Result
    {
        public string Value { get; set; }
        public List<object> Formatters { get; set; }
        public List<object> ContentTypes { get; set; }
        public object DeclaredType { get; set; }
        public int StatusCode { get; set; }
    }

    public class LoginResponse
    {
        public Result Result { get; set; }
        public int Id { get; set; }
        public object Exception { get; set; }
        public int Status { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCompletedSuccessfully { get; set; }
        public int CreationOptions { get; set; }
        public object AsyncState { get; set; }
        public bool IsFaulted { get; set; }
    }
}