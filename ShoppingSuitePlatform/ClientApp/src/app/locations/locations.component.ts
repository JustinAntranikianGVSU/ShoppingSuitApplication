import { Component, OnInit } from '@angular/core';
import { ComponentBase } from '../_shared/componentBase';
import { ApiClientService } from '../_services/api-client.service';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})
export class LocationsComponent extends ComponentBase implements OnInit {

  public dataLoaded = false
  public locations: any[] = [];

  constructor(private readonly apiClientService: ApiClientService) { super() }

  ngOnInit() {
    this.apiClientService.getLocations().subscribe(data => { 
      this.locations = data
      this.dataLoaded = true
    })
  }

}
