import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';

import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { HomeComponent } from './pages/home/home.component';
import { PreviewComponent } from './pages/preview/preview.component';
import { EditComponent } from './pages/edit/edit.component';
import { StockComponent } from './components/stock/stock.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { AddComponent } from './pages/add/add.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    PreviewComponent,
    EditComponent,
    StockComponent,
    NavbarComponent,
    PaginationComponent,
    AddComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
