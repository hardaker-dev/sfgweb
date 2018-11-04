import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CustomerModel,Customer } from '../viewModels/customer';
import { HistoricCustomerBooking } from '../viewModels/historicCustomerBooking';
import { CustomerBooking } from '../viewModels/customerBooking';
import { AddCustomerBookingResponse } from '../viewModels/addCustomerBookingResponse';
import { CacheService } from './cache.service';

@Injectable()
export class CustomerService {
  constructor(private http: HttpClient,
    private cacheService: CacheService) { }

  getMany(searchText: string) {
    let params = new HttpParams();
    if (searchText) {
      params = params.append('searchText', searchText);
    }
    return this.http.get<CustomerModel[]>('api/customer', { params: params });
  }

  get(id: number) {
    return this.http.get<CustomerModel>('api/customer/' + id);
  }

  add(customer: Customer) {
    this.clearCustomerCache();
    return this.http.post<number>('api/customer', customer.model);
  }

  addHistoricCustomerBooking(booking: HistoricCustomerBooking) {
    return this.http.post('api/customer/booking/historic', booking);
  }
  addCustomerBooking(booking: CustomerBooking) {
    this.clearActivityCache();
    return this.http.post<AddCustomerBookingResponse>('api/customer/booking', booking);
  }
  updateCustomerBooking(booking: CustomerBooking) {
    this.clearActivityCache();
    return this.http.patch('api/customer/booking', booking);
  }

  getCustomerBookings(customerId: number) {
    return this.http.get('api/customer/' + customerId +'booking/');
  }

  clearActivityCache() {
    this.cacheService.clear('api/activity/dates');
  }
  clearCustomerCache() {
    this.cacheService.clear('api/customer');
  }
}
