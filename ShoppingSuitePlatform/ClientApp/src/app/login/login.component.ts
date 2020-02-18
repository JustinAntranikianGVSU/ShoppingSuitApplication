import { Component } from '@angular/core';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';

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
    email: 'csmith@gmail.com',
    password: ''
  }

  constructor(
    private router: Router,
    private loginService: LoginService) {}

  public onLoginClicked() {
    this.loginService.post(this.loginInfo).subscribe(
      () => this.router.navigate(['user-list']),
      error => console.log(error)
    )
  }
}
