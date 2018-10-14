import { Component, OnInit, ChangeDetectorRef, ChangeDetectionStrategy, Input, ViewChild  } from '@angular/core';
import { first, debounceTime, distinctUntilChanged, map, filter } from 'rxjs/operators';

import { User } from '../viewModels/User';
import { Customer } from '../viewModels/Customer';
import { CustomerService } from '../services/customer.service';
import { CalendarComponent } from '../calendar/calendar.component';
import { ActivityService } from '../services/activity.service';
import { ActivityDate, ActivityDateModel } from '../viewModels/activityDate';
import { CalendarEvent } from 'calendar-utils';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Activity } from '../viewModels/activity';
import { Observable, merge, Subject } from 'rxjs';
import { NgbTypeahead } from '@ng-bootstrap/ng-bootstrap';
import { ActivitySku } from '../viewModels/activitySku';
import { ActivitySkuDate } from '../models/activitySkuDate';
import { CustomerBooking } from '../viewModels/customerBooking';


@Component({
  templateUrl: 'bookings.component.html',
  styleUrls: ['./bookings.component.css']
})
export class BookingsComponent implements OnInit {
  currentAccount: User;
  activityDates: ActivityDate[] = [];
  activities: Activity[] = [];
  activitySkus: ActivitySku[] = [];
  customers: Customer[] = [];
  thisObj = this;
  addBooking: (date: Date) => void;
  addDate: (date: Date) => void;
  submitted = false;
  addingBooking: boolean;
  addingDate: boolean;
  addBookingForm: FormGroup;
  addDateForm: FormGroup;
  selectedActivity: string;
  @ViewChild('instance') instance: NgbTypeahead;
  focus$ = new Subject<string>();
  click$ = new Subject<string>();

  private _searchModel: any;
  get searchModel(): any {
    return this._searchModel;
  }

  set searchModel(search: any) {
    this._searchModel = search;

    if (this._searchModel && this._searchModel.model) {
      this.formattedSelection = this._searchModel.formattedNameEmail();
    }
    else {
      this.formattedSelection = this._searchModel;
    }
  }

  constructor(
    private customerService: CustomerService,
    private activityService: ActivityService,
    private formBuilder: FormBuilder) {

    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
    var thisObj = this;
    this.addBooking = (date) => {
      thisObj.addingBooking = true;
      var hour = date.getHours();
      if (hour == 0) { date.setHours(7); }
      thisObj.addBookingForm.get('datetime').setValue(date.toISOString().slice(0, -1));
    };
    this.addDate = (date) => {
      thisObj.addingDate = true;
      var hour = date.getHours();
      if (hour == 0) { date.setHours(7); }
      thisObj.addDateForm.get('datetime').setValue(date.toISOString().slice(0, -1));
    };
  }

