import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { catchError, of, switchMap } from 'rxjs';
import { NotesService, UpdateAllowedUsersRequest } from 'src/app/api';
import { AllowedUser } from 'src/app/api/model/allowedUser';
import { MessageService } from 'src/app/shared/toast/services/message.service';

@Component({
  selector: 'app-user-access-modal',
  templateUrl: './user-access-modal.component.html'
})
export class UserAccessModalComponent implements OnInit {
  @Input()
  noteId: number = 0;
  @Output()
  closeModal = new EventEmitter<void>();

  updateAllowedUsersForm: FormGroup = new FormGroup({
    allowedUsers: new FormArray([])
  });

  get allowedUsers() {
    return this.updateAllowedUsersForm.controls["allowedUsers"] as FormArray;
  }

  accesses: AllowedUser[] = [];

  constructor(
    private notesService: NotesService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.notesService.apiNotesIdAccessGet(this.noteId).pipe(
      switchMap(accesses => of(this.accesses = accesses.allowedUsers!)),
      catchError(err => of(this.messageService.showError(err.error.message)))
    ).subscribe(() => this.initForm());
    console.log(this.allowedUsers.controls);
  }

  updateAccesses() {
    const accesses: AllowedUser[] = [];
    this.allowedUsers.controls.forEach(x => {
      accesses.push({
        email: x.value.email,
        canRead: x.value.canRead,
        canWrite: x.value.canWrite
      })
    });

    this.notesService.apiNotesIdAccessPut(this.noteId, { allowedUsers: accesses }).pipe(
      switchMap(() => of(this.messageService.showSuccess('Successfuly updated accesses!'))),
      catchError(err => of(this.messageService.showError(err.error.message)))
    ).subscribe(() => this.closeModal.emit());
  }

  initForm() {
    this.accesses.forEach(x => {
      this.addAllowedUser(x.email!, x.canRead!, x.canWrite!);
    })
  }

  addAllowedUser(email?: string, canRead?: boolean, canWrite?: boolean) {
    const allowedUserForm = new FormGroup({
        email: new FormControl(email ? email : '' , Validators.required),
        canRead: new FormControl(canRead ? canRead : false , Validators.required),
        canWrite: new FormControl(canWrite ? canWrite : false, Validators.required),
    });

    this.allowedUsers.push(allowedUserForm);
  }

  deleteAllowedUser(userIndex: number) {
    this.allowedUsers.removeAt(userIndex);
  }
}
