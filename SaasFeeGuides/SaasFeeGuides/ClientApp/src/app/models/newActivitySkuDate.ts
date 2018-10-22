import { CustomerBooking } from "../viewModels/customerBooking";

export class NewActivitySkuDate {

  constructor(
    public activityName: string,
    public activitySkuName: string,
    public dateTime: Date,
    public customerBookings: CustomerBooking[]
  ) {
  }

}
