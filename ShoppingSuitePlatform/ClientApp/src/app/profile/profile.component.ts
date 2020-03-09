import { Component, OnInit } from '@angular/core';
import { ProfileService } from '../_services/profile.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public dataLoaded = false
  public loggedInUserProfile: any
  public impersonationUserProfile: any
  public isImpersonating: boolean

  constructor(private readonly profileService: ProfileService) {}

  ngOnInit() {
    this.profileService.get().subscribe(data => {

      const { loggedInUserProfile, impersonationUserProfile, isImpersonating } = data

      this.loggedInUserProfile = loggedInUserProfile
      this.impersonationUserProfile = impersonationUserProfile
      this.isImpersonating = isImpersonating

      this.dataLoaded = true
    })
  }

}
