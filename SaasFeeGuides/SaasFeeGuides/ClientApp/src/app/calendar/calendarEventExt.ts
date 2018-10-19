import { CalendarEvent } from "calendar-utils";

export interface CalendarEventExt<MetaType = any> extends  CalendarEvent<MetaType> {
  notifyTimeChanged: () => void;
}
