import { Component, OnInit, Output, Input  } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { API } from './../../services/api';

@Component({
  selector: 'app-preview',
  templateUrl: './preview.component.html',
  styleUrls: ['./preview.component.css']
})
export class PreviewComponent implements OnInit {

  stock_id:number;
  stock:any;
  isLoggedIn:any = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private api: API) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.stock_id = +params['id']
      this.stock = this.api.get_stock(this.stock_id);
      let sessionKey = window.localStorage.getItem('sessionKey');
      if(sessionKey){
        this.isLoggedIn = true;
      } else {
        this.isLoggedIn = false;
      }
   });
  }

  gotoEdit(){
    this.router.navigateByUrl('/edit/' + this.stock.id);
  }

  delete(){
    let session_key = window.localStorage.getItem('sessionKey');
    let result = this.api.delete_stock(this.stock_id, session_key);
    this.router.navigateByUrl('/home');
  }

}
