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
  addingBooking: boolean;
  addingDate: boolean;
  addBookingForm: FormGroup;
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
      thisObj.addBookingForm.get('datetime').setValue(date.toISOString().slice(0, -1));
    };
  }

  onSubmit() {
    var activity = this.addBookingForm.get('activity').value;
    var customer = this.addBookingForm.get('customer').value;
  }
  searchCustomer() {
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
  }
  ngOnInit() {
    this.addBookingForm = this.formBuilder.group({
      activity: ['', Validators.required],
      customer: ['', Validators.required],
      numPersons: ['', Validators.required],
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
