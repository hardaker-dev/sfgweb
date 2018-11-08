
import { Content } from './content';


export class ActivitySkuPrice {

  constructor(
    public activitySkuId: number,
    public name: string,
    public descriptionContent: Content[],
    public discountCode: string,
    public price: number,
    public maxPersons: number,
    public minPersons: number,
    public discountPercentage :number,
    public validFrom :Date,
    public validTo: Date
  ) {
  }

}


