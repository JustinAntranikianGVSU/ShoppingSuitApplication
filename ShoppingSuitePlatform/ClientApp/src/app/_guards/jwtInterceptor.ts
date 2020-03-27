import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConstants } from '../_shared/appConstants';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const userToken = localStorage.getItem(AppConstants.UserToken);

    if (userToken) {
      const bearer = { Authorization: `Bearer ${userToken}` }
      request = request.clone({ setHeaders: bearer });
    }

    return next.handle(request);
  }
}