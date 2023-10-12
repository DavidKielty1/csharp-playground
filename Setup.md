## install:

Angular
Node
Angular CLI
Dotnet CLI
Microsoft.EntityFrameworkCore.Designer
Microsoft.EntityFrameworkCore.Core
Microsoft.EntityFrameworkCore.Sqlite
Dotnet EF tool
mkcert, /client/ssl -> mkcert 'localhost'

mkcert -> angular.json
boostrap -> angular.json

## Extensions:

C#
C# Dev-kit
SQLite explorer
Angular

## Processes:

_Priliminary_
API.csproj -> <ImplicitUsings>enable</ImplicitUsings>
appsettings.development -> "Logging": {"LogLevel": {"Default": "Information","Microsoft.AspNetCore": "Information"}},
"ConnectionStrings": {"DefaultConnection": "Data source=geekmeet.db" }

Properties/launchSettings -> "launchBrowser": false, "applicationUrl": "http://localhost:5000;https://localhost:5001",

Program.cs ->'Dependency injection'
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>{opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));});
builder.Services.AddCors();

client/angular.json -> "architect" / "serve": {"options": {"ssl": true,"sslCert": "./ssl/localhost.pem", "sslKey": "./ssl/localhost-key.pem" },}

package.json boostrap, ngx-boostrap. Dev: tslib

Data/DbContext
database(SQLite) -> program.cs + appsettings.Development.json: Connecting: DefaultString
CORS: allow https:4200, link up with client serve

Dotnet EF -> add migrations [name], database update. Create first database push.

_Auth, controllers_
