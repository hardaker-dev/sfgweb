import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoginInputs } from './LoginInputs'
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { Observable } from "rxjs/Observable";
import * as moment from 'moment';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

function setSession(authResult) {
  const expiresAt = moment().add(authResult.expires_in, 'second');

  localStorage.setItem('id_token', authResult.auth_token);
  localStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()));
}  

@Component({
  selector: 'authenticate-form',
  templateUrl: './authenticate-form.component.html'
})
@Injectable()
export class AuthenticateFormComponent {
  constructor(private http: HttpClient) { }

  submitted = false;
  UserNameInput = null;
  PasswordInput = null;
  model = new LoginInputs('',''); 

      

  onSubmit() {
   
     this.http.post<LoginInputs>("api/auth/login", this.model, httpOptions)    
      .catch((error: any) => Observable.throw(error.json().error || 'Server error')) //...errors if
       .subscribe({
         next(result) {
           setSession(result);
         },
         error(msg) { console.log('Error logging in: ', msg); }
       });
    this.submitted = true;


  }
}


