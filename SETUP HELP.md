## use:

Navigate to folders; Client/API.
Client: See README.md within client.
API: dotnet run.
Client: ng serve
Login+Token: Blanca - Pa$$w0rd, or see Data/seed information in API folder.

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
. mkcert -> angular.json. serve/options

System.IdentityModel.Tokens.Jwt - For Tokens - NuGet Package (Ctrl Shift P - Open NuGet Gallery)
microsoft.aspnetcore.authentication (Nuget Gallery)

boostrap (npm) + import
. boostrap -> angular.json. styles
. bootstrap, bootswatch
. bootswatch theme -> angular.json. styles
ngx-toastr (npm)
.ngx-toastr -> angular.json. styles
ngx-spinner
. Import/export from shared module. angular.json + specific spinner type
. added to app.html above navBar to be useable throughout app.
<ngx-spinner bdColor = "rgba(0, 0, 0, 0.8)" size = "medium" color = "#32fbe2" type = "ball-climbing-dot" [fullScreen] = "true"><p style="color: white" > Loading... </p></ngx-spinner>

Automapper

ng-gallery (npm)

## VSCode Extensions:

C#
C# Dev-kit
SQLite explorer
Angular

## Tidbits

JsonGenerator
Json to TypeScript

## Processes:

/
1-3. **Preliminary**
API.csproj -> <ImplicitUsings>enable</ImplicitUsings>
appsettings.development -> "Logging": {"LogLevel": {"Default": "Information","Microsoft.AspNetCore": "Information"}},
"ConnectionStrings": {"DefaultConnection": "Data source=geekmeet.db" }

Properties/launchSettings -> "launchBrowser": false, "applicationUrl": "http://localhost:5000;https://localhost:5001",

Program.cs ->'Dependency injection'
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>{opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));}); ((Moved to extensions folder)).
builder.Services.AddCors();
<< Services can be added to an extensions folder to keep program.cs tidy>>

client/angular.json -> "architect" / "serve": {"options": {"ssl": true,"sslCert": "./ssl/localhost.pem", "sslKey": "./ssl/localhost-key.pem" },}

package.json boostrap, ngx-boostrap. Dev: tslib.

Data/DbContext.
database(SQLite) -> program.cs + appsettings.Development.json: Connecting: DefaultString.
CORS: allow https:4200, link up with client serve.

Dotnet EF -> add migrations [name], database update. Create first database push.
/
**misc Quotes**
HttpClient returns observables, in order to transform or modify a observable we use pipe method from RxJS. We must subscribe to observables.
Javascript: NotFound(), NoContent() Reponse for a PutRequest 204.
/ 4. **Auth: Token(Services, Interface), Controllers, Middleware, Entity**
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
/ 5. **Setting up Angular navbar/homepage components. Conditional rendering**
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
/ 6. **Angular Routing**
Components, not pages.
{ <a* routerLink="/home" routerLinkActive="active">Link</a*> }
Route AuthGuard. Private, session based auth.
. app-routing.module: wrap one/all routes with {runGuardsAndResolvers: 'always', canActivate: [authGuard], children: [ {path}, {path}, {path}]}.
. html-template: {<ngcontainer* \*ngIf="accountService.currentUser$ | async"><li*>Link</li*><li*>Link</li*></ngcontainer*>.}

\*extras:

- Bootstrap theme, bootswatch
  Toast - error messages within login, register methods.
  sharedModule - holds app module imports outwith angular.
  /

7. **Error handling**
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
   /
8. **Extending the API**
   Extending Entities, Entity framework relationships / conventions:
   AppUser => Added user property fields & List<Photo> = new();
   Photos => Id, Url, IsMain, PublicId, {AppUserId, AppUser Appuser ( creating relation in Entity) } [Table("Photos)].
   Entity(Appuser, Photos) -> Dto(Member, MemberPhotos); DTOs (Only Select fields you want (no password/hash)).
   Controller(User) -> IUserRepository(UserRepository).
   Added DateTime extension to calculate age. Do not have logic in entity, messes with mapping/AutoMapping to DTOs.

_Repository pattern_
. Pros: minimizes duplicate query logic, decouples application for persistence framework (db), DB queries are centralized rather than scattered throughout app. Promotes testability, can easily mock the IRepository relative to DBContext.
. Cons: Abstraction of an abstraction. Each root entity should have it's own entity => more code. Also need to implement Unit of Work pattern to control transactions.
Creating a repository
. UserController injects UserRepository. UserRepository interacts with DB.

_Automapper_ - adding and usage -
Tool used to help map selected entity properties to EntityDto.
.Automapper config. Added to ApplicationServices(Extension).
Repository -> ProjectTo<MemberDto>
.AutoMapper queryable extensions. CreateMap<AppUser, MemberDto>().ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))

.Generating seed data - File.ReadAllTextAsync('Data/UserSeedData.json'). JsonSerializer.Deserialize. Middleware program.cs check if empty DB. Seed().
/ 9. **Building the UI**
apiUrl .environment/environment.development import => account.service (environment.apiUrl)
memberService fetches memberDTO info from API.
interceptor sends JWT tokens.

