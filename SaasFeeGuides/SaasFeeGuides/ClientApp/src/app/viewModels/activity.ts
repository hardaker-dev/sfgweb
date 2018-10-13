
import { Content } from './content';

export class Activity {

  constructor(
    public id: number,
    public name: string,
    public titleContent: Content[],
    public descriptionContent: Content[],
    public menuImageContent: Content[],
    public videoContent: Content[],
    public imageContent: Content[],
    public isActive: boolean,
    public categoryName: string,
    public skus: any[],
    public equiptment:any[]
    
  ) {
  }
  
}


