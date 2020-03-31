import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserListComponent } from './user-list/user-list.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/authGuard';
import { JwtInterceptor } from './_guards/jwtInterceptor';
import { LocationsComponent } from './locations/locations.component';
import { ProfileComponent } from './profile/profile.component';
import { AccessListsComponent } from './access-lists/access-lists.component';
import { AccessListEditComponent } from './access-list-edit/access-list-edit.component';
import { RouteConstants } from './_shared/routeConstants';
import { ApiClientService } from './_services/api-client.service';
import { LocalStorageService } from './_services/local-storage.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    UserEditComponent,
    UserListComponent,
    LoginComponent,
    LocationsComponent,
    ProfileComponent,
    AccessListsComponent,
    AccessListEditComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: RouteConstants.UsersPage, component: UserListComponent, canActivate: [AuthGuard] },
      { path: `${RouteConstants.UserEditPage}/:id`, component: UserEditComponent, canActivate: [AuthGuard] },
      { path: RouteConstants.LocationsPage, component: LocationsComponent, canActivate: [AuthGuard] },
      { path: RouteConstants.AccessListsPage, component: AccessListsComponent, canActivate: [AuthGuard] },
      { path: `${RouteConstants.AccessListEditPage}/:id`, component: AccessListEditComponent, canActivate: [AuthGuard] },
      { path: RouteConstants.ProfilePage, component: ProfileComponent, canActivate: [AuthGuard] },
      { path: RouteConstants.LoginPage, component: LoginComponent },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    ApiClientService,
    LocalStorageService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
