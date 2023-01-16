import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-toast-message',
  templateUrl: './toast-message.component.html'
})
export class ToastMessageComponent implements OnInit {

  @Input() type!: "success" | "info" | "error";
  @Input() message!: string;
  @Output() onClose: EventEmitter<void> = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void {
  }

}
