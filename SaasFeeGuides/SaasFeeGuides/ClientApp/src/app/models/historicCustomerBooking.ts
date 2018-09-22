export class HistoricCustomerBooking {

  constructor(
    public activitySkuName : string,
    public date: Date,
    public customerEmail: string,     
    public numPersons : number,
    public amountPaid : number) { }

}
