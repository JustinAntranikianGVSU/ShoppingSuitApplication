import { Component, OnInit } from '@angular/core';
import { LocationsService } from '../_services/locations.service';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})
export class LocationsComponent implements OnInit {

  public dataLoaded = false
  public locations: any[] = [];

  constructor(private readonly locationService: LocationsService) {}

  ngOnInit() {
    this.locationService.getAllForClient().subscribe(data => { 
      this.locations = data
      this.dataLoaded = true
    })
  }

}
