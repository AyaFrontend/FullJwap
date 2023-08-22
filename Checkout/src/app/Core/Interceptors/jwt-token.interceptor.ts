import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtTokenInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = localStorage.getItem('jwt-token');
    request = request.clone({
    //url : request.url.replace('http://' , 'https://'),
     setHeaders:
     {
        Authorization: `Bearer ${token}`
     }
     
    })
   
    return next.handle(request);
  }
}
