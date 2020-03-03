import { Component } from '@angular/core';
import { LoginService } from '../_services/login.service';
import { Router, ActivatedRoute } from '@angular/router';

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
    private router: Router,
    private route: ActivatedRoute,
    private loginService: LoginService
  ) {}

  public onLoginClicked() {
    this.loginService.post(this.loginInfo).subscribe(
      (data) => {

        localStorage.setItem('userToken', data.token)

        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || 'user-list'
        this.router.navigate([returnUrl])
      },
      error => console.log(error)
    )
  }
}
