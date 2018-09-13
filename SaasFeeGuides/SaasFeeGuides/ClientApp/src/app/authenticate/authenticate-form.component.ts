import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoginInputs } from './LoginInputs'
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

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
    var x = this.http.post<LoginInputs>("/auth/login", this.model, httpOptions)
      .pipe(
        //catchError(this.handleError('addHero', hero))
      );
    this.submitted = true;


  }
}


