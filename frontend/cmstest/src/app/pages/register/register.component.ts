import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { API } from './../../services/api';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private api: API) {}

  ngOnInit(): void {
  }

  tryRegister(){
    (document.getElementById("error") as any).innerHTML  = "";
    let email = (document.getElementById('email') as any).value;
    let password = (document.getElementById('password') as any).value;
    let confirmpassword = (document.getElementById('confirmpassword') as any).value;
    
    let result = this.api.register(email, password, confirmpassword);
    if(result === false || result['success'] == false){
      (document.getElementById("error") as any).innerHTML  = "Error registering";
      return;
    } else {
      let sessionKey = result['sessionKey'];
      window.localStorage.setItem('sessionKey', sessionKey);
      this.router.navigate(['/home']);
    }
  }
}
