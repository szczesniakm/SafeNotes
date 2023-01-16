import { ChangeDetectorRef, Component, ComponentFactoryResolver, ComponentRef, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { Observable } from 'rxjs';
import { ToastMessageComponent } from './components/toast-message/toast-message.component';
import { ToastMessage } from './models/toast-message.model';
import { MessageService } from './services/message.service';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html'
})
export class ToastComponent implements OnInit{
  messages: ToastMessage[] = [];

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.messageService.getMessagesObservable().subscribe(message => this.messages.push(message));
  }

  handleOnClose(id: number) {
    this.messages.splice(id, 1);
  }
}
