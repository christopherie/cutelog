using System.Net.Http;
using System.Runtime.CompilerServices;

namespace CuteLog
{
    public interface IDbLogHttpResponseMessage
    {
        void Log(HttpResponseMessage httpResponseMessage,
                 HttpRequestMessage httpRequestMessage,
                 string data,
                 string context,
                 [CallerMemberName] string memberName = "",
                 [CallerFilePath] string fileName = "",
                 [CallerLineNumber] int lineNumber = 0);
    }
}
