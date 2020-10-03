import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Console } from 'console';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TaxesClient';
  displayedColumns: string[] = ['Municipality', 'Date', 'TaxesRatio', 'TaxesSchedule'];
  loadedTaxes: any;
  constructor(readonly httpClient: HttpClient) {
    
  }

  ngOnInit() {
    console.log('init call');

    this.httpClient.get('http://localhost:5005/api/Taxes').subscribe(i =>{
        console.log(i)
        this.loadedTaxes = i;
    })
  }
}
