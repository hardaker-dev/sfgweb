import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { User } from '../viewModels/User';
import { Customer } from '../viewModels/Customer';
import { CustomerService } from '../services/customer.service';
import { AlertService } from '../services/alert.service';

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
    private alertService: AlertService)
  {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
 
  }
  onSubmit() {
    this.submitted = true;
    if (this.addCustomerForm.invalid) {
      return;
    }
    this.loading = true;
    var customer = new Customer(null, this.f.email.value, this.f.firstName.value, this.f.lastName.value, this.f.address.value, this.f.dob.value, this.f.phoneNumber.value);
    this.customerService
      .add(customer)
      .pipe(first())
      .subscribe(
        id => {
          customer.id = id;
          this.customers.push(customer);
          this.loading = false;
          this.addingCustomer = false;
          this.addCustomerForm.clearValidators();
          this.addCustomerForm.reset();
          this.submitted = false;
      },
      error => {
        this.alertService.error(error);
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
    this.customerService.getMany()
      .pipe(first())
      .subscribe(customers => {
      this.customers = customers;
    });
  }

  get f() { return this.addCustomerForm.controls; }
}
