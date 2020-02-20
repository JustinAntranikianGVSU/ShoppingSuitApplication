import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      const bearer = { Authorization: `Bearer ${currentUser.token}` }
      request = request.clone({ setHeaders: bearer });
    }

    console.log('IN CUSTOM Intercept !!')
    return next.handle(request);
  }
}