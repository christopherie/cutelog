using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace CuteLog
{
    public sealed class DbLogManager : LogBase, IDbLogException, IDbLogHttpResponseMessage
    {
        private static readonly Lazy<DbLogManager> instance = new Lazy<DbLogManager>(() => new DbLogManager());

        private DbLogManager()
        {

        }

        public static DbLogManager GetInstance => instance.Value;

        public void Log(Exception exception, string context, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            lock (lockObj)
            {
                Error error = new Error
                {
                    Context = context,
                    ErrorMessage = exception,
                    Site = Site,
                    MemberName = memberName,
                    FileName = fileName,
                    LineNumber = lineNumber
                };

                using (SqlConnection sqlConnection = new SqlConnection(DbConnectionString))
                {
                    sqlConnection.Open();
                    const string insertStatement = "INSERT INTO ErrorLog (ErrorTime,Context,Site,ExceptionType,Message,LineNumber,MethodName,FileName,StackTrace) VALUES (@ErrorTime,@Context,@Site,@ExceptionType,@Message,@LineNumber,@MethodName,@FileName,@StackTrace)";
                    using (SqlCommand sqlCommand = new SqlCommand(insertStatement, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ErrorTime", DateTime.Now.ToString(CustomTimestamp));
                        sqlCommand.Parameters.AddWithValue("@Context", error.Context);
                        sqlCommand.Parameters.AddWithValue("@Site", error.Site);
                        sqlCommand.Parameters.AddWithValue("@Exceptiontype", error.ErrorMessage.GetType().Name);
                        sqlCommand.Parameters.AddWithValue("@Message", error.ErrorMessage.Message);
                        sqlCommand.Parameters.AddWithValue("@LineNumber", error.LineNumber.ToString());
                        sqlCommand.Parameters.AddWithValue("@MethodName", error.MemberName);
                        sqlCommand.Parameters.AddWithValue("@FileName", error.FileName);
                        sqlCommand.Parameters.AddWithValue("@StackTrace", error.ErrorMessage.StackTrace);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Log(HttpResponseMessage httpResponseMessage, HttpRequestMessage httpRequestMessage, string data, string context, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            lock (lockObj)
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage
                {
                    StatusCode = httpResponseMessage.StatusCode,
                    ReasonPhrase = httpResponseMessage.ReasonPhrase,
                    Content = httpResponseMessage.Content,
                    RequestMessage = httpResponseMessage.RequestMessage
                };

                using (SqlConnection sqlConnection = new SqlConnection(DbConnectionString))
                {
                    sqlConnection.Open();
                    const string insertStatement = "INSERT INTO ErrorLog (ErrorTime,Context,Site,ExceptionType,Message,LineNumber,MethodName,FileName,StackTrace) VALUES (@ErrorTime,@Context,@Site,@ExceptionType,@Message,@LineNumber,@MethodName,@FileName,@StackTrace)";
                    using (SqlCommand sqlCommand = new SqlCommand(insertStatement, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ErrorTime", DateTime.Now.ToString(CustomTimestamp));
                        sqlCommand.Parameters.AddWithValue("@Context", context);
                        sqlCommand.Parameters.AddWithValue("@Site", Site);
                        sqlCommand.Parameters.AddWithValue("@Exceptiontype", httpResponse.StatusCode.ToString());
                        sqlCommand.Parameters.AddWithValue("@Message", data);
                        sqlCommand.Parameters.AddWithValue("@LineNumber", lineNumber.ToString());
                        sqlCommand.Parameters.AddWithValue("@MethodName", memberName);
                        sqlCommand.Parameters.AddWithValue("@FileName", fileName);
                        sqlCommand.Parameters.AddWithValue("@StackTrace", httpResponse.RequestMessage.ToString());
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
