import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { API } from './../../services/api';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private api: API) {}

  ngOnInit(): void {
  }

  tryLogin(){
    (document.getElementById("error") as any).innerHTML  = "";
    let email = (document.getElementById('email') as any).value;
    let password = (document.getElementById('password') as any).value;
    
    let result = this.api.login(email, password);
    if(result === false || result['success'] == false){
      (document.getElementById("error") as any).innerHTML  = "Error logging in";
      return;
    } else {
      let sessionKey = result['sessionKey'];
      window.localStorage.setItem('sessionKey', sessionKey);
      this.router.navigate(['/home']);
    }
  }
}
