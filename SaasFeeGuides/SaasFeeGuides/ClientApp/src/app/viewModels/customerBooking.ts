export class CustomerBooking {

  constructor(
    public customerDisplayName : string,
    public activitySkuName : string,
    public dateTime: Date,
    public customerEmail: string,     
    public numPersons: number,
    public hasPaid: boolean,
    public hasConfirmed: boolean,
    public priceOptionName:string) { }

}
