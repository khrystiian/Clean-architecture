import { Injectable } from '@angular/core';
import { Passenger } from '../models/Passenger';
import { HandleError, HttpErrorHandler } from './http-error-handler.service';
import { Observable, Subject } from 'rxjs';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
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
  url = "http://localhost:56287/";  // URL to web api
  private handleError: HandleError;
  public navBarUsername = new Subject<any>(); //here is stored the notification object

  constructor(private http: HttpClient, httpErrorHandler: HttpErrorHandler) { 
    this.handleError = httpErrorHandler.createHandleError('PassengerService'); 
  }

  postData(data): any {
    const body = new HttpParams()
      .set('grant_type', data.grant_type)
      .set('username', data.username)
      .set('password', data.password)
    return this.http.post(this.url+'token', body.toString(), {
      observe: 'response',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    }).pipe(catchError(this.handleError('login', data)));
  }  



  addPassenger(user: Passenger): Observable<Passenger>{
    this.navBarUsername.next(user.Email);
    var result = this.http.post<Passenger>(this.url+"api/passenger", user, httpOptions)
    .pipe(catchError(this.handleError('addPassenger', user))
    );
    return result;
  }
  
  login(email, token): Observable<Passenger>{
    this.navBarUsername.next(email);

    //New httpHeaders necessary for authentication
    const httpOptions2 = {     
    headers: new HttpHeaders(
      {
        'Authorization': "bearer " + token
      })
    };
    return this.http.get<Passenger>(this.url + "api/passenger?email=" + email, httpOptions2).pipe(catchError(this.handleError('login', email)));
  }
}
