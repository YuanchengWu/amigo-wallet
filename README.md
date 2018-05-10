# AmigoWallet

##### _An ASP.NET Core web app with MVC and Microsoft SQL Server_

## Requirements

For deployment, you will need [.NET Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=30653) and [Microsoft Internet Information Services (IIS)](https://www.microsoft.com/en-us/download/details.aspx?id=48264) enabled on Windows 7 or higher, or Windows Server 2012 or higher.

For development, as well as deploying locally, you will need [.NET Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=30653) and [Microsoft Visual Studio](https://www.visualstudio.com/vs/features/net-development/).

In addition, [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017) is necessary to run the database. Although you may replace it with other options.

### .NET Framework

You must have Windows 7 SP1 or higher to run .NET Framework 4.5. If you have Windows 8 or above, the framework is already installed. To manually install it on Windows 7 SP1, download the installer [here](https://www.microsoft.com/en-us/download/details.aspx?id=30653) and it will guide you through the process.

### Microsoft Visual Studio

Simply download and install from the official [website](https://www.visualstudio.com/downloads/).

---

## Install

Create a SQL server and run `AmigoWalletDAL/AmigoSQL.sql` to create the data schema.

Open the project with Visual Studio by importing the solution file: `AmigoWallet.sln`.

Modify `AmigoWalletDAL/App.Config` to include your data source string under the connectionStrings tag (replace `<source directory>` and `<server name>`).

```xml
<connectionStrings>
  <add name="AmigoWalletDBContext" connectionString="metadata=res://*/AmigoWalletModel.csdl|res://*/AmigoWalletModel.ssdl|res://*/AmigoWalletModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source= <source directory>;initial catalog= <server name>;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
```

## Deploying on IIS with Visual Studio

MSDN has detailed [instructions](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/deployment/visual-studio-web-deployment/deploying-to-iis) that covers this topic extensively.

---

## Languages & tools

### HTML

- [Razor syntax (CSHTML)](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-2.1) for embedding server-based code.

### C# #

- [ASP.NET](https://www.asp.net/) for all front-end and back-end logic.
    - [MVC](https://www.asp.net/mvc) for web framework.

### JavaScript

- [Bootstrap 3](https://getbootstrap.com/) is used for UI.

### Database

- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017) for user/admin account, transaction creation and management.
