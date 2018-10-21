import { CalendarEvent, EventColor, EventAction } from 'calendar-utils';

import { colors } from '../utils/colors';
import { EventDispatcher } from "strongly-typed-events";
import { CalendarEventExt } from '../calendar/calendarEventExt';
import { CustomerBooking } from './customerBooking';

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
  deleted?: boolean;
  customerBookings: CustomerBooking[];
}
export class ActivityDate implements CalendarEventExt {
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
  private _onTimeChanged = new EventDispatcher<ActivityDate, void>();
  get onTimeChanged() {
    return this._onTimeChanged.asEvent();
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
  notifyTimeChanged() {
    this.model.startDateTime = this.start;
    this.model.endDateTime = this.end;
    this._onTimeChanged.dispatch(this, null);
  }
  delete() {
    this._onDeleted.dispatch(this, null);
  }
  
}

