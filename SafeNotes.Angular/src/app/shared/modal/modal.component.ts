import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html'
})
export class ModalComponent implements OnInit {
  @Input() title: string = '';
  @Output() onClose = new EventEmitter<void>();
  constructor() { }

  ngOnInit(): void {
  }

}
