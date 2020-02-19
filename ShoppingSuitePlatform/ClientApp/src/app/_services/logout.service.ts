import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LogoutService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public post(): Observable<any> {
    return this.http.post<any[]>(this.baseUrl + `Logout`, {})
  }
}
