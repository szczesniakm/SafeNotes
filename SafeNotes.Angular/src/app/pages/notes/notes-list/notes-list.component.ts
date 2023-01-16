import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { GetNoteListResponse } from 'src/app/api/model/getNoteListResponse';
import { NotePreview } from 'src/app/api/model/notePreview';

@Component({
  selector: 'app-notes-list',
  templateUrl: './notes-list.component.html'
})
export class NotesListComponent {
  @Input()
  notes: GetNoteListResponse = {};
  @Output()
  noteSelected = new EventEmitter<NotePreview>();
}
