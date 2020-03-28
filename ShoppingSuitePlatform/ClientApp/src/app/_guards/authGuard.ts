import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { LocalStorageService } from '../_services/local-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(
    private readonly localStorageService: LocalStorageService,
    private readonly router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    if (this.localStorageService.hasToken()) {
      return true;
    }

    this.router.navigate(['mylogin'], { queryParams: { returnUrl: state.url }});
    return false;
  }
}