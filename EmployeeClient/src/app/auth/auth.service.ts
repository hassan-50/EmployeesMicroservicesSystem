import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, tap } from 'rxjs';
import { environment } from './../../environments/environment';
import { LoginRequest } from './login-request';
import { LoginResult } from './login-result';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
 providedIn: 'root',
})
export class AuthService {
 constructor(
 protected http: HttpClient) {
 }
 public tokenKey: string = "token";
 public roleKey: string = "role";
 public roles: string[] = [];

 private _authStatus = new Subject<boolean>();
 public authStatus = this._authStatus.asObservable();

 private _adminStatus = new Subject<boolean>();
 public adminStatus = this._adminStatus.asObservable();

 isAuthenticated() : boolean {
 return this.getToken() !== null;
 }

 isAdmin() : boolean {
  // var isAdmin = false
  // console.log(this.roles)
  // this.roles?.forEach((role:any)=>{
  //   if(role == "Administrator")
  //     isAdmin = true
  // })

  // return isAdmin;

  return this.getRole() === "Administrator";
  }

 getToken() : string | null {
  return localStorage.getItem(this.tokenKey);
  }

  getRole() : string | null {
    return localStorage.getItem(this.roleKey);
    }

  init() : void {
    if (this.isAuthenticated())
    this.setAuthStatus(true);

    if (this.isAdmin())
    this.setAdminStatus(true);

    }
  login(item: LoginRequest): Observable<LoginResult> {
    const helper = new JwtHelperService();
  var url = environment.firstGateway + "Account/Login";
  return this.http.post<LoginResult>(url, item)
 .pipe(tap(loginResult => {
 if (loginResult.success && loginResult.token) {
 localStorage.setItem(this.tokenKey, loginResult.token);
 if(typeof(helper.decodeToken(loginResult.token).Role) == 'string'){
   this.roles.push(helper.decodeToken(loginResult.token).Role as string)
   localStorage.setItem(this.roleKey, "RegisteredUser");
 }else {
  this.roles = (helper.decodeToken(loginResult.token).Role as Array<string>)
  localStorage.setItem(this.roleKey, "Administrator");
 }


 if(this.isAdmin())
 this.setAdminStatus(true);

 this.setAuthStatus(true);
 }
 }));
}

 logout() {
  localStorage.removeItem(this.tokenKey);
  this.setAdminStatus(false);
  this.setAuthStatus(false);
  }

 private setAuthStatus(isAuthenticated: boolean): void {
  this._authStatus.next(isAuthenticated);
  }

  private setAdminStatus(isAdmin: boolean): void {
    this._adminStatus.next(isAdmin);
    }
  }

