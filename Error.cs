using System;

namespace CuteLog
{
    public class Error
    {
        public string Context { get; set; }
        public Exception ErrorMessage { get; set; }
        public string ErrorMessageString { get; set; }
        public string ErrorTime { get; set; }
        public string ExceptionType { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public string MemberName { get; set; }
        public string Site { get; set; }
    }
}
