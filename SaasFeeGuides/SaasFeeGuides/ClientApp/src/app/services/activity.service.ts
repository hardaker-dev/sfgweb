import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Activity } from '../viewModels/activity';
import { ActivityDate, ActivityDateModel } from '../viewModels/activityDate';
import { NewActivitySkuDate } from '../models/newActivitySkuDate';
import { ActivitySkuDate } from '../models/activitySkuDate';
import { CacheService } from './cache.service';



@Injectable()
export class ActivityService {
  constructor(private http: HttpClient,
    private cacheService: CacheService) { }

  clearActivityCache() {
    this.cacheService.clear('api/activity/dates');
  }
  addDate(activitySkuDate: NewActivitySkuDate) {
    this.clearActivityCache();
    return this.http.post<number>('api/activity/sku/date', activitySkuDate);
  }

  getMany() {
    return this.http.get<Activity[]>('api/activity/edit');
  }

  get(id: number) {
    return this.http.get<Activity>('api/Activity/' + id + '/edit');
  }

  deleteDate(activitySkuDateId: number) {
    this.clearActivityCache();
    return this.http.delete('api/Activity/Sku/date/' + activitySkuDateId);
  }

  updateDate(activitySkuDate: ActivitySkuDate) {
    this.clearActivityCache();
    return this.http.patch('api/Activity/Sku/date', activitySkuDate);
  }

  deleteCustomerBooking(activitySkuDateId: number, customerEmail: string) {
    this.clearActivityCache();
    return this.http.delete('api/Activity/Sku/date/' + activitySkuDateId + '/customer/' + customerEmail);
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
