import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserEditService } from '../_services/user-edit.service';
import { LocationsService } from '../_services/locations.service';
import * as _ from 'lodash'; 
import { concat, forkJoin } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  public user: any
  public locationChunks: any[]
  public accessListChunks: any[]
  public roleChunks: any[]

  private readonly columnChunks = 4
  private userId: number

  constructor(
    private readonly route: ActivatedRoute,
    private readonly userService: UserEditService,
    private readonly locationService: LocationsService
  ) {}

  ngOnInit() {
    this.userId = parseInt(this.route.snapshot.paramMap.get('id'))

    const userObservable = this.userService.getUser(this.userId).pipe(tap(data => this.user = data))
    concat(userObservable, this.getParallelObservables()).subscribe(() => {})
  }

  // combines these three api calls into a single observable that is executed at once, but only after the getUser call completes. 
  private getParallelObservables = () => {
    const locationsObservable = this.locationService.getAllForClient().pipe(tap(this.mapLocations))
    const accessListsObservable = this.locationService.getAccessLists().pipe(tap(this.mapAccessLists))
    const rolesObservable = this.locationService.getRoles().pipe(tap(this.mapRoles))

    return forkJoin(locationsObservable, accessListsObservable, rolesObservable)
  }

  private mapLocations = (data) => {
    const ids = this.user.locations.map(oo => oo.id)
    this.locationChunks = this.mapToCheckboxItems(data, ids)
  }

  private mapAccessLists = (data) => {
    const ids = this.user.accessLists.map(oo => oo.id)
    this.accessListChunks = this.mapToCheckboxItems(data, ids)
  }

  private mapRoles = (data) => {
    const ids = this.user.roles.map(oo => oo.id)
    this.roleChunks = this.mapToCheckboxItems(data, ids)
  }

  private mapToCheckboxItems = (data: any[], ids: number[]) => {
    const mapToCheckboxFunc = this.mapToCheckbox(ids)
    const checkboxes = data.map(mapToCheckboxFunc);
    return this.splitIntoChunksOfFour(checkboxes)
  }

  private mapToCheckbox = (ids: number[]) => (item: any) =>
  ({ 
    isChecked: ids.includes(item.id), 
    id: item.id, 
    name: item.name
  })
  
  private splitIntoChunksOfFour = (items: any[]) => {

    const sizeForEachArray = Math.ceil(items.length / this.columnChunks)

    const chunk1 = items.splice(0, sizeForEachArray);
    const chunk2 = items.splice(0, sizeForEachArray);
    const chunk3 = items.splice(0, sizeForEachArray);
    const chunk4 = items.splice(0, this.getItemCountForLastChunk(items));
    
    return [chunk1, chunk2, chunk3, chunk4]
  }

  private getItemCountForLastChunk = (items: any[]) => {
    const remainder = items.length % this.columnChunks
    return remainder != 0 ? remainder : this.columnChunks
  }

  public onUpdateUserClicked() {

    const userData = {
      ...this.user,
      roleIds: this.extractCheckedIds(this.roleChunks),
      accessListIds: this.extractCheckedIds(this.accessListChunks)
    }

    this.userService.updateUser(this.userId, userData).subscribe(data => {
      const ids = data.locations.map(oo => oo.id);
      this.locationChunks = this.mapToCheckboxItems(_.flatten(this.locationChunks), ids)
    })
  }

  private extractCheckedIds = (chunkedData: any) => {
    return _.flatten(chunkedData).filter(oo => oo.isChecked).map(oo => oo.id)
  }

}
