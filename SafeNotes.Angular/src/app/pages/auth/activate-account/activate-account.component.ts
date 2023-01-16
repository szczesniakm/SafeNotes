import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { off } from 'process';
import { catchError, of, switchMap } from 'rxjs';
import { UsersService } from 'src/app/api';
import { MessageService } from 'src/app/shared/toast/services/message.service';

@Component({
  selector: 'app-activate-account',
  templateUrl: './activate-account.component.html'
})
export class ActivateAccountComponent implements OnInit {

  constructor(
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private usersService: UsersService,
    private router: Router
    ) {}

  ngOnInit() {
    // Note: Below 'queryParams' can be replaced with 'params' depending on your requirements
    this.activatedRoute.queryParams.subscribe(params => {
        const emailConfirmationCode: string = params['c'];
        console.log(emailConfirmationCode);
        if(emailConfirmationCode) {
          this.usersService.apiUsersConfirmEmailEmailConfirmationCodeGet(emailConfirmationCode).pipe(
            switchMap(() => of(
              this.messageService.showSuccess('Your email confirmed successfuly.'),
              this.router.navigateByUrl('login'))),
            catchError(err => of(this.messageService.showError(err.error.message)))
          ).subscribe();
        }
      });
  }

}
