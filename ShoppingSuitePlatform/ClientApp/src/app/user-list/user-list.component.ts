import { Component, OnInit } from '@angular/core';
import { UserEditService } from '../_services/user-edit.service';
import { ImpersonateService } from '../_services/impersonate.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  public users: any[]
  public dataLoaded = false

  constructor(
    private readonly userService: UserEditService,
    private readonly impersonateService: ImpersonateService
  ) {}
  
  ngOnInit() {
    this.userService.getAll().subscribe(users => { 
      this.users = users
      this.dataLoaded = true
    })
  }

  public onImpersonateClicked(user: any) {

    this.impersonateService.post(user.id).subscribe(data => {
      localStorage.setItem('userToken', data.token)
    })
  }

}
