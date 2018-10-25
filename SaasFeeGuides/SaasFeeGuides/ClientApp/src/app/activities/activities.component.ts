import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../viewModels/User';
import { Activity } from '../viewModels/Activity';
import { ActivityService } from '../services/activity.service';
import { Router } from '@angular/router';

@Component({
  templateUrl: 'activities.component.html',
  styleUrls: ['./activities.component.css'] })
export class ActivitiesComponent implements OnInit {
  currentAccount: User;
  activities: Activity[] = [];

  constructor(private activityService: ActivityService,
    private router: Router) {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
  }


  filterClicked(activity: Activity) {

    this.router.navigate(['/bookings'], { queryParams: { activityName: activity.name } });
  }

  ngOnInit() {
    this.loadAllActivities();
  }

  private loadAllActivities() {

   

    this.activityService.getMany().pipe(first())
      .subscribe(
      activities => {
        this.activities = activities;
      });
  }
}
