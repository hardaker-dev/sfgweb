import { Component, OnInit, ChangeDetectorRef, ChangeDetectionStrategy  } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../viewModels/User';
import { Customer } from '../viewModels/Customer';
import { CustomerService } from '../services/customer.service';
import { CalendarComponent } from '../calendar/calendar.component';
import { ActivityService } from '../services/activity.service';
import { ActivityDate, ActivityDateModel } from '../viewModels/activityDate';
import { CalendarEvent } from 'calendar-utils';


@Component({
  templateUrl: 'bookings.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush, })
export class BookingsComponent implements OnInit {
  currentAccount: User;

  activityDates: ActivityDate[];
  public getActiveActivityDates() {
    if (this.activityDates) {
      return this.activityDates.filter(function (d) { return !d.model.deleted; });
    }
    return null;
  }

  constructor(private activityService: ActivityService,private cd: ChangeDetectorRef) {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.loadSchedule();
  }

  private loadSchedule() {
    this.activityService.getAllActivityDates(null, null)
      .pipe(first())
      .subscribe((dates) => {
        this.activityDates = dates.map((m) =>
        {
          var activityDate = new ActivityDate(m);
          activityDate.onDeleted.subscribe(() =>
          {
            this.activityService
              .deleteDate(m.activitySkuDateId)
              .pipe(first())
              .subscribe(() => {
                m.deleted = true;
                this.cd.detectChanges();
              });           
          });
          return activityDate;
        });
       
    });
  }

}
