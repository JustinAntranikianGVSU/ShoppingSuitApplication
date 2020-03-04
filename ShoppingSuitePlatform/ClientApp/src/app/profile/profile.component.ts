import { Component, OnInit } from '@angular/core';
import { LocationsService } from '../_services/locations.service';
import { ProfileService } from '../_services/profile.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public profileData: any = {
    loggedInUser: {},
    loggedInClient: {},
    isImpersonating: false,
    impersonatingUser: {},
    impersonatingClient: {},
    loggedInUserLocations: [],
    impersonatingUserLocations: []
  }
  public locations: any[] = []

  constructor(
    private readonly profileService: ProfileService
  ) {}

  ngOnInit() {
    this.profileService.get().subscribe(data => this.profileData = data)
  }

}
