export * from './auth.service';
import { AuthService } from './auth.service';
export * from './notes.service';
import { NotesService } from './notes.service';
export * from './users.service';
import { UsersService } from './users.service';
export const APIS = [AuthService, NotesService, UsersService];
