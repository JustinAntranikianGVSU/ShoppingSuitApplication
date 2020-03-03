import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationsService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public get(): Observable<any> {
    return this.http.get<any[]>(this.baseUrl + `LocationsByUser`)
  }
}
