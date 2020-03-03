import { Component, OnInit } from '@angular/core';
import { LocationsService } from '../_services/locations.service';

@Component({
  selector: 'app-locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})
export class LocationsComponent implements OnInit {

  public locations: any[];

  constructor(
    private readonly locationService: LocationsService
  ) { }

  ngOnInit() {
    this.locationService.get().subscribe(data =>
      this.locations = data
    )
  }

}
