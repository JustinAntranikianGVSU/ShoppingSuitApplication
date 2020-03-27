import { Component, OnInit } from '@angular/core';
import { RouteConstants } from './_shared/routeConstants';
import { ComponentBase } from './_shared/componentBase';
import { ApiClientService } from './_services/api-client.service';
import { AppConstants } from './_shared/appConstants';
import { User } from './_models/user';
import { ProfileData } from './_models/profileData';

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

  constructor(private readonly apiClientService: ApiClientService) { super() }

  ngOnInit() {
    var userToken = localStorage.getItem(AppConstants.UserToken)
    this.isAuthenticated = userToken != null
    this.apiClientService.getProfile().subscribe(this.handleGetProfileCompleted)
  }

  private handleGetProfileCompleted = (data: ProfileData) => {
    const {loggedInUserProfile, impersonationUserProfile, isImpersonating} = data

    this.loggedInUserProfile = loggedInUserProfile
    this.impersonationUserProfile = impersonationUserProfile
    this.isImpersonating = isImpersonating
    this.initals = (isImpersonating ? impersonationUserProfile : loggedInUserProfile).initals

    this.showWelcomeToast = true
  }

  public onLogoutClicked() {
    localStorage.removeItem(AppConstants.UserToken);
    window.location.href = RouteConstants.LoginPage
  }

  public onExitImpersonationClicked() {

    this.apiClientService.exitImpersonation().subscribe(data => {
      localStorage.setItem(AppConstants.UserToken, data.token)
      window.location.reload()
    })
  }
}
