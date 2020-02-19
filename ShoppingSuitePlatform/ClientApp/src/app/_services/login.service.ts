import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public post(body: any): Observable<any> {
    return this.http.post<any[]>(this.baseUrl + `Login`, body)
  }
}
