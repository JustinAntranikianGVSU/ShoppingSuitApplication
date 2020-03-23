import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationsService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public getAllForClient(): Observable<any> {
    return this.http.get<any[]>(this.baseUrl + `Locations`)
  }

  public getUsersByLocationId(locationId: number): Observable<any> {
    return this.http.get<any[]>(this.baseUrl + `Locations/${locationId}`)
  }

  public getAccessLists(): Observable<any> {
    return this.http.get<any[]>(this.baseUrl + `AccessList`)
  }

  public getAccessList(accessListId: number): Observable<any> {
    return this.http.get<any[]>(this.baseUrl + `AccessList/${accessListId}`)
  }
}
