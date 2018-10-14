export class CustomerBooking {

  constructor(
    public activitySkuName : string,
    public dateTime: Date,
    public customerEmail: string,     
    public numPersons : number) { }

}
