import { Component, OnInit,  ViewChild  } from '@angular/core';
import { first, debounceTime, distinctUntilChanged, map, filter } from 'rxjs/operators';

import { User } from '../viewModels/User';
import { Customer } from '../viewModels/Customer';
import { CustomerService } from '../services/customer.service';
import { ActivityService } from '../services/activity.service';
import { ActivityDate } from '../viewModels/activityDate';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Activity } from '../viewModels/activity';
import { Observable, merge, Subject } from 'rxjs';
import { NgbTypeahead } from '@ng-bootstrap/ng-bootstrap';
import { ActivitySku } from '../viewModels/activitySku';
import { ActivitySkuDate } from '../models/activitySkuDate';
import { CustomerBooking } from '../viewModels/customerBooking';
import { NewActivitySkuDate } from '../models/newActivitySkuDate';
import { ActivatedRoute } from '@angular/router';
import { ActivitySkuPrice } from '../viewModels/activitySkuPrice';


@Component({
  templateUrl: 'bookings.component.html',
  styleUrls: ['./bookings.component.css']
})
export class BookingsComponent implements OnInit {
  currentAccount: User;
  viewEditActivityDate: ActivityDate;
  activityDates: ActivityDate[] = [];
  activities: Activity[] = [];
  activitySkus: ActivitySku[] = [];
  customers: Customer[] = [];
  thisObj = this;
  viewEditBooking: (date: ActivityDate) => void;
  appendBooking: (date: ActivityDate) => void;
  deleteBooking: (date: CustomerBooking) => void;
  refresh: Subject<any> = new Subject();
  addBooking: (date: Date) => void;
  addDate: (date: Date) => void;
  submitted = false;
  addingBooking: boolean;
  addingDate: boolean;
  addBookingForm: FormGroup;
  addDateForm: FormGroup;
  showCancel= true;
  selectedActivity: string;
  customerBookingExists = false;
  @ViewChild('instance') instance: NgbTypeahead;
  focus$ = new Subject<string>();
  click$ = new Subject<string>();
  filterCustomerEmail: string;
  filterActivityName: string;
  personsOption: number[];
  private _searchModel: any;
  get searchModel(): any {
    return this._searchModel;
  }

  set searchModel(search: any) {
    this._searchModel = search;

    if (this._searchModel && this._searchModel.model) {
      this.formattedSelection = this._searchModel.formattedNameEmail();
      if (this.viewEditActivityDate) {
        this.customerBookingExists = this.viewEditActivityDate.model.customerBookings.find((x) => x.customerEmail === this._searchModel.model.email)!=null;
      }
    }
    else {
      this.formattedSelection = this._searchModel;
    }
  }

  constructor(
    private customerService: CustomerService,
    private activityService: ActivityService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute) {
    
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
    var thisObj = this;

    var setDefaults = (formGroup: FormGroup, date: Date) => {
      formGroup.controls.activity.enable();
      formGroup.controls.datetime.enable();
      formGroup.controls.priceOption.enable();
      formGroup.controls.datetime.setValue(this.getLocalISOTime(date));
      formGroup.controls.activity.setValue(thisObj.activitySkus[0]);

    }
    var setActivity = (formGroup: FormGroup, activityDate: ActivityDate) => {

      var activitySku = thisObj.activitySkus.find((sku) => sku.id == activityDate.model.activitySkuId);
      formGroup.controls.activity.setValue(activitySku);
      formGroup.controls.activity.disable();

      formGroup.controls.datetime.setValue(this.getLocalISOTime(activityDate.start));
      formGroup.controls.datetime.disable();

      formGroup.controls.priceOption.setValue(activitySku.priceOptions.find((sku) => sku.name == activityDate.model.priceOptionName));
      formGroup.controls.priceOption.disable();
    }
    this.viewEditBooking = (activityDate: ActivityDate) => {
      thisObj.viewEditActivityDate = activityDate;
      thisObj.addingDate = true;

      setActivity(thisObj.addDateForm, activityDate);      

      thisObj.showCancel = false;
    };

    this.appendBooking = (activityDate: ActivityDate) => {
      if (!activityDate)
      {
        var activitySku = this.addDateForm.controls.activity.value as ActivitySku;

        var date = this.offsetLocalDateTime(new Date(this.addDateForm.controls.datetime.value as string));       
        var priceOption = this.addDateForm.controls.priceOption.value as ActivitySkuPrice;
        activityDate = this.createActivity(0, date, activitySku, -1, [], priceOption.name);
      }
      thisObj.viewEditActivityDate = activityDate;
      thisObj.addingBooking = true;
      
      setActivity(thisObj.addBookingForm, activityDate);   
    };

    this.deleteBooking = (customerBooking) => {
      this.activityService.deleteCustomerBooking(this.viewEditActivityDate.model.activitySkuDateId, customerBooking.customerEmail)
        .pipe(first()).subscribe(response => {
          var index = thisObj.viewEditActivityDate.model.customerBookings.findIndex((b) => b.customerEmail === customerBooking.customerEmail);
          thisObj.viewEditActivityDate.model.customerBookings.splice(index,1);
          this.viewEditActivityDate.model.numPersons -= customerBooking.numPersons;
          this.refresh.next();
        });
    };

   
    this.addBooking = (date) => {
      thisObj.addingBooking = true;
      var hour = date.getHours();
      if (hour == 0) { date.setHours(7); }
      setDefaults(thisObj.addBookingForm, date);

      var priceOption = thisObj.activitySkus[0].priceOptions[0];
      thisObj.addBookingForm.controls.priceOption.setValue(priceOption);
      thisObj.personsOption = Array.apply(null, { length: priceOption.maxPersons }).map(Number.call, Number).map(value => value+1);
      thisObj.addBookingForm.controls.numPersons.setValue(thisObj.personsOption[0]);
     };
    this.addDate = (date) => {
      thisObj.addingDate = true;
      var hour = date.getHours();
      if (hour == 0) { date.setHours(7); }
      
      setDefaults(thisObj.addDateForm, date);

      this.viewEditActivityDate = null;

      thisObj.showCancel = true;
    };
  }

