import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiClientService } from '../_services/api-client.service';
import { AppConstants } from '../_shared/appConstants';
import { LoginDto } from '../_models/login';
import { UserTokenResponse } from '../_models/userTokenResponse';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  public loginInfo: LoginDto

  constructor(
    private readonly apiClientService: ApiClientService,
    private readonly route: ActivatedRoute
  )
  {
    this.loginInfo = new LoginDto('jantranikian@gmail.com', '')
  }

  public onLoginClicked() {
    this.apiClientService.login(this.loginInfo).subscribe(this.handleLoginCompleted)
  }

  private handleLoginCompleted = (tokenResponse: UserTokenResponse) => {
    localStorage.setItem(AppConstants.UserToken, tokenResponse.token)
    const returnUrl = this.route.snapshot.queryParams['returnUrl']
    window.location.href = returnUrl || '/'
  }
}
