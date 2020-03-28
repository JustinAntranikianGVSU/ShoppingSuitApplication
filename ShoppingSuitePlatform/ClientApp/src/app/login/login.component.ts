import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiClientService } from '../_services/api-client.service';
import { LoginDto } from '../_models/login';
import { UserTokenResponse } from '../_models/userTokenResponse';
import { LocalStorageService } from '../_services/local-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  public loginInfo: LoginDto

  constructor(
    private readonly apiClientService: ApiClientService,
    private readonly localStorageService: LocalStorageService,
    private readonly route: ActivatedRoute
  )
  {
    this.loginInfo = new LoginDto('jantranikian@gmail.com', '')
  }

  public onLoginClicked() {
    this.apiClientService.login(this.loginInfo).subscribe(this.handleLoginCompleted)
  }

  private handleLoginCompleted = (tokenResponse: UserTokenResponse) => {
    this.localStorageService.setToken(tokenResponse.token)
    const returnUrl = this.route.snapshot.queryParams['returnUrl']
    window.location.href = returnUrl || '/'
  }
}
