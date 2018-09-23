import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../models/User';
import { Activity } from '../models/Activity';
import { ActivityService } from '../services/activity.service';

@Component({
  templateUrl: 'activities.component.html',
  styleUrls: ['./activities.component.css'] })
export class ActivitiesComponent implements OnInit {
  currentAccount: User;
  activities: Activity[] = [];

  constructor(private activityService: ActivityService) {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.loadAllActivities();
  }

  private loadAllActivities() {
    this.activityService.getMany().pipe(first()).subscribe(activities => {
      this.activities = activities;
    });
  }
}
