import { Component } from '@angular/core';
import { LogoutService } from '../services/logout.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(
    private router: Router,
    private logoutService: LogoutService) {}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public onLogoutClicked() {
    this.logoutService.post().subscribe(
      () => this.router.navigate(['mylogin']),
      error => console.log(error)
    )
  }
}
