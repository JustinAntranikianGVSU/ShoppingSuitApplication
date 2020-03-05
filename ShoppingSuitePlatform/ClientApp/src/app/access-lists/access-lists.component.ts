import { Component, OnInit } from '@angular/core';
import { LocationsService } from '../_services/locations.service';

@Component({
  selector: 'app-access-lists',
  templateUrl: './access-lists.component.html',
  styleUrls: ['./access-lists.component.css']
})
export class AccessListsComponent implements OnInit {

  public accessLists: any[]

  constructor(
    private readonly locationService: LocationsService
  ) {}

  ngOnInit() {
    this.locationService.getAccessLists().subscribe(data => this.accessLists = data)
  }

}
