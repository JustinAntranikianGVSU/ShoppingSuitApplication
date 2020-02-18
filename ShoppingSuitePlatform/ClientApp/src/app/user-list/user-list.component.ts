import { Component, OnInit } from '@angular/core';
import { UserEditService } from '../services/user-edit.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  public users: any[]
  public dataLoaded = false

  constructor(
    private userService: UserEditService,
  ) {}
  
  ngOnInit() {
    this.userService.getAll().subscribe(users => { 
      this.users = users
      this.dataLoaded = true
    })
  }

}