  priceOptionChanged() {
    this.priceChanged();
    this.setPersonsOption();
  }
  priceChanged() {
    var priceOption = this.addBookingForm.controls.priceOption.value as ActivitySkuPrice;
    var numPersons = this.addBookingForm.controls.numPersons.value as number;

    this.addBookingForm.controls.price.setValue(priceOption.price / priceOption.maxPersons * numPersons);

  }

  activityChanged() {
    if (this.addingBooking) {
      this.priceChanged();
      this.setPersonsOption();
    }
  }

  setPersonsOption() {
    var priceOption = this.addBookingForm.controls.priceOption.value as ActivitySkuPrice;
    this.personsOption = Array.apply(null, { length: priceOption.maxPersons }).map(Number.call, Number).map(value => value + 1);
    if (this.addBookingForm.controls.numPersons.value > priceOption.maxPersons) {
      this.addBookingForm.controls.numPersons.setValue(priceOption.maxPersons);
    }
    this.priceChanged();
  }
  hasNewConfirmedChanged( event) {
   
    if (!event.currentTarget.checked) {
      this.addBookingForm.controls.paid.setValue(false);      
    }
  }
  hasNewPaidChanged( event) {
    if (event.currentTarget.checked) {
      this.addBookingForm.controls.confirmed.setValue(true);
      this.addBookingForm.controls.confirmed.disable();
    }
    else {
      this.addBookingForm.controls.confirmed.enable();
    }
  }

  hasConfirmedChanged(customerBooking: CustomerBooking, event) {
    customerBooking.hasConfirmed = event.currentTarget.checked;
    if (!customerBooking.hasConfirmed) {
      customerBooking.hasPaid = false;
    }
    this.customerService.updateCustomerBooking(customerBooking)
      .pipe(first())
      .subscribe(response =>
      { }
      );
  }
  hasPaidChanged(customerBooking: CustomerBooking, event) {
    customerBooking.hasPaid = event.currentTarget.checked;
    if (customerBooking.hasPaid) {
      customerBooking.hasConfirmed = true;     
    }

    this.customerService.updateCustomerBooking(customerBooking)
      .pipe(first())
      .subscribe(response =>
      { }
      );
  }
  deleteDate(date: ActivityDate) {
    date.delete();
  }
  customerBookings() {
    if (this.viewEditActivityDate) {
      return this.viewEditActivityDate.model.customerBookings;
    }
    return [];
  }

  getLocalISOTime(date) {
    var tzoffset = date.getTimezoneOffset() * 60000; //offset in milliseconds
    var localISOTime = (new Date(date.getTime() - tzoffset)).toISOString().slice(0, -1);
    return localISOTime;
  }


