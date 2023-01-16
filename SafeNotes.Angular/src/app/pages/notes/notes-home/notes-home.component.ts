import { Component, OnDestroy, OnInit } from '@angular/core';
import { catchError, of, Subscription, switchMap } from 'rxjs';
import { CreateNoteRequest, NotesService } from 'src/app/api';
import { GetNoteListResponse } from 'src/app/api/model/getNoteListResponse';
import { GetNoteResponse } from 'src/app/api/model/getNoteResponse';
import { NotePreview } from 'src/app/api/model/notePreview';
import { AuthenticationService } from 'src/app/core/authentication/authentication.service';
import { MessageService } from 'src/app/shared/toast/services/message.service';

@Component({
  selector: 'app-notes-home',
  templateUrl: './notes-home.component.html'
})
export class NotesHomeComponent implements OnInit, OnDestroy {
  notePreviewList: GetNoteListResponse = {};
  selectedNote: NotePreview | null = null;
  displayCreateNewNoteModal: boolean = false;
  displayAccessModal: boolean = false;
  displayEnterNoteKeyModal: boolean = false;
  encryptKey: string = '';
  subscriptions: Subscription[] = [];

  noteContent: GetNoteResponse | null = {};

  createForm: CreateNoteRequest = {
    title: '',
    content: '',
    isPublic: false,
    isEncryptedWithUserSpecifiedKey: false,
    key: ''
  };

  constructor(
    private notesService: NotesService,
    private messageService: MessageService,
    public authenticationService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.updateNotesPreviewList();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  handleNoteHasChanged(noteForm: CreateNoteRequest) {
    this.createForm.title = noteForm.title;
    this.createForm.content = noteForm.content;
  }

  handleNoteSelected(selectedNote: NotePreview) {
    if(selectedNote.id != this.selectedNote?.id) {
      this.encryptKey = '';
    }
    this.noteContent = null;
    this.selectedNote = selectedNote;
    if(selectedNote.isEncryptedWithUserSpecifiedKey && this.encryptKey == '') {
      this.displayEnterNoteKeyModal = true;
      return;
    }
    this.subscriptions.push(this.$getSelectedNote(this.encryptKey).subscribe());
  }

  updateNotesPreviewList() {
    this.subscriptions.push(this.$updateNotesPreviewList.subscribe());
  }

  handleUpdateNoteClicked() {
    const $updateNote = this.notesService.apiNotesIdPut(
      this.selectedNote?.id!,
      {
        title: this.createForm.title,
        content: this.createForm.content,
        key: this.encryptKey
      }).pipe(
      switchMap(() => of(
        this.$updateNotesPreviewList.subscribe(() => {
          this.handleNoteSelected(this.selectedNote!);
        })
      )),
      catchError(err => of(this.messageService.showError(err.error.message)))
    )
    this.subscriptions.push($updateNote.subscribe());
  }

  handleCreateNewNoteClicked() {
    this.displayCreateNewNoteModal = true;
  }

  handleManageAccessClicked() {
    this.displayAccessModal = true;
  }

  saveNote() {
    const $saveNote = this.notesService.apiNotesPost(this.createForm).pipe(
      switchMap(idModel => of(
        this.$updateNotesPreviewList.subscribe(() => {
          const selectedNote = this.notePreviewList.userNotes?.find(x => x.id == idModel.id) ?? null;
          this.handleNoteSelected(selectedNote!);
        })
      )),
      catchError(err => of(this.messageService.showError(err.error.message)))
    )
    this.subscriptions.push($saveNote.subscribe(() => this.displayCreateNewNoteModal = false));
  }

  private $updateNotesPreviewList = this.notesService.apiNotesListGet().pipe(
    switchMap((x) => of(this.notePreviewList = x)),
    catchError(err => of(this.messageService.showError(err.message)))
  );

  showEncryptedNote() {
    this.subscriptions.push(this.$getSelectedNote(this.encryptKey).subscribe());
  }

  private $getSelectedNote(key?: string) {
    return this.notesService.apiNotesGetIdPut(this.selectedNote?.id!, { key: key ? key : null}).pipe(
      switchMap(note => of(this.noteContent = note, this.displayEnterNoteKeyModal = false)),
      catchError(err => of(this.messageService.showError(err.message)))
    )
  }
}
