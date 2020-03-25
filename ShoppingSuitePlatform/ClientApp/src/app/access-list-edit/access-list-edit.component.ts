import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LocationsService } from '../_services/locations.service';
import { UserService } from '../_services/user-edit.service';
import { concat, forkJoin } from 'rxjs';
import { tap } from 'rxjs/operators';
import * as _ from 'lodash';
import { CheckBoxComponentBase } from '../_shared/CheckBoxComponentBase';

@Component({
  selector: 'app-access-list-edit',
  templateUrl: './access-list-edit.component.html',
  styleUrls: ['./access-list-edit.component.css']
})
export class AccessListEditComponent extends CheckBoxComponentBase implements OnInit {

  public accessList: any = {}
  public locationChunks: any[][]
  public userChunks: any[][]
  private accessListId: number

  constructor(
    private readonly route: ActivatedRoute,
    private readonly userService: UserService,
    private readonly locationService: LocationsService
  ) { super() }

  ngOnInit() {
    this.accessListId = parseInt(this.route.snapshot.paramMap.get('id'))

    concat(this.getAccessList$(), this.getParallelObservables$()).subscribe(() => {})
  }

  private getAccessList$ = () => {
    return this.locationService.getAccessList(this.accessListId).pipe(
      tap(data => {
        this.accessList = data
      })
    )
  }

  // combines these three api calls into a single observable that is executed at once, but only after the getUser call completes. 
  private getParallelObservables$ = () => {
    const parallelCalls$ = [this.getLocations$(), this.getUsers$()]
    return forkJoin(...parallelCalls$)
  }

  private getLocations$ = () => {
    return this.locationService.getAllForClient().pipe(
      tap(data => {
        const ids = this.accessList.locations.map(oo => oo.id)
        this.locationChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  private getUsers$ = () => {
    return this.userService.getAll().pipe(
      tap(data => {
        const ids = this.accessList.users.map(oo => oo.id)
        this.userChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }
 
  public onUpdateClicked() {

    const accessListData = {
      ...this.accessList,
      locationIds: this.extractCheckedIds(this.locationChunks),
      userIds: this.extractCheckedIds(this.userChunks)
    }

    this.locationService.updateAccessList(this.accessListId, accessListData).subscribe(data => {
      const ids = data.locations.map(oo => oo.id);
      this.locationChunks = this.mapToCheckboxChunks(_.flatten(this.locationChunks), ids)
    })
  }

}
