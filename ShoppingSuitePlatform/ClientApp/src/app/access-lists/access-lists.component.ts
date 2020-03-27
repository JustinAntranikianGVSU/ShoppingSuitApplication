import { Component, OnInit } from '@angular/core';
import { ComponentBase } from '../_shared/componentBase';
import { ApiClientService } from '../_services/api-client.service';

@Component({
  selector: 'app-access-lists',
  templateUrl: './access-lists.component.html',
  styleUrls: ['./access-lists.component.css']
})
export class AccessListsComponent extends ComponentBase implements OnInit {

  public accessLists: any[]

  constructor(private readonly apiClientService: ApiClientService) { super() }

  ngOnInit() {
    this.apiClientService.getAccessLists().subscribe(data => this.accessLists = data)
  }
}
