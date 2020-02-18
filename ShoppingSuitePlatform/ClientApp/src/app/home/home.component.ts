import { Component } from '@angular/core';
import { LoginService } from '../services/login.service';

interface ILoginInfo {
  email: string
  password: string
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public loginInfo: ILoginInfo = {
    email: "",
    password: ""
  }

  constructor(private loginService: LoginService) {}

  public onLoginClicked() {
    this.loginService.post(this.loginInfo).subscribe(
      data => console.log(data),
      error => console.log(error)
    )
  }
}
