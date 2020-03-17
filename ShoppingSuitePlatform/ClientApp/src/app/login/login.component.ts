import { Component } from '@angular/core';
import { LoginService } from '../_services/login.service';
import { ActivatedRoute } from '@angular/router';

interface ILoginInfo {
  email: string
  password: string
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  public loginInfo: ILoginInfo = {
    email: 'jantranikian@gmail.com',
    password: ''
  }

  constructor(
    private readonly route: ActivatedRoute,
    private readonly loginService: LoginService
  ) {}

  public onLoginClicked() {
    this.loginService.post(this.loginInfo).subscribe(
      data => {
        localStorage.setItem('userToken', data.token)
        const returnUrl = this.route.snapshot.queryParams['returnUrl']
        window.location.href = returnUrl || '/'
      },
      error => console.log(error)
    )
  }
}
