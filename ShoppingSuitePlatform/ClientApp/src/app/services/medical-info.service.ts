import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MedicalInfoService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }
  
  public GetOccupations(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'GetOccupations')
  }

  public GetOccupationSectors(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'OccupationSectors')
  }

  public SaveUserInfo(data: any): Observable<any> {
    return this.http.post<any>(this.baseUrl + 'UserInfo', data)
  }

}
