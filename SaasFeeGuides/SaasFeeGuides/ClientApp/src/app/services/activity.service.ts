import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Activity } from '../viewModels/activity';
import { ActivityDate, ActivityDateModel } from '../viewModels/activityDate';
import { ActivitySkuDate } from '../models/activitySkuDate';

@Injectable()
export class ActivityService {
  constructor(private http: HttpClient) { }

  addDate(activitySkuDate: ActivitySkuDate) {
    return this.http.post<number>('api/activity/sku/date', activitySkuDate);
  }

  getMany() {
    return this.http.get<Activity[]>('api/activity/edit');
  }

  get(id: number) {
    return this.http.get<Activity>('api/Activity/' + id + '/edit');
  }

  deleteDate(activitySkuDateId: number) {
    return this.http.delete('api/Activity/Sku/date/' + activitySkuDateId);
  }


  getActivityDates(activityId: number, dateFrom: Date, dateTo: Date) {

    let params =
      new HttpParams()
        .append('dateFrom', dateFrom.toDateString())
        .append('dateTo', dateTo.toDateString());

    return this.http.get<ActivityDateModel[]>('api/activity/' + activityId + '/dates', { params: params });
  }
  getAllActivityDates( dateFrom: Date, dateTo: Date) {

    let params = new HttpParams();
    if (dateFrom) {
      params = params.append('dateFrom', dateFrom.toDateString() )
    }
    if (dateTo) {
      params = params.append('dateTo', dateTo.toDateString() )
    }

    return this.http.get<ActivityDateModel[]>('api/activity/dates', { params: params });
  }
}
