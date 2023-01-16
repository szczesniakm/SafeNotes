import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CreateNoteRequest, NotesService } from 'src/app/api';
import { Output, EventEmitter } from '@angular/core';
import { NotePreview } from 'src/app/api/model/notePreview';
import { Subscription } from 'rxjs';
import { GetNoteResponse } from 'src/app/api/model/getNoteResponse';

@Component({
  selector: 'app-view-notes',
  templateUrl: './view-notes.component.html'
})
export class ViewNotesComponent implements OnInit, OnDestroy, OnChanges  {
  @Input()
  note: GetNoteResponse | null = null;
  @Input()
  isNewNote: boolean = true;
  @Output()
  noteHasChanged = new EventEmitter<CreateNoteRequest>();

  private subscriptions: Subscription[] = [];

  form = new FormGroup({
    title: new FormControl('', [Validators.required]),
    content: new FormControl('', [Validators.required])
  });

  ngOnInit(): void {
    if(!this.isNewNote) {
      this.updateForm();
    }

    this.subscriptions.push(this.form.valueChanges.subscribe(x => this.noteHasChanged.emit(x as CreateNoteRequest)));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.note = changes['note'].currentValue;
    this.updateForm();
  }

  updateForm() {
    if(this.note) {
      this.form.get('title')?.setValue(this.note.title!);
      this.form.get('content')?.setValue(this.note.content!);
    }
  }
}
