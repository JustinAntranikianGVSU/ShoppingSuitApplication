import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CheckBoxComponentBase } from '../_shared/CheckBoxComponentBase';
import { ApiClientService } from '../_services/api-client.service';
import { concat, forkJoin } from 'rxjs';
import { tap } from 'rxjs/operators';
import * as _ from 'lodash';
import { User, UserUpdateDto } from '../_models/user';
import { Role } from '../_models/role';
import { AccessList } from '../_models/accessList';
import { Location } from '../_models/location';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent extends CheckBoxComponentBase implements OnInit {

  public user: User
  public locationChunks: Location[][]
  public accessListChunks: AccessList[][]
  public roleChunks: Role[][]

  constructor(
    private readonly apiClientService: ApiClientService,
    private readonly route: ActivatedRoute
  ) { super() }

  ngOnInit() {
    concat(this.getUser$(), this.getParallelObservables$()).subscribe(() => {})
  }

  private getUser$ = () => {
    return this.apiClientService.getUser(this.getId()).pipe(
      tap(data => {
        this.user = data
      })
    )
  }

  // combines these three api calls into a single observable that is executed in parallel, but only after the getUser api call completes. 
  private getParallelObservables$ = () => {
    const parallelCalls$ = [this.getLocations$(), this.getAccessLists$(), this.getRoles$()]
    return forkJoin(...parallelCalls$)
  }

  private getLocations$ = () => {
    return this.apiClientService.getLocations().pipe(
      tap(data => {
        const ids = this.user.locations.map(oo => oo.id)
        this.locationChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  private getAccessLists$ = () => {
    return this.apiClientService.getAccessLists().pipe(
      tap(data => {
        const ids = this.user.accessLists.map(oo => oo.id)
        this.accessListChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  private getRoles$ = () => {
    return this.apiClientService.getRoles().pipe(
      tap(data => {
        const ids = this.user.roles.map(oo => oo.id)
        this.roleChunks = this.mapToCheckboxChunks(data, ids)
      })
    )
  }

  public onUpdateUserClicked() {

    const {firstName, lastName, email} = this.user
    const accessListIds = this.extractCheckedIds(this.accessListChunks)
    const roleIds = this.extractCheckedIds(this.roleChunks)

    const userUpdateDto = new UserUpdateDto(firstName, lastName, email, accessListIds, roleIds)

    this.apiClientService.updateUser(this.getId(), userUpdateDto).subscribe(data => {
      const ids = data.locations.map(oo => oo.id);
      this.locationChunks = this.mapToCheckboxChunks(_.flatten(this.locationChunks), ids)
    })
  }

  private getId = () => parseInt(this.route.snapshot.paramMap.get('id'))
}
