
import { Content } from './content';
import { ActivitySkuPrice } from './activitySkuPrice';


export class ActivitySku {

  constructor(
    public id: number,
    public activityName: string,
    public name: string,
    public titleContent: Content[],
    public descriptionContent: Content[],
    public pricePerPerson: number,
    public minPersons: number,
    public maxPersons: number,
    public additionalRequirementsContent: Content[],
    public durationDays: number,
    public durationHours: number,
    public webContentId: string,
    public activityId: number,
    public priceOptions: ActivitySkuPrice[]

  ) {
  }

}


