import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'statistics',
    templateUrl: './statistics.component.html'
})
export class StatisticsComponent {
   

    public statistics: Statistics;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/about/statistics').subscribe(result => {
            this.statistics = result.json() as Statistics;
        }, error => console.error(error));
    }
    
}
interface Statistics {
    Users: number;
    Drivers: number;
    Passengers: number;
    Organizations: number;

}