  createActivity(numPersons: number, date: Date, activitySku: ActivitySku, id: number, customerBookings: CustomerBooking[],priceOptionName:string) {
    return new ActivityDate({
      numPersons: numPersons,
      startDateTime: date,
      activityId: activitySku.activityId,
      activitySkuId: activitySku.id,
      activityName: activitySku.activityName,
      activitySkuName: activitySku.name,
      totalPrice: activitySku.pricePerPerson * numPersons,
      endDateTime: new Date(date.getTime() + (1000 * 60 * 60 * 24) * activitySku.durationDays + (1000 * 60 * 60) * activitySku.durationHours),
      amountPaid: 0,
      deleted: false,
      activitySkuDateId: id,
      customerBookings: customerBookings,
      priceOptionName: priceOptionName
    });
  }
  offsetLocalDateTime(dateTime) {
    var offset = dateTime.getTimezoneOffset() * 60 * 1000;
    return new Date(dateTime.getTime() + offset);
  }
  onSubmit() {
    this.submitted = true;
    if (this.addingBooking) {

      if (this.addBookingForm.invalid) {
        return;
      }
      var activitySku = this.addBookingForm.controls.activity.value as ActivitySku;
      var customer = this.addBookingForm.controls.customer.value as Customer;
      var date = this.offsetLocalDateTime(new Date(this.addBookingForm.controls.datetime.value as string));
      var priceOption = this.addBookingForm.controls.priceOption.value as ActivitySkuPrice;
      var numPersons = +this.addBookingForm.controls.numPersons.value as number;
      var confirmed = this.addBookingForm.controls.confirmed.value as boolean;
      var paid = this.addBookingForm.controls.paid.value as boolean;

      var customerBooking = new CustomerBooking(customer.model.firstName + ' ' + customer.model.lastName, activitySku.name, date, customer.model.email, numPersons, paid, confirmed, priceOption.name);
      if (this.viewEditActivityDate && this.viewEditActivityDate.model.activitySkuDateId < 0) {
        this.viewEditActivityDate.model.customerBookings.push(customerBooking);
        this.addingBooking = false;
        this.resetAddBookingForm();
        this.submitted = false;
        this.refresh.next();
      }
      else {
        if (this.viewEditActivityDate) {
          this.customerBookingExists = this.viewEditActivityDate.model.customerBookings.find((x) => x.customerEmail === customer.model.email)!=null;
          if (this.customerBookingExists) {
           
            return;
          }
        }
        this.customerService
          .addCustomerBooking(customerBooking)
          .pipe(first())
          .subscribe(
            response => {
              if (!this.viewEditActivityDate) {
                var activityDate = this.createActivity(numPersons, date, activitySku, response.activitySkuDateId, [customerBooking], priceOption.name);
                this.hookEvents(activityDate);
                this.activityDates.push(activityDate);
              }
              else {
                this.viewEditActivityDate.model.numPersons += numPersons;
                this.viewEditActivityDate.model.totalPrice = activitySku.pricePerPerson * this.viewEditActivityDate.model.numPersons;
                this.viewEditActivityDate.model.customerBookings.push(customerBooking);
                if (!this.addingDate) {
                  this.viewEditActivityDate = null;
                }
              }
              this.refresh.next();

              this.addingBooking = false;

              if (!this.addingDate) {
                this.resetAddDateForm();
              }
              this.resetAddBookingForm();
              this.submitted = false;
            });
      }
    }
    else {
      if (this.addDateForm.invalid) {
        return;
      }
      if (!this.viewEditActivityDate || this.viewEditActivityDate.model.activitySkuDateId < 0) {

        var activitySku = this.addDateForm.controls.activity.value as ActivitySku;
        var date = this.offsetLocalDateTime(new Date(this.addDateForm.controls.datetime.value as string));
        var priceOption = this.addDateForm.controls.priceOption.value as ActivitySkuPrice;
        var customers = this.viewEditActivityDate ? this.viewEditActivityDate.model.customerBookings : [];
        this.activityService.addDate(new NewActivitySkuDate(activitySku.activityName, activitySku.name, date, customers, priceOption.name)).pipe(first())
          .subscribe(
          id => {
            
            var numPersons = customers.length > 0 ? customers.map((c) => c.numPersons).reduce((sum, current) => sum + current) : 0;
            var activityDate = this.createActivity(numPersons, date, activitySku, id, customers, priceOption.name);
              this.hookEvents(activityDate);
              this.activityDates.push(activityDate);
              this.refresh.next();
              this.addingDate = false;
              this.resetForms();
              this.submitted = false;
              this.viewEditActivityDate = null;
            });
      }
      else {
        this.addingDate = false;
        this.resetForms();
        this.submitted = false;
        this.viewEditActivityDate = null;
      }
    }
  }
  searchCustomerByEmail(email: string) {
    this.customerService.getMany(email).pipe(first()).subscribe(customers => {
      this.customers = customers.map((m) => new Customer(m));
      if (this.customers.length === 1) {
       this._searchModel = this.customers[0];
      }
      else {
        var element = document.getElementById('customer') as HTMLElement;
        element.click();
      }
    });
  }
  searchCustomer(event) {
    if (event) {
      if (event.key != 'Enter')
        return;
      event.preventDefault();
    }
    var search = this.addBookingForm.controls.customer.value;
    if (search && search.model && search.model.firstName) {
      this.searchCustomerByEmail(search.model.email);      
    }
    else {
      this.searchCustomerByEmail(search);
    }
  }
 
