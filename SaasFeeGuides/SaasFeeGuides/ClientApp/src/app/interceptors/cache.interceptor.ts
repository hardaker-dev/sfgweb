import { Injectable } from '@angular/core';
import { HttpEvent, HttpRequest, HttpResponse, HttpInterceptor, HttpHandler } from '@angular/common/http';

import { startWith, tap } from 'rxjs/operators';


import { CacheService } from '../services/cache.service'
import { Observable, of } from 'rxjs';
@Injectable()
export class CachingInterceptor implements HttpInterceptor {
  constructor(private cache: CacheService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const cachedResponse = this.cache.get(req);
    return cachedResponse ? of(cachedResponse) : this.sendRequest(req, next, this.cache);
  }

  sendRequest(
    req: HttpRequest<any>,
    next: HttpHandler,
    cache: CacheService): Observable<HttpEvent<any>> {
    if (req.method === "GET") {
      return next.handle(req).pipe(
        tap(event => {
          if (event instanceof HttpResponse) {
            cache.put(req, event);
          }
        })
      );
    }
    return next.handle(req);
  }
}
