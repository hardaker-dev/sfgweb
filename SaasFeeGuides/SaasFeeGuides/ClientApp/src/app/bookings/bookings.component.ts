import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../viewModels/User';
import { Customer } from '../viewModels/Customer';
import { CustomerService } from '../services/customer.service';
import { CalendarComponent } from '../calendar/calendar.component';
import { ActivityService } from '../services/activity.service';
import { ActivityDate, ActivityDateModel } from '../viewModels/activityDate';
import { CalendarEvent } from 'calendar-utils';


@Component({ templateUrl: 'bookings.component.html' })
export class BookingsComponent implements OnInit {
  currentAccount: User;
  activityDates: ActivityDate[];

  constructor(private activityService: ActivityService) {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.loadSchedule();
  }

  private loadSchedule() {
    this.activityService.getAllActivityDates(null, null)
      .pipe(first())
      .subscribe((dates) => {
        this.activityDates = dates.map(function (m) { return new ActivityDate(m) });
       
    });
  }

}
