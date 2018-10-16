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
  viewEditActivityDate: ActivityDate;
  activityDates: ActivityDate[] = [];
  activities: Activity[] = [];
  activitySkus: ActivitySku[] = [];
  customers: Customer[] = [];
  thisObj = this;
  appendBooking: (date: ActivityDate) => void;
 
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
    this.appendBooking = (activityDate: ActivityDate) => {
      thisObj.viewEditActivityDate = activityDate;
      thisObj.addingBooking = true;
      var dateTimeField = thisObj.addBookingForm.get('datetime');
      var activitySkuField = thisObj.addBookingForm.get('activity');

      activitySkuField.setValue(thisObj.activitySkus.find((sku) => sku.id == activityDate.model.activitySkuId));
      activitySkuField.disable();
      dateTimeField.setValue(activityDate.model.startDateTime);
      dateTimeField.disable();
    };
    
    this.addBooking = (date) => {
      thisObj.addingBooking = true;
      var hour = date.getHours();
      if (hour == 0) { date.setHours(7); }
      this.enableField('activity', thisObj.addBookingForm);
      this.enableField('datetime', thisObj.addBookingForm);
      
      thisObj.addBookingForm.get('datetime').setValue(this.getLocalISOTime(date));
     };
    this.addDate = (date) => {
      thisObj.addingDate = true;
      var hour = date.getHours();
      if (hour == 0) { date.setHours(7); }
      this.enableField('activity', thisObj.addBookingForm);
      this.enableField('datetime', thisObj.addBookingForm);
      thisObj.addDateForm.get('datetime').setValue(this.getLocalISOTime(date));
    };
  }
  enableField(name, form: FormGroup) {
    form.get(name).enable();
  }

  disableField(name, form: FormGroup) {
    form.get(name).disable();
  }
  getLocalISOTime(date) {
    var tzoffset = date.getTimezoneOffset() * 60000; //offset in milliseconds
    var localISOTime = (new Date(date.getTime() - tzoffset)).toISOString().slice(0, -1);
    return localISOTime;
  }
  deleteBooking(date: ActivityDate) {
    date.delete();
  }
  onSubmit() {
    this.submitted = true;
    if (this.addingBooking) {
      
      if (this.addBookingForm.invalid) {
        return;
      }
      var activitySku = this.addBookingForm.get('activity').value as ActivitySku;
 
    
      var customer = this.addBookingForm.get('customer').value as Customer;
      var date = new Date(this.addBookingForm.get('datetime').value as string);
      var numPersons = +this.addBookingForm.get('numPersons').value as number;
      this.customerService
        .addCustomerBooking(new CustomerBooking(activitySku.name, date, customer.model.email, numPersons))
        .pipe(first())
        .subscribe(
          response => {
            if (!this.viewEditActivityDate) {
              var activityDate = new ActivityDate({
                numPersons: numPersons,
                startDateTime: date,
                activityId: activitySku.activityId,
                activitySkuId: activitySku.id,
                activityName: activitySku.activityName,
                activitySkuName: activitySku.name,
                totalPrice: activitySku.pricePerPerson * numPersons,
                endDateTime: new Date(date.getTime() + (1000 * 60 * 60 * 24) * activitySku.durationDays + (1000 * 60 * 60) * activitySku.durationHours),
                amountPaid: 0,
                deleted: false,
                activitySkuDateId: response.activitySkuDateId
              });

              this.hookEvents(activityDate);
              this.activityDates.push(activityDate);
            }
            else {
              this.viewEditActivityDate.model.numPersons += numPersons;
              this.viewEditActivityDate.model.totalPrice = activitySku.pricePerPerson * this.viewEditActivityDate.model.numPersons;
              this.viewEditActivityDate = null;
            }
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
            this.hookEvents(activityDate);
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
  bookingClicked(event) {

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
