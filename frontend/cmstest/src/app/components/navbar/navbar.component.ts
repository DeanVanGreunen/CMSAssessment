import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { API } from './../../services/api';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  isLoggedIn:any = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private api: API) {}

  ngOnInit(): void {
    let sessionKey = window.localStorage.getItem('sessionKey');
    this.isLoggedIn = sessionKey != "";
  }

  logout(){
    let sessionKey = window.localStorage.getItem('sessionKey');
    window.localStorage.setItem('sessionKey', "");
    let result = this.api.logout(sessionKey);
    location.reload(); // reload page
  }
  
  gotoLogin(){
    this.router.navigate(['/login']);
  }

  gototHome(){
    this.router.navigate(['/home']);
  }
  gotoAdd(){
    this.router.navigate(['/add']);
  }
}
