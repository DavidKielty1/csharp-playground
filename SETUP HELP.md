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
bootstrap, bootswatch
ngx-toastr
Automapper

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

_misc Quotes_
HttpClient returns observables, in order to transform or modify a observable we use pipe method from RxJS.

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
API: Added Controllers/BuggyController, Errors/APIException.cs, Middleware/ExceptionMiddleware, added ExceptionMiddleware to program.cs
Client: \_interceptors/error.interceptor.ts
. HttpErrorResponse observable piped with case/switch statements for all error codes. Error, error.error, error.error.errors.
Errors/HTML templates: errors/not-found, errors/server-error, errors/test-error(for testing).
. test-error.component.ts - logic for various errors (400, 401, 404, 500, 400 Validation error(register/login)).
. 400 bad request toastr
. 401 unauthorized toastr
. 404 not-found - redirect 404 not found page.
. 500 server-error - redirect to server-error page; error.details, error.message, guide to server errors.
Troubleshooting - Network tab for errors.

_Extending the API_
Extending Entities, Entity framework relationships / conventions:
AppUser => Added user property fields & List<Photo> = new();
Photos => Id, Url, IsMain, PublicId, {AppUserId, AppUser Appuser ( creating relation in Entity) } [Table("Photos)].
Entity(Appuser, Photos) -> Dto(Member, MemberPhotos); DTOs (Only Select fields you want (no password/hash)).
Controller(User) -> IUserRepository(UserRepository).
Added DateTime extension to calculate age. Do not have logic in entity, messes with mapping/AutoMapping to DTOs.

.Repository pattern -
. Pros: minimizes duplicate query logic, decouples application for persistence framework (db), DB queries are centralized rather than scattered throughout app. Promotes testability, can easily mock the IRepository relative to DBContext.
. Cons: Abstraction of an abstraction. Each root entity should have it's own entity => more code. Also need to implement Unit of Work pattern to control transactions.
Creating a repository
. UserController injects UserRepository. UserRepository interacts with DB.

.Automapper - adding and usage - tool to help map selected entity properties to EntityDto.
.Automapper config. Added to ApplicationServices(Extension).
Repository -> ProjectTo<MemberDto>
.AutoMapper queryable extensions. CreateMap<AppUser, MemberDto>().ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))

.Generating seed data - File.ReadAllTextAsync('Data/UserSeedData.json'). JsonSerializer.Deserialize. Middleware program.cs check if empty DB. Seed().
