export interface CustomerModel {

  id: number;
  email: string;
  firstName: string;
  lastName: string;
  address: string;
  dateOfBirth: Date;
  phoneNumber: string;
}

export class Customer {

  constructor(
    public model: CustomerModel
  ) {
  }

  public formattedName() {
    return this.model.firstName + ' ' + this.model.lastName;
  }

  public formattedNameEmail() {
    return this.model.firstName + ' ' + this.model.lastName + ', ' + this.model.email;
  }
}
