import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule,HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';

import { AppComponent } from './app.component';
import { routing } from './app.routing';

import { AlertComponent } from './dailogs/alert/alert.component';
import { AuthGuard } from './guards/auth.guard';
import {  ErrorInterceptor } from './interceptors/error.interceptor';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { AlertService } from './services/alert.service';
import { AuthenticationService } from './services/authentication.service';
import { AccountService } from './services/account.service';
import { CustomerService } from './services/customer.service';
import { CustomersComponent } from './customers/customers.component';
import { ActivitiesComponent } from './activities/activities.component';
import { BookingsComponent } from './bookings/bookings.component';
import { AuthenticateComponent } from './authenticate/authenticate.component';
import { RegisterComponent } from './register/register.component';
import { ActivityService } from './services/activity.service';
import { SfgCalendarModule } from './calendar/calendar.module';

@NgModule({
  imports: [
    SfgCalendarModule,
    CommonModule,
    BrowserAnimationsModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    routing
  ],
  declarations: [
    AppComponent,
    AlertComponent,
    CustomersComponent,
    BookingsComponent,
    ActivitiesComponent,
    AuthenticateComponent,
    RegisterComponent
  ],
  providers: [
    AuthGuard,
    AlertService,
    AuthenticationService,
    AccountService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    CustomerService,
    ActivityService
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
