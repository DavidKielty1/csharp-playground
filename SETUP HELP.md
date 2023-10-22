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

mkcert -> angular.json
boostrap -> angular.json

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

_Seeting up Angular navbar/homepage components. Conditional rendering_
Angular CLI ng g c, ng g s. Generate component/service.
Angular template forms.
. #registerForm="ngForm", ngSubmit="register()",
. [(ngModel)]="model.username", [(ngModel)]="model.password",
. type="submit" (click)="register()", type="button" (click)="cancel()".
Angular Services: client/src/app/services/acount.service.ts.
. Dependency injection. Service lasts throughout entire browsing lifecycle. Component lasts as long as component is rendered on the screen.
. Service is injected into app. private currentUserSource = new BehaviorSubject<User | null>(null); behaviourSubject currentUser$ can be get and set.
. Observable get - currentUser$ = this.currentUserSource.asObservable();,
. next: set - this.currentUserSource.next(user);
Structural directives: Parent -> child; child -> parent component communications. Input output properties.
. Within html template: \*ngIf="registerMode" <app-register(cancelRegister)="cancelRegisterMode($event)"></app-register>
. @Input() component: function () => {};  template: [propname]="prop name".
. @Output() component: cancelRegister = new EventEmitter(); template: (cancelRegister)="cancelRegisterMode($event)".
Post: constructor(private http: HttpClient) {}, this.http.get('https://localhost:5001/api/users').
.subscribe({next: (response) => (this.users = response), error, complete}).

test
