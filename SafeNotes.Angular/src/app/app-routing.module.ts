import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/authentication/auth.guard';
import { ActivateAccountComponent } from './pages/auth/activate-account/activate-account.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { RegisterComponent } from './pages/auth/register/register.component';
import { NotesHomeComponent } from './pages/notes/notes-home/notes-home.component';

const routes: Routes = [
  { path: '', component: NotesHomeComponent, canActivate: [AuthGuard] },
  { path: 'confirm-email', component: ActivateAccountComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule { }
