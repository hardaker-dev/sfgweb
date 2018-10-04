import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../viewModels/User';
import { Customer } from '../viewModels/Customer';
import { CustomerService } from '../services/customer.service';

@Component({
  templateUrl: 'customers.component.html',
  styleUrls: ['./customers.component.css'] })
export class CustomersComponent implements OnInit {
  currentAccount: User;
  customers: Customer[] = [];

  constructor(private customerService: CustomerService) {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.loadAllCustomers();
  } 

  private loadAllCustomers() {
    this.customerService.getMany()
      .pipe(first())
      .subscribe(customers => {
      this.customers = customers;
    });
  }
}
