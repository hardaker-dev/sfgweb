import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { User } from '../viewModels/User';
import { Customer } from '../viewModels/Customer';
import { CustomerService } from '../services/customer.service';
import { AlertService } from '../services/alert.service';
import { Router } from '@angular/router';

@Component({
  templateUrl: 'customers.component.html',
  styleUrls: ['./customers.component.css'] })
export class CustomersComponent implements OnInit {
  currentAccount: User;
  customers: Customer[] = [];
  addingCustomer:boolean;
  addCustomerForm: FormGroup;
  submitted = false;
  loading = false;
  constructor(
    private customerService: CustomerService,
    private formBuilder: FormBuilder,
    private router: Router,
    private alertService: AlertService)
  {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
 
  }
  filterClicked(customer:Customer) {

    this.router.navigate(['/bookings'], { queryParams: { customerEmail: customer.model.email } });
  }
  onSubmit() {
    this.submitted = true;
    if (this.addCustomerForm.invalid) {
      return;
    }
    this.loading = true;
    var customer = new Customer({
      id: null,
      email: this.f.email.value,
      firstName: this.f.firstName.value,
      lastName: this.f.lastName.value,
      address: this.f.address.value,
      dateOfBirth: this.f.dob.value,
      phoneNumber: this.f.phoneNumber.value
    });
    this.customerService
      .add(customer)
      .pipe(first())
      .subscribe(
      id => {
        customer.model.id = id;
          this.customers.push(customer);
          this.loading = false;
          this.addingCustomer = false;
          this.addCustomerForm.clearValidators();
          this.addCustomerForm.reset();
          this.submitted = false;
      },
      error => {
        this.loading = false;        
      });


  }
  ngOnInit() {
   
    this.addCustomerForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      dob: ['', Validators.required],
      address: ['', Validators.required]
    });
    this.addingCustomer = false;
    this.loading = false;
    this.submitted = false;
    this.loadAllCustomers();
  }
  cancelClick() {
    this.addingCustomer = false;
    this.addCustomerForm.reset();
  }
  addCustomerClick() {
    this.addingCustomer = true;
  }

  private loadAllCustomers() {
    this.customerService.getMany(null)
      .pipe(first())
      .subscribe(customers => {
        this.customers = customers.map((m) => new Customer(m));;
    });
  }

  get f() { return this.addCustomerForm.controls; }
}
