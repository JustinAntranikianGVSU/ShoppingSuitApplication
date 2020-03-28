import { Component, OnInit } from '@angular/core';
import { RouteConstants } from './_shared/routeConstants';
import { ComponentBase } from './_shared/componentBase';
import { ApiClientService } from './_services/api-client.service';
import { User } from './_models/user';
import { ProfileData } from './_models/profileData';
import { LocalStorageService } from './_services/local-storage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent extends ComponentBase implements OnInit {
  public isAuthenticated: boolean
  public loggedInUserProfile: User
  public impersonationUserProfile: User
  public isImpersonating: boolean
  public showWelcomeToast = false
  public initals = ""

  constructor(
    private readonly apiClientService: ApiClientService,
    private readonly localStorageService: LocalStorageService
  ) { super() }

  ngOnInit() {
    this.isAuthenticated = this.localStorageService.hasToken()
    this.apiClientService.getProfile().subscribe(this.handleGetProfileCompleted)
  }

  private handleGetProfileCompleted = ({loggedInUserProfile, impersonationUserProfile, isImpersonating}: ProfileData) => {
    this.loggedInUserProfile = loggedInUserProfile
    this.impersonationUserProfile = impersonationUserProfile
    this.isImpersonating = isImpersonating
    this.initals = (isImpersonating ? impersonationUserProfile : loggedInUserProfile).initals
    this.showWelcomeToast = true
  }

  public onLogoutClicked() {
    this.localStorageService.removeToken()
    window.location.href = RouteConstants.LoginPage
  }

  public onExitImpersonationClicked() {

    this.apiClientService.exitImpersonation().subscribe(data => {
      this.localStorageService.setToken(data.token)
      window.location.reload()
    })
  }
}
