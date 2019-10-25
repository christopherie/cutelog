using System;
using System.Runtime.CompilerServices;

namespace CuteLog
{
    public interface IAlertMessage
    {
        void MailAlert(Exception exception,
                       string context,
                       [CallerMemberName] string memberName = "",
                       [CallerFilePath] string fileName = "",
                       [CallerLineNumber] int lineNumber = 0);

        void SlackAlert(Exception exception,
                        string context,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string fileName = "",
                        [CallerLineNumber] int lineNumber = 0);
    }
}
