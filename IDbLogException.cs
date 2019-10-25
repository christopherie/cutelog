using System;
using System.Runtime.CompilerServices;

namespace CuteLog
{
    public interface IDbLogException
    {
        void Log(Exception exception,
                 string context,
                 [CallerMemberName] string memberName = "",
                 [CallerFilePath] string fileName = "",
                 [CallerLineNumber] int lineNumber = 0);
    }
}
