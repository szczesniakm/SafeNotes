import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, of, switchMap, take } from 'rxjs';
import { AuthService, UsersService } from 'src/app/api';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
import { MessageService } from 'src/app/shared/toast/services/message.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  registerForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  constructor(
    private usersService: UsersService,
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
    return this.registerForm.get("email");
  }

  get password(): any {
    return this.registerForm.get('password');
  }

  public register(): void {
    if(!this.registerForm.valid) {
      return;
    }
    this.usersService.apiUsersPost({
        email: this.email.value,
        password: this.password.value
      }).pipe(
      switchMap(() => of(this.messageService.showSuccess("You have registered successfuly. Activate your account by clicking in the link we sent to your e-mail address."))),
      catchError(err => of(this.messageService.showError(err.error.message)))
    ).subscribe();
  }
}