_members-list.component.ts/html_
Fetches members OnInit => subscribes to injected memberService. members = this.members (members array [] instantiated in component).
*ngFor="let member of members". Square breacket [member]="member" used to pass property from component.ts file to another component (member-list => member-card).
/
*member-card.component.ts/hmtl*
@Input() member: Member | undefined. Used to accept property from higher component (member-list).
Within button: routerLink="members/{{member.Username}}" Upon click -> route to memeber.Username (member-detail) (set up as memebers/username in app.router).
*ngIf="member" => no need for mass member?. throughout html. {member && . . . }.
/
_member-detail.component.ts/html_
Inject MemberService in constructor to fetch member details from memberAPI.
Inject ActivatedRoute to get username from route/url params: /member/:username.
. { this.memberService.getMember(this.route.snapshot.paramMap.get('username')); }. (this gets the username param from member/:username routeURL).
standalone: true. Must import member, tabset, gallery into the standalone component (imports array in component.ts).
. imports: [CommonModule (*ngIf), TabsModule (tabset), GalleryModule].
. new ImageItem within ngx-gallery { this.images.push(new ImageItem({ src: photos.url, thumb: photos.url })); }.
. #photoTab="Tab" trick to get first image to load in tab.

shared.module -> tabs ngx boostrap. Import. Must export TabsModule to use in html template.
app.module: MemberDetailComponent NgModule -> StandAlone component.
ng-gallery.

Angular component lifecycle (e.g. member-detail):

1. Component class is constructor (And injectables are injected).
2. template is constructed (no member info is loaded yet).
3. initialized (OnInit: loadMembers (members are loaded in)).
4. member details are now injected into the {{member?.property}} items./

**10 Updating Resources**
Implement persistence when updating resources in API.

1. Angular Template Forums: Angular form = { #editForm="ngForm". #[name] = ngForm. ngSubmit on form component. }
   . @ViewChild('editForm') editForm: NgForm | undefined;
   . member-edit.html = banana in box syntax for two-way binding [(ngModel)]="member".
2. CanDeactive Route Guard - if someone presses back, give prompt (are you sure you want to leave you will lose data).
   CanDeactivate(component, currentRoute, currentState, nextState) GuardRoute. Guards against clicking to go to another component within same app.
   @HostListener -> prompts if user navigates via browser (e.g. if they navigate via typing in URL).
   . { @HostListener('window:beforeunload', ['$event']) unloadNotification }
3. The @ViewChild decorator (access elements from templates in components).
   Html template is a child of component.ts Viewchild is used to access editForm within component (to reset form).
4. Persisting changes to API (update user). member-edit.ts/html.
   .pipe(map(() => { const index = this.members.indexOf(member);
   this.members[index] = { ...this.members[index], ...member }; }). Updating member info with spread operator. Latest state > prev.
   Use AutoMapper to map between MemberUpdateTo to AppUser (put updates).
5. Adding loading indicators. loadingInteptor, busyService, appModule provider, appComponent (top level).
   Interceptor implements HttpInterceptor. Shoots on any http request. As we have moved from API http request to stored data, no http request is taking place, thus we do not see the loading spinner anymore.
   \_interceptors/LoadingInterceptor
   \_busyService has logic for turning on spinner and setting busy to > 0 (++) ; and turning off spinner, setting spinner to 0 (--).
   \_interceptor will tell busyService if a http request has been made, thus kicking off ++.
   loadingInterceptor added to appModole providers array.
6. Caching data in Angular services.
   Storing members Get request info in membersService to persist data. rxjs .piping to persist data. e.g. Member-list component:
   Piping get and put requests within memberService => attaching to const 'member'.
   Within component.ts file, member$ = Observable<Member[]> | undefined;
   { ngOnInit(): void {this.members$ = this.memberService.getMembers();} }.
   html: { \*ngFor="let member of members$ | async" }

**11 Adding photo upload functionality**

1. Photo storage options.
   Databases are not optimized for large objects.
   Space. Do not want to run out of space. Web servers will crash from lack of disk space.
   File service on server. Space issue as above. Need logic for aspect ratio changes.
   Cloud service. No space issue. Could be costly. Logic built-in to transform aspect ratio. Square images.
   . Coudinary

   System:

   1. Client uploads photo to API server with JWT.
   2. Cloudinary will give us API keys which we will store on server. Server uploads photo to cloudinary with stored API key.
   3. Cloudinary stores photo and sends response (Image URL and ID).
   4. Server will save photoURL and publicID to DB.
   5. DB will auto generate an ID to send back to server.
   6. 201 created response. Server sends location and autogenerated ID in response header.

2. Adding rleated entities.
3. 3rd party API.
4. Debugger.
5. Updating and deleting resources in our API controller.
6. What to return when creating resources in a REST based API.

REST: Representational State Transfer. Software architectural style that sets constraints to be used with web services.
