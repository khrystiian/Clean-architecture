import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { HttpErrorHandler, HandleError } from './http-error-handler.service';
import { catchError, debounce } from 'rxjs/operators';

const httpOptions = {
  //headers: new HttpHeaders({
  //  'Content-Type': 'application/json',
  //  'Authorization': 'my-auth-token'
  //})
};

@Injectable({
  providedIn: 'root'
})
  /**
   * ENABLE CORS FOR ELASTICSEARCH.
   * MODIFY elasticsearch.yml
   */ 
export class ElasticsearchService { 
  url = "http://localhost:9200/";  // URL to web api
  private handleError: HandleError;

  constructor(private http: HttpClient, httpErrorHandler: HttpErrorHandler) {
    this.handleError = httpErrorHandler.createHandleError('ElasticsearchService');
  }

  search(): Observable<any> {
     return this.http.get<any>("http://localhost:9200/legs/trip/_search", httpOptions).pipe(catchError(this.handleError('search', null)));
  }
}
