import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { UtilsModule } from '../utils/util.module';
import { ContextMenuModule } from 'ngx-contextmenu';
import { CalendarComponent } from './calendar.component';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';

@NgModule({
  imports: [
    CommonModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    ContextMenuModule.forRoot({
      useBootstrap4: true
    }),
    UtilsModule
  ],
  declarations: [CalendarComponent],
  exports: [CalendarComponent]
})
export class SfgCalendarModule {}
