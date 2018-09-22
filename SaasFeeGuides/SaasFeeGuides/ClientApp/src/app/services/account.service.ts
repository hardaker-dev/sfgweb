import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Account } from '../models/account';

@Injectable()
export class AccountService {
  constructor(private http: HttpClient) { }

  //getAll() {
  //  return this.http.get<User[]>(`users`);
  //}

  //getById(id: number) {
  //  return this.http.get(`api/` + id);
  //}

  register(account: Account) {
    return this.http.post('api/account', account);
  }

  update(account: Account) {
    return this.http.put('api/account', account);
  }

  delete() {
    return this.http.delete('api/account');
  }
}
