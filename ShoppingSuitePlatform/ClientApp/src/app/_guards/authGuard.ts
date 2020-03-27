import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AppConstants } from '../_shared/appConstants';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    if (localStorage.getItem(AppConstants.UserToken)) {
      return true;
    }

    this.router.navigate(['mylogin'], { queryParams: { returnUrl: state.url }});
    return false;
  }
}