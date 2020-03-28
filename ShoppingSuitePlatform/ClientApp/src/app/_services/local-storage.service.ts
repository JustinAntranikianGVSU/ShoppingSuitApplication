import { Injectable } from '@angular/core';
import { AppConstants } from '../_shared/appConstants';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  public hasToken = (): boolean => this.getToken() != null
  
  public getToken = () => localStorage.getItem(AppConstants.UserToken)
  
  public setToken = (token: string) => localStorage.setItem(AppConstants.UserToken, token)
  
  public removeToken = () => localStorage.removeItem(AppConstants.UserToken)  
}
