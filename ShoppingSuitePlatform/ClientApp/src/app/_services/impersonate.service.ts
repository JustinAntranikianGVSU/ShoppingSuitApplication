import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImpersonateService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public post(userId: number): Observable<any> {
    return this.http.post<any[]>(this.baseUrl + `Impersonation`, userId)
  }

  public exit(): Observable<any> {
    return this.http.post<any[]>(this.baseUrl + `ExitImpersonation`, null)
  }
}
