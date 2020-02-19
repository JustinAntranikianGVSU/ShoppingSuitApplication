import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ObservablesTestComponent } from './observables-test/observables-test.component';
import { TodoService } from './_services/todo.service';
import { MedicalInfoComponent } from './medical-info/medical-info.component';
import { MedicalInfoService } from './_services/medical-info.service';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserEditService } from './_services/user-edit.service';
import { UserListComponent } from './user-list/user-list.component';
import { LoginService } from './_services/login.service';
import { LogoutService } from './_services/logout.service';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ObservablesTestComponent,
    MedicalInfoComponent,
    UserEditComponent,
    UserListComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'mylogin', component: LoginComponent },
      { path: 'observables-test', component: ObservablesTestComponent },
      { path: 'observables-test', component: ObservablesTestComponent },
      { path: 'user-list', component: UserListComponent },
      { path: 'user-edit/:id', component: UserEditComponent },
      { path: 'medical-info', component: MedicalInfoComponent },
    ])
  ],
  providers: [TodoService, MedicalInfoService, UserEditService, LoginService, LogoutService],
  bootstrap: [AppComponent]
})
export class AppModule { }
