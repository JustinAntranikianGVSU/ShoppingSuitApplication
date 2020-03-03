import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ObservablesTestComponent } from './observables-test/observables-test.component';
import { TodoService } from './_services/todo.service';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserEditService } from './_services/user-edit.service';
import { UserListComponent } from './user-list/user-list.component';
import { LoginService } from './_services/login.service';
import { LogoutService } from './_services/logout.service';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/authGuard';
import { JwtInterceptor } from './_guards/jwtInterceptor';
import { LocationsComponent } from './locations/locations.component';
import { LocationsService } from './_services/locations.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ObservablesTestComponent,
    UserEditComponent,
    UserListComponent,
    LoginComponent,
    LocationsComponent
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
      { path: 'observables-test', component: ObservablesTestComponent, canActivate: [AuthGuard] },
      { path: 'locations', component: LocationsComponent, canActivate: [AuthGuard] },
      { path: 'mylogin', component: LoginComponent },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },    
    TodoService,
    UserEditService,
    LoginService,
    LogoutService,
    LocationsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
