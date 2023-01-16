import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable } from 'rxjs';
import { JwtTokenResponse } from 'src/app/api';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private isAuthenticated = new BehaviorSubject<boolean>(false);

  constructor(
    private jwtHelper: JwtHelperService,
    private router: Router
  ) {
    this.updateStatus();
  }

  public setToken(token: JwtTokenResponse) : void {
    sessionStorage.setItem('jwtToken', token.token!);
    this.updateStatus();
  }

  public getToken() : string | null {
    return sessionStorage.getItem('jwtToken');
  }

  logout(): void {
    sessionStorage.removeItem('jwtToken');
    this.isAuthenticated.next(false);
    this.router.navigateByUrl('/login');
  }

  updateStatus(): void {
    const token = sessionStorage.getItem('jwtToken');
    if(this.jwtHelper.isTokenExpired(token)) {
      this.isAuthenticated.next(false);
      return;
    }
    this.isAuthenticated.next(true);
  }

  isAuthenticatedObservable(): Observable<boolean> {
    this.updateStatus();
    return this.isAuthenticated.asObservable();
  }
}
