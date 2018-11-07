import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CacheService } from './cache.service';
import { Guide, GuideModel } from '../viewModels/guide';

@Injectable()
export class GuideService {
  constructor(private http: HttpClient,
    private cacheService: CacheService) { }

  getMany(searchText: string) {
    let params = new HttpParams();
    if (searchText) {
      params = params.append('searchText', searchText);
    }
    return this.http.get<GuideModel[]>('api/guide', { params: params });
  }

  get(id: number) {
    return this.http.get<GuideModel>('api/guide/' + id);
  }

  add(guide: Guide) {
    this.clearGuideCache();
    return this.http.post<number>('api/guide', guide.model);
  }

  

  clearActivityCache() {
    this.cacheService.clear('api/activity/dates');
  }
  clearGuideCache() {
    this.cacheService.clear('api/guide');
  }
}
