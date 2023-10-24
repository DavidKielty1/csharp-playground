## install:

Angular
Node
Angular CLI
Dotnet CLI
Microsoft.EntityFrameworkCore.Designer - For Database
Microsoft.EntityFrameworkCore.Core - For Database
Microsoft.EntityFrameworkCore.Sqlite - For Database
Dotnet EF -For tool Database migrations and updates
mkcert, /client/ssl -> mkcert 'localhost' - For Making perm and key for SSL, HTTPS
System.IdentityModel.Tokens.Jwt - For Tokens - NuGet Package (Ctrl Shift P - Open NuGet Gallery)
microsoft.aspnetcore.authentication (Nuget Gallery)

mkcert -> angular.json. serve/options
boostrap -> angular.json. styles
bootswatch theme -> angular.json. styles
ngx-toastr -> angular.json. styles

## VSCode Extensions:

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
<< Services can be added to an extensions folder to keep program.cs tidy>>

client/angular.json -> "architect" / "serve": {"options": {"ssl": true,"sslCert": "./ssl/localhost.pem", "sslKey": "./ssl/localhost-key.pem" },}

package.json boostrap, ngx-boostrap. Dev: tslib.

Data/DbContext.
database(SQLite) -> program.cs + appsettings.Development.json: Connecting: DefaultString.
CORS: allow https:4200, link up with client serve.

Dotnet EF -> add migrations [name], database update. Create first database push.

_Auth: Token(Services, Interface), Controllers, Middleware, Entity_
Jwt Token Implementation
Added PasswordSalt and PasswordHash to AppUser Entity
Added folders: DTOs, Services(Token), Interfaces(IToken).
DTOs: LoginDto, RegisterDto, UserDto.
Added to controllers: Account and User.
appsettings.Development: Added Token Secret (Only used in Dev).
Account: Register and Login. JwtToken logic. Hashing, salts, secrets.
User: [Authorize] root, [AllowAnonymous] GetAll.
Services: AddIdentityServices.
Middleware: use app.UseAuthentication(); app.UseAuthorization();

_Setting up Angular navbar/homepage components. Conditional rendering_
Angular CLI ng g c/s; generate component/service.
Angular template forms.
. {#registerForm="ngForm", ngSubmit="register()",
. [(ngModel)]="model.username"/"model.password",
. (click)="register()"/"cancel()".}
Angular Services: acount.service.ts.
. Injection. Service - entire browsing lifecycle (singletons). Good place to store global state.
. {private currentUserSource = new BehaviorSubject<User | null>(null); behaviourSubject currentUser$} can get/set.
. Observable get - {currentUser$ = this.currentUserSource.asObservable();},
. next: set - {this.currentUserSource.next(user)};
Structural directives: Parent/child component communication. Input output properties.
. Within html template: {\*ngIf="registerMode" <app-register(cancelRegister)="cancelRegisterMode($event)"></app-register>}
. {@Input() component: function () => {};  template: [propname]="prop name".
. @Output() component: cancelRegister = new EventEmitter(); template: (cancelRegister)="cancelRegisterMode($event)".}
Post: {constructor(private http: HttpClient) {}, this.http.get('https://localhost:5001/api/users')
.subscribe({next: (response) => (this.users = response), error, complete})}.

_Angular Routing_
Components, not pages.
<a routerLink="/home" routerLinkActive="active">Link</a>
Route AuthGuard. Private, session based auth.
. app-routing.module: wrap one/all routes with {runGuardsAndResolvers: 'always', canActivate: [authGuard], children: [ {path}, {path}, {path}]}.
. html-template: <ngcontainer \*ngIf="accountService.currentUser$ | async"><li>Link</li><li>Link</li></ngcontainer>.

\*extras:

- Bootstrap theme, bootswatch
  Toast - error messages within login, register methods.
  sharedModule - holds app module imports outwith angular.

_Error handling_
API Middleware
Angular intercetors
Troubleshooting exceptions
