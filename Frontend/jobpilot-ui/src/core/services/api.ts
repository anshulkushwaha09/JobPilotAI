import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class Api {

  private http = inject(HttpClient);

  get<T>(url: string): Observable<T> {

    return this.http.get<T>(
      `${environment.apiUrl}/${url}`
    );

  }

  post<T>(url: string, body: any): Observable<T> {

    return this.http.post<T>(
      `${environment.apiUrl}/${url}`,
      body
    );

  }

  put<T>(url: string, body: any): Observable<T> {

    return this.http.put<T>(
      `${environment.apiUrl}/${url}`,
      body
    );

  }

  delete<T>(url: string): Observable<T> {

    return this.http.delete<T>(
      `${environment.apiUrl}/${url}`
    );

  }

}