import { Component, ChangeDetectionStrategy } from '@angular/core';
import { Subject } from 'rxjs';
import {
  CalendarEvent,
  CalendarEventTimesChangedEvent
} from 'angular-calendar';
import { colors } from '../utils/colors';

@Component({
  selector: 'mwl-demo-component',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: 'calendar.component.html'
})
export class CalendarComponent {
  view: string = 'month';

  viewDate: Date = new Date();

  events: CalendarEvent[] = [
    {
      title: 'Draggable event',
      color: colors.yellow,
      start: new Date(),
      draggable: true
    },
    {
      title: 'A non draggable event',
      color: colors.blue,
      start: new Date()
    }
  ];

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
