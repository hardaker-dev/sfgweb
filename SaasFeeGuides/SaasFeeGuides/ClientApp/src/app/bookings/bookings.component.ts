import { Component, OnInit, ChangeDetectorRef, ChangeDetectionStrategy  } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../viewModels/User';
import { Customer } from '../viewModels/Customer';
import { CustomerService } from '../services/customer.service';
import { CalendarComponent } from '../calendar/calendar.component';
import { ActivityService } from '../services/activity.service';
import { ActivityDate, ActivityDateModel } from '../viewModels/activityDate';
import { CalendarEvent } from 'calendar-utils';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Activity } from '../viewModels/activity';


@Component({
  templateUrl: 'bookings.component.html',
  styleUrls: ['./bookings.component.css'] })
export class BookingsComponent implements OnInit {
  currentAccount: User;
  activityDates: ActivityDate[] = [];
  activities: Activity[] = [];
  addActivityDate: (date: Date) => void;
  addingBooking: boolean;
  addBookingForm: FormGroup;
  selectedActivity: string;
  getActivities() {
    return this.activities;
  }
  constructor(
    private activityService: ActivityService,
    private formBuilder: FormBuilder) {

    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
    var thisObj = this;
    this.addActivityDate = (date) => {
      thisObj.addingBooking = true;
        //var newActivityDate = new ActivityDate({
        //  activitySkuDateId: null,
        //  activityId: null,
        //  activitySkuId: null,
        //  activityName: 'Allalin',
        //  activitySkuName: 'AllalinSku',
        //  startDateTime: date,
        //  endDateTime: new Date(date.getTime() + (1000 * 60 * 60 * 4)),
        //  numPersons: 1,
        //  amountPaid: 200,
        //  totalPrice: 200
        //});
        //thisObj.activityDates.push(newActivityDate);
        //thisObj.hookEvents(newActivityDate);
        //thisObj.refreshActivities();
      };
  }

  onSubmit() {
  }

  cancelClick() {
  }
  ngOnInit() {
    this.addBookingForm = this.formBuilder.group({
      activityInput: ['sadads', Validators.required],
      customer: ['asda', Validators.required],
      numPersons: ['asdasd', Validators.required]     
    });
    this.loadSchedule();
    this.loadActivities();
  }

  loadActivities() {
    var thisObj = this;
    this.activityService.getMany().pipe(first())
      .subscribe(
        activities => {
          thisObj.activities = activities;
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
