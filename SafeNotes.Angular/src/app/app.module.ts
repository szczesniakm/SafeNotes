import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ApiModule, Configuration, ConfigurationParameters } from './api';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { environment } from '../environments/environment';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { ViewNotesComponent } from './pages/notes/view-notes/view-notes.component';
import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { NotesListComponent } from './pages/notes/notes-list/notes-list.component';
import { NotesHomeComponent } from './pages/notes/notes-home/notes-home.component';
import { ModalComponent } from './shared/modal/modal.component';
import { ToastComponent } from './shared/toast/toast.component';
import { ToastMessageComponent } from './shared/toast/components/toast-message/toast-message.component';
import { RegisterComponent } from './pages/auth/register/register.component';
import { ActivateAccountComponent } from './pages/auth/activate-account/activate-account.component';
import { UserAccessModalComponent } from './pages/notes/notes-home/components/user-access-modal/user-access-modal.component';

export function apiConfigFactory(): Configuration {
  const params: ConfigurationParameters = {
    basePath: environment.BASE_API_URL
  };
  return new Configuration(params);
}

export function tokenGetter() {
  return sessionStorage.getItem('jwtToken');
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ViewNotesComponent,
    NotesListComponent,
    NotesHomeComponent,
    ModalComponent,
    ToastComponent,
    ToastMessageComponent,
    RegisterComponent,
    ActivateAccountComponent,
    UserAccessModalComponent
  ],
  imports: [
    ApiModule.forRoot(apiConfigFactory),
    BrowserModule,
    AppRoutingModule,
    ApiModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: [environment.BASE_API_DOMAIN],

      },
    }),
    FormsModule,
    ReactiveFormsModule,
    EditorModule
  ],
  providers: [
    { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
