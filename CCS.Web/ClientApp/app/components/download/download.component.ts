import { Component } from '@angular/core';

@Component({
    selector: 'download',
    templateUrl: './download.component.html'
})
export class DownloadComponent {
    public currentCount = 0;

    public incrementCounter() {
        this.currentCount++;
    }

}
