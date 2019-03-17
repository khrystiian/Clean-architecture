import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { HandleError, HttpErrorHandler } from './http-error-handler.service';
import { catchError, debounceTime } from 'rxjs/operators';
import { RootObject } from '../models/TripRootObject';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    'Authorization': 'my-auth-token'
  })
};
//AIzaSyCq7O84-3Q3EfCSwVSM0HYbGpkceQWCXIU -google API

@Injectable({
  providedIn: 'root'
})
export class TripService {
  url = "http://localhost:56287/api/";  // URL to web api
  private handleError: HandleError;


  constructor(private http: HttpClient, httpErrorHandler: HttpErrorHandler) { this.handleError = httpErrorHandler.createHandleError('TripService'); }

  
  getCalendar(): Observable<any[]> { return this.http.get<any[]>('http://www.mocky.io/v2/586c2fb1110000f51c2e0ea7');}
 
  calculatePrice(trip: RootObject): Observable<RootObject>{ return this.http.post<RootObject>(this.url+"trip", trip, httpOptions).pipe(catchError(this.handleError('addTrip', trip))) }

  confirmPayment(confirm : boolean) {
    this.http.put<boolean>(this.url+"trip?confirm=", confirm, httpOptions).pipe(catchError(this.handleError('confirmPayment', confirm)))
  }

}
