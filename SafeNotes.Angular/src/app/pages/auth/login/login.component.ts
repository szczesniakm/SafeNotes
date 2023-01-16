import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { off } from 'process';
import { catchError, of, pipe, switchMap, take } from 'rxjs';
import { AuthService } from 'src/app/api';
import { MessageService } from 'src/app/shared/toast/services/message.service';
import { AuthenticationService } from '../../../core/authentication/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  constructor(
    private authApiService: AuthService,
    private authenticationService: AuthenticationService,
    private messageService: MessageService,
    private router: Router) { }

  ngOnInit(): void {
    this.authenticationService
      .isAuthenticatedObservable()
      .pipe(take(1))
      .subscribe(authenticated => {
        if(authenticated) {
          this.router.navigateByUrl('');
        }
      });
  }

  get email(): any {
    return this.loginForm.get("email");
  }

  get password(): any {
    return this.loginForm.get('password');
  }

  public login(): void {
    if(!this.loginForm.valid) {
      return;
    }
    this.authApiService.apiAuthPost({
        email: this.email.value,
        password: this.password.value
      }).pipe(
      switchMap(token => of(this.authenticationService.setToken(token))),
      catchError(err => of(this.messageService.showError(err.error.message)))
    ).subscribe(() => this.router.navigateByUrl(''));
  }
}
