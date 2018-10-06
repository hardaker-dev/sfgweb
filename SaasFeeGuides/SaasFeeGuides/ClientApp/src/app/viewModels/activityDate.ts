import { CalendarEvent, EventColor, EventAction } from 'calendar-utils';

import { colors } from '../utils/colors';
import { EventDispatcher } from "strongly-typed-events";

export interface ActivityDateModel  {
  activitySkuDateId: number;
  activityId: number;
  activitySkuId: number;
  activityName: string;
  activitySkuName: string;
  startDateTime: Date;
  endDateTime: Date;
  numPersons: number;
  amountPaid: number;
  totalPrice: number;
  deleted: boolean;
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

  private _onDeleted = new EventDispatcher<ActivityDate,void>();
  get onDeleted() {
    return this._onDeleted.asEvent();
  }

  constructor(public model: ActivityDateModel) {

    this.color = colors.blue;
    this.draggable = model.amountPaid == 0;
    this.start = new Date(model.startDateTime);
    this.end = new Date(model.endDateTime);
    this.title = `${model.activityName}, Persons: ${model.numPersons}` ;

    this.actions = [];
    if (model.numPersons == 0) {
      this.actions.push(
        {
          label: '<i class="fa fa-fw fa-times"></i>',
          onClick: ({ event }: { event: CalendarEvent }): void => {           
            this._onDeleted.dispatch(this, null);
          }
        });
    }
  }
  
}

