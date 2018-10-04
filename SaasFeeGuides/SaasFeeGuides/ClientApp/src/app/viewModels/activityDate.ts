import { CalendarEvent, EventColor, EventAction } from 'calendar-utils';

import { colors } from '../utils/colors';

export interface ActivityDateModel  {
  activitySkuDateId: number;
  activityId: number;
  activitySkuId: number;
  activityName: string;
  activitySkuName: string;
  dateTime: Date;
  numPersons: number;
  amountPaid: number;
  totalPrice: number
}
export class ActivityDate implements CalendarEvent {
    id?: string | number;
    start: Date;
    end?: Date;
    title: string;
    color?: EventColor;
    actions?: EventAction[];
    allDay?: boolean;
    cssClass?: string;
    resizable?: { beforeStart?: boolean; afterEnd?: boolean; };
    draggable?: boolean;
    meta?: any;

  constructor(public model: ActivityDateModel   ) {
    this.color = colors.yellow;
    this.draggable = true;
    this.start =  new Date(model.dateTime);
    this.title = model.activityName;
  }
  
}

