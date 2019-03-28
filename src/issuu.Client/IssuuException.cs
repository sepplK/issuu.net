using System;

namespace issuu.Client
{

    public class IssuuException : Exception
    {

        public IssuuException(IssuuExceptionDetails error)
        {
            Error = error;
        }

        public IssuuExceptionDetails Error { get; }

        public override string Message => Error.ToString();

    }


    public class IssuuExceptionDetails
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Field { get; set; }

        public override string ToString()
        {
            return $"Error {Code}, {Message}, Field {Field}";
        }
    }

}
