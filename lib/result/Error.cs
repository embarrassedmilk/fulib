using System;

namespace func {
    public class Error {
        public Error(string message)
        {
            Message = message;
            Exception = new Exception(message);
        }

        public Error(Exception exception) {
            Message = exception.Message;
            Exception = exception;
        }

        public string Message { get; }
        public Exception Exception { get; }
    }
}