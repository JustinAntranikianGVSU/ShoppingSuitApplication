import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserEditService } from '../_services/user-edit.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  public user: any
  public locations: any[]

  constructor(
    private readonly route: ActivatedRoute,
    private readonly userService: UserEditService
  ) {}

  ngOnInit() {
    const userId = parseInt(this.route.snapshot.paramMap.get('id'))
    this.userService.getUser(userId).subscribe(data => this.user = data)
  }
}
