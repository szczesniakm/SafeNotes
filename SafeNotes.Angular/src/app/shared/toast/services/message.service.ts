import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { ToastMessage } from "../models/toast-message.model";

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  constructor() {}

  message$: Subject<ToastMessage> = new Subject();

  getMessagesObservable(): Observable<ToastMessage> {
    return this.message$;
  }

  showSuccess(message: string) {
    this.message$.next({ message, type: 'success' });
  }

  showError(message: string) {
    console.log(message);
    this.message$.next({ message, type: 'error' });
  }

  showInfo(message: string) {
    this.message$.next({ message, type: 'info' });
  }
 }
