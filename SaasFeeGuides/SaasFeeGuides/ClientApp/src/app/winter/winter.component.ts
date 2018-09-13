import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DashboardIndex } from './DashboardIndex';

@Component({
  selector: 'app-winter',
  templateUrl: './winter.component.html'
})
export class WinterComponent {
  public dashboardIndex: DashboardIndex;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    const idToken = localStorage.getItem("id_token");

    if (idToken) {
      var httpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + idToken
        })
      };
     

      http.get<DashboardIndex>(baseUrl + 'api/dashboard/index', httpOptions).subscribe(result => {
        this.dashboardIndex = result;
      }, error => console.error(error));
    }
    else {
      var x = 5;
      
    }

    


  }
}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
