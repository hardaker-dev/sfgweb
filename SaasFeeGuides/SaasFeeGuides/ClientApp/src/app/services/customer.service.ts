import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Customer } from '../models/customer';
import { HistoricCustomerBooking } from '../models/historicCustomerBooking';

@Injectable()
export class CustomerService {
  constructor(private http: HttpClient) { }

  getMany() {
    return this.http.get<Customer[]>('api/customer');
  }

  get(id: number) {
    return this.http.get<Customer>('api/customer/' + id);
  }

  add(customer: Customer) {
    return this.http.post('api/customer', customer);
  }

  addHistoricCustomerBooking(booking: HistoricCustomerBooking) {
    return this.http.post('api/customer/booking/historic', booking);
  }

}
