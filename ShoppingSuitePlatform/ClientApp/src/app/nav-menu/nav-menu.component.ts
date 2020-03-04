import { Component } from '@angular/core';
import { LogoutService } from '../_services/logout.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ImpersonateService } from '../_services/impersonate.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(
    private router: Router,
    private logoutService: LogoutService,
    private readonly impersonateService: ImpersonateService
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

  public onExitImpersonationClicked() {

    this.impersonateService.exit().subscribe(data => {
      localStorage.setItem('userToken', data.token)
      window.location.reload()
    })
  }
}