  onSubmit() {
    this.submitted = true;
    if (this.addingBooking) {
      
      if (this.addBookingForm.invalid) {
        return;
      }
      var activity = this.addBookingForm.get('activity').value as ActivitySku;
      var customer = this.addBookingForm.get('customer').value as Customer;
      var date = new Date(this.addBookingForm.get('datetime').value as string);
      var numPersons = this.addBookingForm.get('numPersons').value as number;
      this.customerService
        .addCustomerBooking(new CustomerBooking(activity.name, date, customer.model.email, numPersons))
        .pipe(first())
        .subscribe(
          id => {
            var activityDate = new ActivityDate({
              numPersons : numPersons,
              startDateTime : date,
              activityId : activity.activityId,
              activitySkuId : activity.id,
              activityName : activity.activityName,
              activitySkuName : activity.name,
              totalPrice : activity.pricePerPerson * numPersons,
              endDateTime : new Date(date.getTime() + (1000 * 60 * 60 * 24) * activity.durationDays + (1000 * 60 * 60) * activity.durationHours),
              amountPaid : 0,
              deleted : false,
              activitySkuDateId : 1
            });
            this.activityDates.push(activityDate);
            this.refreshActivities();      
            this.addingBooking = false;
            this.addBookingForm.clearValidators();
            this.addBookingForm.reset();
            this.submitted = false;
          },
          error => {
          });
    }
    else {
      if (this.addDateForm.invalid) {
        return;
      }
      var activity = this.addDateForm.get('activity').value as ActivitySku;
      var date = new Date(this.addDateForm.get('datetime').value as string);
      this.activityService.addDate(new ActivitySkuDate(activity.activityName, activity.name, date)).pipe(first())
        .subscribe(
          id => {
            var activityDate = new ActivityDate({
              numPersons: 0,
              startDateTime: date,
              activityId: activity.activityId,
              activitySkuId: activity.id,
              activityName: activity.activityName,
              activitySkuName: activity.name,
              totalPrice:0,
              endDateTime: new Date(date.getTime() + (1000 * 60 * 60 * 24) * activity.durationDays + (1000 * 60 * 60) * activity.durationHours),
              amountPaid: 0,
              deleted: false,
              activitySkuDateId: id
            });
            this.activityDates.push(activityDate);
            this.refreshActivities();
            this.addingDate = false;
            this.addDateForm.clearValidators();
            this.addDateForm.reset();
            this.submitted = false;
          },
          error => {
          });
    }
  }
  searchCustomer(event) {

    if (event) {
      if (event.key != 'Enter')
        return;
      event.preventDefault();
    }
    var search = this.addBookingForm.get('customer').value;
    if (search && search.model && search.model.firstName) {
      this.customerService.getMany(search.email).pipe(first()).subscribe(customers => {
        this.customers = customers.map((m) => new Customer(m));
        var element = document.getElementById('customer') as HTMLElement;
        element.click();
      });
    }
    else {
      this.customerService.getMany(search).pipe(first()).subscribe(customers => {
        this.customers = customers.map((m) => new Customer(m));
        var element = document.getElementById('customer') as HTMLElement;
        element.click();
      });
    }
  }
  cancelClick() {
    this.addingBooking = false;
    this.addingDate = false;
    this.addBookingForm.clearValidators();
    this.addBookingForm.reset();
    this.addDateForm.clearValidators();
    this.addDateForm.reset();
    this.submitted = false;
  }
  ngOnInit() {
    this.addBookingForm = this.formBuilder.group({
      activity: ['', Validators.required],
      customer: ['', Validators.required],
      numPersons: ['', Validators.required],
      datetime: ['', Validators.required]
    });
    this.addDateForm = this.formBuilder.group({
      activity: ['', Validators.required],     
      datetime: ['', Validators.required]
    });
    this.loadSchedule();
    this.loadActivities();
  }

  inputFormatter = (x: any) => {
    if (x.formattedNameEmail) {
      return x.formattedNameEmail();
    }
    return x;
  }
  formatter = (x: any) => {
    return x.formattedNameEmail();
  } 

  formattedSelection: string;
  search = (text$: Observable<string>) => {
    const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
    const clicksWithClosedPopup$ = this.click$.pipe(filter(() => !this.instance.isPopupOpen()));
    const inputFocus$ = this.focus$;

    var thisObj = this;
    return merge(debouncedText$, inputFocus$, clicksWithClosedPopup$)
      .pipe(
      map(term => (term === '' ? thisObj.customers
        : thisObj.customers.filter(c =>
          c.formattedNameEmail().toLowerCase().includes(term.toLowerCase()))).slice(0, 10)));
  }

  loadActivities() {
    var thisObj = this;
    this.activityService.getMany().pipe(first())
      .subscribe(
        activities => {
          thisObj.activities = activities;
          thisObj.activitySkus = activities.reduce((pn, u) => [...pn, ...u.skus], []);
        });
  }

  refreshActivities() {
    this.activityDates = this.activityDates.filter(function (d) { return !d.model.deleted; });
  }

  hookEvents(activityDate: ActivityDate):void {
    activityDate.onDeleted.subscribe(() => {
      if (activityDate.model.activitySkuDateId && activityDate.model.activitySkuDateId > 0) {
        this.activityService
          .deleteDate(activityDate.model.activitySkuDateId)
          .pipe(first())
          .subscribe(() => {
            activityDate.model.deleted = true;
            this.refreshActivities();
          });
      }
      else {
        activityDate.model.deleted = true;
        this.refreshActivities();
      }
    });
  }

  private loadSchedule() {
    this.activityService.getAllActivityDates(null, null)
      .pipe(first())
      .subscribe((dates) => {
        this.activityDates = this.activityDates.concat(dates.map((m) =>
          {
            var activityDate = new ActivityDate(m);
            this.hookEvents(activityDate);
            return activityDate;
          }));       
    });
  }

}
