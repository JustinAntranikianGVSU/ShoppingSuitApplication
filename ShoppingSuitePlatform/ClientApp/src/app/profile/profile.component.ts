import { Component, OnInit } from '@angular/core';
import { ApiClientService } from '../_services/api-client.service';
import { User } from '../_models/user';
import { ComponentBase } from '../_shared/componentBase';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent extends ComponentBase implements OnInit {

  public dataLoaded = false
  public loggedInUserProfile: User
  public impersonationUserProfile: User
  public isImpersonating: boolean

  constructor(private readonly apiClientService: ApiClientService) { super() }

  ngOnInit() {
    this.apiClientService.getProfile().subscribe(data => {

      const { loggedInUserProfile, impersonationUserProfile, isImpersonating } = data

      this.loggedInUserProfile = loggedInUserProfile
      this.impersonationUserProfile = impersonationUserProfile
      this.isImpersonating = isImpersonating

      this.dataLoaded = true
    })
  }

}
