import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CheckBoxComponentBase } from '../_shared/CheckBoxComponentBase';
import { User } from '../_models/user';
import { ApiClientService } from '../_services/api-client.service';
import { concat, forkJoin } from 'rxjs';
import { tap } from 'rxjs/operators';
import * as _ from 'lodash';
import { AccessList, AccessListUpdateDto } from '../_models/accessList';
import { Location } from '../_models/location';

@Component({
  selector: 'app-access-list-edit',
  templateUrl: './access-list-edit.component.html',
  styleUrls: ['./access-list-edit.component.css']
})
export class AccessListEditComponent extends CheckBoxComponentBase implements OnInit {

  public accessList: AccessList
  public locationChunks: Location[][]
  public userChunks: User[][]

  constructor(
    private readonly apiClientService: ApiClientService,
    private readonly route: ActivatedRoute
  ) { super() }

  ngOnInit() {
    concat(this.getAccessList$(), this.getParallelObservables$()).subscribe(() => {})
  }

  private getAccessList$ = () => {
    return this.apiClientService.getAccessList(this.getId()).pipe(
      tap(data => {
        this.accessList = data
      })
    )
  }

  // combines these two api calls into a single observable that is executed in parallel, but only after the getAccessList api call completes. 
  private getParallelObservables$ = () => {
    const parallelCalls$ = [this.getLocations$(), this.getUsers$()]
    return forkJoin(...parallelCalls$)
  }

  private getLocations$ = () => {
    return this.apiClientService.getLocations().pipe(
      tap(data => {
        const ids = this.accessList.locations.map(oo => oo.id)
        this.locationChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  private getUsers$ = () => {
    return this.apiClientService.getUsers().pipe(
      tap(data => {
        const ids = this.accessList.users.map(oo => oo.id)
        this.userChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  public onUpdateClicked() {

    const locationIds = this.extractCheckedIds(this.locationChunks)
    const userIds = this.extractCheckedIds(this.userChunks)
    const accessListUpdate = new AccessListUpdateDto(this.accessList.name, locationIds, userIds)

    this.apiClientService.updateAccessList(this.getId(), accessListUpdate).subscribe(data => {
      const ids = data.locations.map(oo => oo.id);
      this.locationChunks = this.mapToCheckboxChunks(_.flatten(this.locationChunks), ids)
    })
  }

  private getId = () => parseInt(this.route.snapshot.paramMap.get('id'))
}
