import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { API } from './../../services/api';
import Stock from './../../models/Stock';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  page_number:number = 1;
  total_pages:any = 1;
  stocks:any = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private api: API) { }

  ngOnInit(): void {
    this.page_number = +this.route.snapshot.queryParamMap.get('page') | 1;
    // query api and get all stock listings
    this.total_pages = +this.api.total_stock() / 10 + 1;
    let result = this.api.list_stock(this.page_number);
    this.stocks = result;
  }
}
