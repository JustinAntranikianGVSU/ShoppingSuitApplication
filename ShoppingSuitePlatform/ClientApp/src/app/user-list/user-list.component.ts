import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UserEditService } from '../_services/user-edit.service';
import { ImpersonateService } from '../_services/impersonate.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  public users: any[]
  public dataLoaded = false
  public userToImpersonate: any
  
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

    this.impersonateService.post(id).subscribe(data => {
      localStorage.setItem('userToken', data.token)
      this.modalService.open(this.impersonationCompleteRef).result.then(this.handleImpersonationLinkClicked)
    })
  }

  private handleImpersonationLinkClicked = (linkType: string) => {
    if (linkType === 'Reload') {
      window.location.reload()
      return
    }

    this.router.navigate(['/locations'])
  }

}
