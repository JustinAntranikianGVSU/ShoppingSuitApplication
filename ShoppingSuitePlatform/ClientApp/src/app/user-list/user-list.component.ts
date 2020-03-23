import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UserEditService } from '../_services/user-edit.service';
import { ImpersonateService } from '../_services/impersonate.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

const commonNames = ['Justin', 'Bob', 'Barry', 'Calvin', 'Don', 'Chris']

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  public users: any[]
  public dataLoaded = false
  public userToImpersonate: any
  public showErrorToast = false
  public isFilterCollapsed = false
  public filterModel: any
  public listView = true

  @ViewChild('impersonationComplete', null) 
  private impersonationCompleteRef: ElementRef

  @ViewChild('impersonationConfirm', null) 
  private impersonationConfirmRef: ElementRef

  constructor(
    private readonly userService: UserEditService,
    private readonly impersonateService: ImpersonateService,
    private readonly router: Router,
    private readonly modalService: NgbModal
  ) {}
  
  ngOnInit() {
    this.userService.getAll().subscribe(users => { 
      this.users = users
      this.dataLoaded = true
    })
  }

  public onImpersonateClicked(user: any) {
    
    this.userToImpersonate = user

    this.modalService.open(this.impersonationConfirmRef, {ariaLabelledBy: 'modal-basic-title'}).result.then(
      this.handleImpersonationConfirmed, 
      () => {}
    );
  }

  private handleImpersonationConfirmed = () => {

    const {id} = this.userToImpersonate

    this.impersonateService.post(id).subscribe(
      data => {
        localStorage.setItem('userToken', data.token)
        this.modalService.open(this.impersonationCompleteRef).result.then(this.handleImpersonationLinkClicked)
      }, 
      error => this.showErrorToast = error.status === 403
    )
  }

  private handleImpersonationLinkClicked = (linkType: string) => {
    linkType === 'Reload' ? window.location.reload() : this.router.navigate(['/mylocations'])
  }

  public onFirstNameFilterClicked() {
    this.users = this.users.filter(oo => oo.firstName.toLowerCase() == this.filterModel.toLowerCase())
  }

  public searchUsers = (text$: Observable<string>) => {
    return text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? [] : commonNames.filter(v => v.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10))
    )
  }
}
