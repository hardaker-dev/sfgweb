import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { User } from '../viewModels/User';
import { GuideService } from '../services/guide.service';
import { Router } from '@angular/router';
import { Guide } from '../viewModels/guide';

@Component({
  templateUrl: 'guides.component.html',
  styleUrls: ['./guides.component.css'] })
export class GuidesComponent implements OnInit {
  currentAccount: User;
  guides: Guide[] = [];
  addingGuide:boolean;
  addGuideForm: FormGroup;
  submitted = false;
  loading = false;
  constructor(
    private guideService: GuideService,
    private formBuilder: FormBuilder,
    private router: Router)
  {
    this.currentAccount = JSON.parse(localStorage.getItem('currentUser'));
 
  }
  filterClicked(guide:Guide) {

    this.router.navigate(['/bookings'], { queryParams: { guideEmail: guide.model.email } });
  }
  onSubmit() {
    this.submitted = true;
    if (this.addGuideForm.invalid) {
      return;
    }
    this.loading = true;
    var guide = new Guide({
      id: null,
      email: this.f.email.value,
      firstName: this.f.firstName.value,
      lastName: this.f.lastName.value,
      address: this.f.address.value,
      dateOfBirth: this.f.dob.value,
      phoneNumber: this.f.phoneNumber.value
    });
    this.guideService
      .add(guide)
      .pipe(first())
      .subscribe(
      id => {
        guide.model.id = id;
        this.guides.push(guide);
          this.loading = false;
          this.addingGuide = false;
          this.addGuideForm.clearValidators();
        this.addGuideForm.reset();
          this.submitted = false;
      },
      error => {
        this.loading = false;        
      });


  }
  ngOnInit() {
   
    this.addGuideForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      dob: ['', Validators.required],
      address: ['', Validators.required]
    });
    this.addingGuide = false;
    this.loading = false;
    this.submitted = false;
    this.loadAllGuides();
  }
  cancelClick() {
    this.addingGuide = false;
    this.addGuideForm.reset();
  }
  addGuideClick() {
    this.addingGuide = true;
  }

  private loadAllGuides() {
    this.guideService.getMany(null)
      .pipe(first())
      .subscribe(guides => {
        this.guides = guides.map((m) => new Guide(m));;
    });
  }

  get f() { return this.addGuideForm.controls; }
}
