using System;
using System.Net;

namespace Messenger.BLL
{
    public class MessageSendResult
    {
        public HttpStatusCode ResponseCode { get; set; }
        public Exception Exception { get; set; }

        public bool Sucsess => Exception == null && ResponseCode == HttpStatusCode.Created;

        public static MessageSendResult FromResponseStatusCode(HttpStatusCode responseCode)
        {
            return new MessageSendResult() { ResponseCode = responseCode };
        }
        public static MessageSendResult FromException(Exception exception)
        {
            return new MessageSendResult() { Exception = exception };
        }
        public static MessageSendResult BeforeSend()
        {
            return new MessageSendResult();
        }
    }
}
