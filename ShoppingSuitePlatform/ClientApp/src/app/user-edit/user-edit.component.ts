import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserEditService } from '../services/user-edit.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  public user: any

  constructor(
    private route: ActivatedRoute,
    private userService: UserEditService,
  ) {}

  ngOnInit() {
    const userId = parseInt(this.route.snapshot.paramMap.get('id'))
    this.userService.getUser(userId).subscribe(data => this.user = data)
  }
}
