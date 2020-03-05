import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LocationsService } from '../_services/locations.service';

@Component({
  selector: 'app-access-list-edit',
  templateUrl: './access-list-edit.component.html',
  styleUrls: ['./access-list-edit.component.css']
})
export class AccessListEditComponent implements OnInit {

  public accessList: any = {}

  constructor(
    private readonly route: ActivatedRoute,
    private readonly locationsService: LocationsService
  ) {}

  ngOnInit() {
    const accessListId = parseInt(this.route.snapshot.paramMap.get('id'))
    this.locationsService.getAccessList(accessListId).subscribe(data => this.accessList = data)
  }

}
