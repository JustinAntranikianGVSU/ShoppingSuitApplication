import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, UserUpdateDto } from '../_models/user';
import { AccessList, AccessListUpdateDto } from '../_models/accessList';
import { Role } from '../_models/role';
import { ProfileData } from '../_models/profileData';
import { Location } from '../_models/location';
import { LoginDto } from '../_models/login';
import { UserTokenResponse } from '../_models/userTokenResponse';

@Injectable({
  providedIn: 'root'
})
export class ApiClientService {

  constructor(
    private readonly http: HttpClient, 
    @Inject('BASE_URL') private readonly baseUrl: string
  ) { }

  /// Users
  public getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + `User`)
  }

  public getUser(userId: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + `User/${userId}`)
  }

  public updateUser(id: number, userUpdateDto: UserUpdateDto): Observable<User> {
    return this.http.put<User>(this.baseUrl + `User/${id}`, userUpdateDto)
  }

  // AccessLists
  public getAccessLists(): Observable<AccessList[]> {
    return this.http.get<AccessList[]>(this.baseUrl + `AccessList`)
  }

  public getAccessList(accessListId: number): Observable<AccessList> {
    return this.http.get<AccessList>(this.baseUrl + `AccessList/${accessListId}`)
  }

  public updateAccessList(id: number, accessListUpdate: AccessListUpdateDto): Observable<AccessList> {
    return this.http.put<AccessList>(this.baseUrl + `AccessList/${id}`, accessListUpdate)
  }

  /// Locations
  public getLocations(): Observable<Location[]> {
    return this.http.get<Location[]>(this.baseUrl + `Locations`)
  }

  /// Roles
  public getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(this.baseUrl + `Roles`)
  }

  /// Impersonation
  public impersonate(userId: number): Observable<UserTokenResponse> {
    return this.http.post<UserTokenResponse>(this.baseUrl + `Impersonation`, userId)
  }

  public exitImpersonation(): Observable<UserTokenResponse> {
    return this.http.post<UserTokenResponse>(this.baseUrl + `ExitImpersonation`, null)
  }

  // Login
  public login(loginDto: LoginDto): Observable<UserTokenResponse> {
    return this.http.post<UserTokenResponse>(this.baseUrl + `Login`, loginDto)
  }

  public getProfile(): Observable<ProfileData> {
    return this.http.get<ProfileData>(this.baseUrl + `MyProfile`)
  }
}
