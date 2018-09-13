import { Component } from '@angular/core';

@Component({
  selector: 'app-summer-component',
  templateUrl: './summer.component.html'
})
export class SummerComponent {
  public currentCount = 0;

  public incrementCounter() {
    this.currentCount++;
  }
}
