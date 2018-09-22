import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Login } from '../models/login';
import { User } from '../models/user';
import * as moment from 'moment';

@Injectable()
export class AuthenticationService {
  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    return this.http.post<User>(`api/auth/login`, new Login(username, password))
      .pipe(map(user => {

       
        // login successful if there's a jwt token in the response
        if (user && user.authToken) {

          localStorage.setItem('currentUser', JSON.stringify(user));  
        }

        return user;
      }));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser'); 
  }
}
