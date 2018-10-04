

export class ActivityDate {

  constructor(
    public activitySkuDateId: number,
    public activityId: number,
    public activitySkuId: number,
    public activityName: string,
    public activitySkuName: string,
    public dateTime: Date,
    public numPersons: number,
    public amountPaid: number,
    public totalPrice : number
    
  ) {
  }
  
}

