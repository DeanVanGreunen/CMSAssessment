import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


// LOAD PAGE COMPONENTS
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { PreviewComponent } from './pages/preview/preview.component';
import { AddComponent } from './pages/add/add.component';
import { EditComponent } from './pages/edit/edit.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' }, // Redirect To Home Page
  { path: 'logout', redirectTo: 'home', pathMatch: 'full' }, // Redirect To Home Page
  { path: 'home', component: HomeComponent},
  { path: 'login', component: LoginComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'preview/:id', component: PreviewComponent},
  { path: 'edit/:id', component: EditComponent},
  { path: 'add', component: AddComponent},
  { path: '**', redirectTo: 'home', pathMatch: 'full' }, // Redirect To Home Page
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
