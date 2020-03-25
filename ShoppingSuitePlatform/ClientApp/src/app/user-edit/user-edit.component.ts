import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../_services/user-edit.service';
import { LocationsService } from '../_services/locations.service';
import { concat, forkJoin } from 'rxjs';
import { tap } from 'rxjs/operators';
import * as _ from 'lodash';
import { CheckBoxComponentBase } from '../_shared/CheckBoxComponentBase';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent extends CheckBoxComponentBase implements OnInit {

  public user: any
  public locationChunks: any[][]
  public accessListChunks: any[][]
  public roleChunks: any[][]
  private userId: number

  constructor(
    private readonly route: ActivatedRoute,
    private readonly userService: UserService,
    private readonly locationService: LocationsService
  ) { super() }

  ngOnInit() {
    this.userId = parseInt(this.route.snapshot.paramMap.get('id'))

    concat(this.getUser$(), this.getParallelObservables$()).subscribe(() => {})
  }

  private getUser$ = () => {
    return this.userService.getUser(this.userId).pipe(
      tap(data => {
        this.user = data
      })
    )
  }

  // combines these three api calls into a single observable that is executed at once, but only after the getUser call completes. 
  private getParallelObservables$ = () => {
    const parallelCalls$ = [this.getLocations$(), this.getAccessLists$(), this.getRoles$()]
    return forkJoin(...parallelCalls$)
  }

  private getLocations$ = () => {
    return this.locationService.getAllForClient().pipe(
      tap(data => {
        const ids = this.user.locations.map(oo => oo.id)
        this.locationChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  private getAccessLists$ = () => {
    return this.locationService.getAccessLists().pipe(
      tap(data => {
        const ids = this.user.accessLists.map(oo => oo.id)
        this.accessListChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  private getRoles$ = () => {
    return this.locationService.getRoles().pipe(
      tap(data => {
        const ids = this.user.roles.map(oo => oo.id)
        this.roleChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  public onUpdateUserClicked() {

    const userData = {
      ...this.user,
      roleIds: this.extractCheckedIds(this.roleChunks),
      accessListIds: this.extractCheckedIds(this.accessListChunks)
    }

    this.userService.updateUser(this.userId, userData).subscribe(data => {
      const ids = data.locations.map(oo => oo.id);
      this.locationChunks = this.mapToCheckboxChunks(_.flatten(this.locationChunks), ids)
    })
  }

}
