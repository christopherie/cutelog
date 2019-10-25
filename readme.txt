# CuteLog

CuteLog is a simple but powerful error logging Nuget Package.

  - Log errors to database
  - Send alert messages by email
  - Send alert messages with Slack

# Features!

  - CuteLog allows you to log app errors to a database of your choice.
  - You can send an email containing error details to any email account, but why stop there? With CuteLog, you can send an alert email to your email account that is linked to Jira and an Incident on Jira will be automagically created for you.
  - You can send alert messages to your Slack account of choice and channel of choice using CuteLog's smart Slack integration.

### Getting Started

1. Install CuteLog's dependencies [CuteLog](https://www.nuget.org/packages/CuteLog/)
2. Install CuteLog Nuget Package
3. Configure your web.config (keep the keys as in this example)

```sh
    <connectionStrings>
      // your database connection string 
      <add name="CLogConnectionString" ....
    </connectionStrings>
  <appSettings>
    <add key="CLogSlackUrlWithAccessToken" value="https://hooks.slack.com/services/XXXXXX/XXXXXXXXXXXXX" />
    <add key="CLogSlackChannel" value="#yourchannel" />
    <add key="CLogSlackUser" value="you" />
    <add key="ClogSite" value="Your Site" />
    <add key="CLogCustomTimestamp" value="MM/dd/yyyy hh:mm tt" />
    <add key="CLogEmailTemplate" value="path to \Templates\ErrorAlertEmail.html" />
    <add key="CLogMailMessageHost" value="host" />
    <add key="CLogEmailAddress" value="email" />
    <add key="CLogMailMessagePort" value="port" />
  </appSettings>
```
4. Create the ErrorLog table in your database
```sh
SQL Statement to create error logging table in your database of choice.
CREATE TABLE ErrorLog
(
    [Id] int not null primary key identity(1,1),
	[ErrorTime] varchar(255),
	[AppEnvironment] varchar(255),
	[Context] varchar(255),
	[Site] varchar(255),
	[ExceptionType] varchar(255),
	[Message] varchar(255),
	[LineNumber] varchar(255),
	[MethodName] varchar(255),
	[FileName] varchar(255),
	[StackTrace] text
)
```

### Example use

```sh
using CuteLog;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;

namespace FacetedSearch
{
    public partial class Search : System.Web.UI.Page
    {
        public Database Database { get; set; } = new Database();
        private readonly IDbLogException dbLogException;
        private readonly IAlertMessage alertMessage;

        public Search()
        {
            dbLogException = DbLogManager.GetInstance;
            alertMessage = AlertMessageManager.GetInstance;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Database.SearchDemoConnectionString))
                    {
                        const string sql = "SELECT aosId, title FROM AreaOfStud";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        conn.Open();
                        DropDownList1.DataSource = cmd.ExecuteReader();
                        DropDownList1.DataBind();
                        DropDownList1.Items.Insert(0, new ListItem("Select Area Of Study", "Default"));
                    }
                }
                catch (Exception ex)
                {
                    dbLogException.Log(ex, "Select Area of Study");
                    alertMessage.SlackAlert(ex, "Select Area of Study");
                    alertMessage.MailAlert(ex, "Select Area of Study");
                }
            }
        }
    }
}
```
