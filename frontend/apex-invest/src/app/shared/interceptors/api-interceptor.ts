import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';

export const apiInterceptor: HttpInterceptorFn = (request, next) => {
  const apiReq = request.url.startsWith('http') 
    ? request 
    : request.clone({ url: `${environment.apiUrl}${request.url}` });

  // const token = localStorage.getItem('token');
  // const finalizedReq = token 
  //   ? apiReq.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
  //   : apiReq;

  return next(apiReq);
};