  cancelClick() {
    if (this.addingBooking && this.viewEditActivityDate) {
      this.addingBooking = false;
    }
    else {
      this.addingBooking = false;
      this.addingDate = false;
      this.resetForms();
      this.submitted = false;
    }
  }
  resetForms() {
    this.resetAddBookingForm();
    this.resetAddDateForm();
  }
  resetAddDateForm() {
    this.addDateForm.clearValidators();
    this.addDateForm.reset();
  }
  resetAddBookingForm() {
    this.addBookingForm.clearValidators();
    this.addBookingForm.reset();
    this.addBookingForm.controls.confirmed.setValue(false);
    this.addBookingForm.controls.paid.setValue(false);
  }
  ngOnInit() {
    this.addBookingForm = this.formBuilder.group({
      activity: ['', Validators.required],
      customer: ['', Validators.required],
      priceOption: ['', Validators.required],
      numPersons: ['', Validators.required],
      datetime: ['', Validators.required],
      price: ['', Validators.required],
      confirmed: [false],
      paid: [false]
    });
    this.addDateForm = this.formBuilder.group({
      activity: ['', Validators.required],     
      datetime: ['', Validators.required],
      priceOption: ['', Validators.required]
    });
    this.filterCustomerEmail = this.route.snapshot.queryParamMap.get('customerEmail');
    this.filterActivityName = this.route.snapshot.queryParamMap.get('activityName');
    if (this.filterCustomerEmail) {
      this.searchCustomerByEmail(this.filterCustomerEmail);
    }

    this.loadActivities();
    this.loadSchedule();
  }

  inputFormatter = (x: any) => {
    if (x.formattedNameEmail) {
      return x.formattedNameEmail();
    }
    return x;
  }
  formatter = (x: any) => {
    return x.formattedNameEmail();
  } 

  formattedSelection: string;
  search = (text$: Observable<string>) => {
    const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
    const clicksWithClosedPopup$ = this.click$.pipe(filter(() => !this.instance.isPopupOpen()));
    const inputFocus$ = this.focus$;

    var thisObj = this;
    return merge(debouncedText$, inputFocus$, clicksWithClosedPopup$)
      .pipe(
      map(term => (term === '' ? thisObj.customers
        : thisObj.customers.filter(c =>
          c.formattedNameEmail().toLowerCase().includes(term.toLowerCase()))).slice(0, 10)));
  }

  loadActivities() {
    var thisObj = this;
    this.activityService.getMany().pipe(first())
      .subscribe(
        activities => {
          thisObj.activities = activities;
          thisObj.activitySkus = activities.reduce((pn, u) => [...pn, ...u.skus], []);
        });
  }

  refreshActivities() {
    this.activityDates = this.activityDates.filter((d) => {
      return !d.model.deleted &&

        (!this.filterCustomerEmail || d.model.customerBookings.findIndex((cb) => {
          return cb.customerEmail === this.filterCustomerEmail
        }) >= 0) &&

        (!this.filterActivityName || d.model.activityName == this.filterActivityName);
    });
    
  }

  hookEvents(activityDate: ActivityDate):void {
    activityDate.onDeleted.subscribe(() => {
      if (activityDate.model.activitySkuDateId && activityDate.model.activitySkuDateId > 0) {
        this.activityService
          .deleteDate(activityDate.model.activitySkuDateId)
          .pipe(first())
          .subscribe(() => {
            activityDate.model.deleted = true;
            this.refreshActivities();
          });
      }
      else {
        activityDate.model.deleted = true;
        this.refreshActivities();
      }
    });

    activityDate.onTimeChanged.subscribe(() => {
      if (activityDate.model.activitySkuDateId && activityDate.model.activitySkuDateId > 0) {
        this.activityService
          .updateDate(new ActivitySkuDate(activityDate.model.activitySkuDateId, activityDate.start))
          .pipe(first())
          .subscribe(() => {
            this.refresh.next();
          });
      }
    });
  }

  private loadSchedule() {
    this.activityService.getAllActivityDates(null, null)
      .pipe(first())
      .subscribe((dates) => {
        this.activityDates = this.activityDates.concat(dates.map((m) =>
          {
            var activityDate = new ActivityDate(m);
            this.hookEvents(activityDate);
            return activityDate;
        }));

        this.refreshActivities();
    });
  }
}
