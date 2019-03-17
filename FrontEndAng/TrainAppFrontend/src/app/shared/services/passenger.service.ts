import { Injectable } from '@angular/core';
import { Passenger } from '../models/Passenger';
import { HandleError, HttpErrorHandler } from './http-error-handler.service';
import { Observable, BehaviorSubject } from 'rxjs';
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
  private navbarMsg = new BehaviorSubject(null);
  navbarUsername = this.navbarMsg.asObservable();

  constructor(private http: HttpClient, httpErrorHandler: HttpErrorHandler) { 
    this.handleError = httpErrorHandler.createHandleError('PassengerService'); 
  }

  addPassenger(user: Passenger): Observable<Passenger>{    
    var result = this.http.post<Passenger>(this.url+"passenger", user, httpOptions)
    .pipe(catchError(this.handleError('addPassenger', user))
    );
    this.updateNavbarName(user.Email);
    return result;
  }
  
  updateNavbarName(fname: string) { 
    this.navbarMsg.next(fname)
  }


  login(user: Passenger): Observable<Passenger>{  //FAKE Authentication -- double check
   var result = this.http.get<Passenger>(this.url+"passenger?email="+user.Email, httpOptions).pipe(catchError(this.handleError('login', user)));
   this.updateNavbarName(user.Email)
   return result;
  }
}
