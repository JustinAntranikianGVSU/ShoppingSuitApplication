import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BootstrapTestComponent } from './bootstrap-test/bootstrap-test.component';
import { ObservablesTestComponent } from './observables-test/observables-test.component';
import { TodoService } from './services/todo.service';
import { MedicalInfoComponent } from './medical-info/medical-info.component';
import { MedicalInfoService } from './services/medical-info.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    BootstrapTestComponent,
    ObservablesTestComponent,
    MedicalInfoComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'bootstrap-test', component: BootstrapTestComponent },
      { path: 'observables-test', component: ObservablesTestComponent },
      { path: 'medical-info', component: MedicalInfoComponent },
    ])
  ],
  providers: [TodoService, MedicalInfoService],
  bootstrap: [AppComponent]
})
export class AppModule { }
