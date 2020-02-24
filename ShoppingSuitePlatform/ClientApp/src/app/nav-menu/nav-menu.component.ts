import { Component } from '@angular/core';
import { LogoutService } from '../_services/logout.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(
    private router: Router,
    private logoutService: LogoutService
  ) {}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  public onLogoutClicked() {
    localStorage.removeItem('userToken');
    this.router.navigate(['mylogin'])  
  }
}
