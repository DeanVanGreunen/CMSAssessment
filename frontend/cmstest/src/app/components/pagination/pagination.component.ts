import { Component, OnInit, Output, Input  } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {

  @Input() page_number:number;
  @Input() total_pages:number;

  pages:any;

  constructor() { }

  range(start, end) {
    const isReverse = (start > end);
    const targetLength = isReverse ? (start - end) + 1 : (end - start ) + 1;
    const arr = new Array(targetLength);
    const b = Array.apply(null, arr);
    const result = b.map((discard, n) => {
      return (isReverse) ? n + end : n + start;
    });
  
    return (isReverse) ? result.reverse() : result;
  }
  
  insert(array, index) {
    const items = Array.prototype.slice.call(arguments, 2);
    return [].concat(array.slice(0, index), items, array.slice(index));
  }

  ngOnInit(): void {
    let maxPage = this.page_number + 10 >= this.total_pages ? this.total_pages : this.page_number + 10;
    if(this.page_number >= 2){
      this.pages = this.insert(this.range(this.page_number, maxPage), 0, 1);
    } else {
      this.pages = this.range(this.page_number, maxPage);
    }
  }

  gotoPage(page){
    location.href = "/home?page=" + page;
  }

}
