import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserService } from './_services/user-edit.service';
import { UserListComponent } from './user-list/user-list.component';
import { LoginService } from './_services/login.service';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/authGuard';
import { JwtInterceptor } from './_guards/jwtInterceptor';
import { LocationsComponent } from './locations/locations.component';
import { LocationsService } from './_services/locations.service';
import { ProfileComponent } from './profile/profile.component';
import { ProfileService } from './_services/profile.service';
import { AccessListsComponent } from './access-lists/access-lists.component';
import { AccessListEditComponent } from './access-list-edit/access-list-edit.component';

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
      { path: 'user-list', component: UserListComponent, canActivate: [AuthGuard] },
      { path: 'user-edit/:id', component: UserEditComponent, canActivate: [AuthGuard] },
      { path: 'mylocations', component: LocationsComponent, canActivate: [AuthGuard] },
      { path: 'accessLists', component: AccessListsComponent, canActivate: [AuthGuard] },
      { path: 'accessListEdit/:id', component: AccessListEditComponent, canActivate: [AuthGuard] },
      { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
      { path: 'mylogin', component: LoginComponent },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },    
    UserService,
    LoginService,
    LocationsService,
    ProfileService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
