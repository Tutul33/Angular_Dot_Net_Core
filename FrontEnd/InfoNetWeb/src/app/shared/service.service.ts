import { Inject, Injectable, Component } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse, HttpInterceptor, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subject, ReplaySubject, forkJoin } from 'rxjs';
import { Router } from '@angular/router';
import { map, catchError, tap } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';
@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  public apiHost:string="http://localhost:61081/api/";
  constructor(private _httpClient: HttpClient,
    private router: Router,
    @Inject(DOCUMENT) private document: any) { 
      
    }
    JsonStringify(models: any): string {
      var smodel = '';
      if (models.length !== undefined) {
          if (models.length > 1) {
              for (var i = 0; i < models.length; i++) {
                  if (i == 0) {
                      smodel += "[" + JSON.stringify(models[i]) + ",";
                  }
                  else if (i == (models.length - 1)) {
                      smodel += JSON.stringify(models[i]) + "]";
                  }
                  else {
                      smodel += JSON.stringify(models[i]) + ",";
                  }
              }
          }
          else {
              smodel = "[" + JSON.stringify(models[0]) + "]";
          }
      }
      else {
          smodel = "[" + JSON.stringify(models) + "]";
      }
      return smodel;
  }
  getWithMultipleModel(_apiRout: string, model: any): Observable<any[]> {
    
    let qString = this.JsonStringify(model);
    _apiRout = this.apiHost + _apiRout + '?param=' + qString;

    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this._httpClient.get(_apiRout, { headers: headers })
        .pipe(map((res: any) => {
            return res;
        }))
        .pipe(catchError((error:any[])=>{
                          let errorMsg: string;
                        if (error[0].error instanceof ErrorEvent) {
                            errorMsg = `Error: ${error[0].error.message}`;
                        } else {
                            errorMsg = this.getServerErrorMessage(error[0]);
                        }
          return error;
        }));

}
saveFormWithFormData(model: any, _saveUrl: string): Observable<any> {
  _saveUrl = this.apiHost + _saveUrl;    
  return this._httpClient.post(_saveUrl, model)
      .pipe(map((res: any) => {
          return res;
      }))
      .pipe(catchError((error:any[])=>{
        let errorMsg: string;
      if (error[0].error instanceof ErrorEvent) {
          errorMsg = `Error: ${error[0].error.message}`;
      } else {
          errorMsg = this.getServerErrorMessage(error[0]);
      }
return error;
}));
}
updateFormWithFormData(model: any, _saveUrl: string): Observable<any> {
  _saveUrl = this.apiHost + _saveUrl;    
  return this._httpClient.put(_saveUrl, model)
      .pipe(map((res: any) => {
          return res;
      }))
      .pipe(catchError((error:any[])=>{
        let errorMsg: string;
      if (error[0].error instanceof ErrorEvent) {
          errorMsg = `Error: ${error[0].error.message}`;
      } else {
          errorMsg = this.getServerErrorMessage(error[0]);
      }
return error;
}));
}
deleteData(model: any, _deleteUrl: string): Observable<any> { 
  let qString = this.JsonStringify(model);   
  _deleteUrl = this.apiHost + _deleteUrl+'?param='+qString;    
  return this._httpClient.delete(_deleteUrl)
      .pipe(map((res: any) => {
          return res;
      }))
      .pipe(catchError((error:any[])=>{
        let errorMsg: string;
      if (error[0].error instanceof ErrorEvent) {
          errorMsg = `Error: ${error[0].error.message}`;
      } else {
          errorMsg = this.getServerErrorMessage(error[0]);
      }
return error;
}));
}
private getServerErrorMessage(error: HttpErrorResponse): string {
  switch (error.status) {
      case 404: {
          return `Not Found: ${error.message}`;
      }
      case 403: {
          return `Access Denied: ${error.message}`;
      }
      case 500: {
          return `Internal Server Error: ${error.message}`;
      }
      default: {
          return `Unknown Server Error: ${error.message}`;
      }

  }
}

}
