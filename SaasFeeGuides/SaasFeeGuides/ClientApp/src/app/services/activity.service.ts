import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Activity } from '../models/activity';

@Injectable()
export class ActivityService {
  constructor(private http: HttpClient) { }

  getMany() {
    return this.http.get<Activity[]>('api/activity/edit');
  }

  get(id: number) {
    return this.http.get<Activity>('api/Activity/' + id + '/edit');
  }



}
