import { Component, OnInit, Output, Input  } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import Stock from './../../models/Stock';

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})
export class StockComponent implements OnInit {
  constructor(private router: Router,private route: ActivatedRoute) { }

  @Input() stock:Stock;

  ngOnInit(): void {
  }

  gotoDetailsPage(){
    this.router.navigateByUrl('/preview/' + this.stock.id);
  }
}
