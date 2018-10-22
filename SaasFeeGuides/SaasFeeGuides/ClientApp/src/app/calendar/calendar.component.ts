import { Component, ChangeDetectionStrategy,Input } from '@angular/core';
import { Subject } from 'rxjs';
import {
  CalendarEvent,
  CalendarEventTimesChangedEvent,

  CalendarEventTimesChangedEventType
} from 'angular-calendar';
import { CalendarEventExt } from './calendarEventExt';


export interface CalendarEventTimesChangedEventExt<MetaType = any> {
  type: CalendarEventTimesChangedEventType;
  event: CalendarEventExt<MetaType>;
  newStart: Date;
  newEnd?: Date;
  allDay?: boolean;
}


@Component({
  selector: 'mwl-demo-component',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: 'calendar.component.html'
})
export class CalendarComponent {
  view: string = 'week';

  viewDate: Date = new Date();
  
  vm = this;
  @Input() public eventClicked: void;
  @Input() public addDate: void;
  @Input() public addBooking: void;
  @Input() public deleteBooking: void;
  @Input() public deleteDate: void;  
  @Input() public viewEditBooking: void;
  @Input() public appendBooking: void;
  @Input() public events: CalendarEventExt[];

 
  @Input()  public refresh: Subject<any> = new Subject();

  

  eventTimesChanged({
    event,
    newStart,
    newEnd
  }: CalendarEventTimesChangedEventExt): void {
    event.start = newStart;
    event.end = newEnd;
    event.notifyTimeChanged();
    this.refresh.next();
  }
}
