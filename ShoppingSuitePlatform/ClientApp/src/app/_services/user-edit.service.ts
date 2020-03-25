import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserEditService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + `User`)
  }

  public getUser(userId: number): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + `User/${userId}`)
  }

  public updateUser(id: number, userData: any): Observable<any> {
    return this.http.put<any[]>(this.baseUrl + `User/${id}`, userData)
  }
}
