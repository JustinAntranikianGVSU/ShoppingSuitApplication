import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalStorageService } from '../_services/local-storage.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private readonly localStorageService: LocalStorageService) {}

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const userToken = this.localStorageService.getToken();

    if (userToken) {
      const bearer = { Authorization: `Bearer ${userToken}` }
      request = request.clone({ setHeaders: bearer });
    }

    return next.handle(request);
  }
}