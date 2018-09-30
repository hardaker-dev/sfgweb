import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../models/User';
import { Customer } from '../models/Customer';
import { CustomerService } from '../services/customer.service';
import { CalendarComponent } from '../calendar/calendar.component';

@Component({ templateUrl: 'bookings.component.html' })
export class BookingsComponent implements OnInit {
  currentAccount: User;


  constructor(private customerService: CustomerService) {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
  } 

}
