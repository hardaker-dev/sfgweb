export class CustomerBooking {

  constructor(
    public name : string,
    public activitySkuName : string,
    public dateTime: Date,
    public customerEmail: string,     
    public numPersons: number,
    public hasPaid: boolean,
    public hasConfirmed: boolean) { }

}
