import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Activity } from '../models/activity';
import { ActivityDate } from '../models/activityDate';

@Injectable()
export class ActivityService {
  constructor(private http: HttpClient) { }

  getMany() {
    return this.http.get<Activity[]>('api/activity/edit');
  }

  get(id: number) {
    return this.http.get<Activity>('api/Activity/' + id + '/edit');
  }

  getAllActivityDates(dateFrom: Date,dateTo:Date) {
    return this.http.get<ActivityDate[]>('api/Activity/dates');
  }
  getActivityDates(activityId: number, dateFrom: Date, dateTo: Date) {

    let params =
      new HttpParams()
        .append('dateFrom', dateFrom.toDateString())
        .append('dateTo', dateTo.toDateString());

    return this.http.get<ActivityDate[]>('api/Activity/' + activityId + '/dates', { params: params });
  }

}
