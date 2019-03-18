import { Injectable } from '@angular/core';
import { Passenger } from '../models/Passenger';
import { HandleError, HttpErrorHandler } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    'Authorization': 'my-auth-token'
  })
};

@Injectable({
  providedIn: 'root'
})
export class PassengerService {
  url = "http://localhost:56287/api/";  // URL to web api
  private handleError: HandleError;

  constructor(private http: HttpClient, httpErrorHandler: HttpErrorHandler) { 
    this.handleError = httpErrorHandler.createHandleError('PassengerService'); 
  }

  addPassenger(user: Passenger): Observable<Passenger>{    
    var result = this.http.post<Passenger>(this.url+"passenger", user, httpOptions)
    .pipe(catchError(this.handleError('addPassenger', user))
    );
    return result;
  }
  
  login(user: Passenger): Observable<Passenger>{ 
    var result = this.http.get<Passenger>(this.url + "passenger?email=" + user.Email, httpOptions).pipe(catchError(this.handleError('login', user)));
   return result;
  }
}
