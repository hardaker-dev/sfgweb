import { Component, ChangeDetectionStrategy,Input } from '@angular/core';
import { Subject } from 'rxjs';
import {
  CalendarEvent,
  CalendarEventTimesChangedEvent
} from 'angular-calendar';

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
  @Input() public viewEditBooking: void;
  @Input() public appendBooking: void;
  @Input() public events: CalendarEvent[];

 
  refresh: Subject<any> = new Subject();

  

  eventTimesChanged({
    event,
    newStart,
    newEnd
  }: CalendarEventTimesChangedEvent): void {
    event.start = newStart;
    event.end = newEnd;
    this.refresh.next();
  }
}
