import { Component, OnInit } from '@angular/core';
import { ImpersonateService } from './_services/impersonate.service';
import { ProfileService } from './_services/profile.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  public isAuthenticated: boolean
  public loggedInUserProfile: any
  public impersonationUserProfile: any
  public isImpersonating: boolean
  public showWelcomeToast = false
  public initals = false

  constructor(
    private readonly impersonateService: ImpersonateService,
    private readonly profileService: ProfileService
  ) {}

  ngOnInit() {
    var userToken = localStorage.getItem('userToken')
    this.isAuthenticated = userToken != null

    this.profileService.get().subscribe(data => {

      const { loggedInUserProfile, impersonationUserProfile, isImpersonating } = data

      this.loggedInUserProfile = loggedInUserProfile
      this.impersonationUserProfile = impersonationUserProfile
      this.isImpersonating = isImpersonating
      this.initals = (isImpersonating ? impersonationUserProfile : loggedInUserProfile).initals

      this.showWelcomeToast = true
    })
  }

  public onLogoutClicked() {
    localStorage.removeItem('userToken');
    window.location.href = "/mylogin"
  }

  public onExitImpersonationClicked() {

    this.impersonateService.exit().subscribe(data => {
      localStorage.setItem('userToken', data.token)
      window.location.reload()
    })
  }
}
