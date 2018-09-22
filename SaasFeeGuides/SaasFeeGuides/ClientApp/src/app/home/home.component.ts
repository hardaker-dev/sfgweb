import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../models/User';
import { Customer } from '../models/Customer';
import { CustomerService } from '../services/customer.service';

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent implements OnInit {
  currentAccount: User;
  customers: Customer[] = [];

  constructor(private customerService: CustomerService) {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.loadAllCustomers();
  } 

  private loadAllCustomers() {
    this.customerService.getMany().pipe(first()).subscribe(customers => {
      this.customers = customers;
    });
  }
}
