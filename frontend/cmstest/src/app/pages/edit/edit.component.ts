import { Component, OnInit, Output, Input  } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import Stock from '../../models/Stock';
import { API } from './../../services/api';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit {
  stock:Stock;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private api: API) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.stock_id = +params['id']
      this.stock = this.api.get_stock(this.stock_id);
   });
  }

  addAccessory(){
    let name = (document.getElementById('newName') as any).value;
    let description = (document.getElementById('newDescription') as any).value;
    if(!name || !description){
      alert("enter valid data for an accessory");
      return;
    }
    this.stock.accessories.push({
      ID: 0,
      Name: name,
      Description: description,
      StockID: 0
    });
    this.reloadAccessory();
  }

  clearAccessory(){
    this.stock.accessories = [];
    this.reloadAccessory();
    (document.getElementById('newName') as any).value = "";
    (document.getElementById('newDescription') as any).value = "";
  }

  reloadAccessory(){
    document.getElementById("accessory-container").innerHTML = "";
    for(var i=0; i<this.stock.accessories.length; i++){
      let node = document.createElement('div');
      if(this.stock.accessories[i].Name){
      node.innerHTML = "<span style=\"color:#FFF;font-weight: bold;\">" + this.stock.accessories[i].Name + ":</span>&nbsp;<span style=\"color:#FFF;\">" + this.stock.accessories[i].Description + "</span>";
      } else {
        node.innerHTML = "<span style=\"color:#FFF;font-weight: bold;\">" + this.stock.accessories[i].name + ":</span>&nbsp;<span style=\"color:#FFF;\">" + this.stock.accessories[i].description + "</span>";
      }
      document.getElementById("accessory-container").appendChild(node);
    }
  }

  async update(){
    this.stock.RegNumber = (document.getElementById("RegNumber") as any).value;
    this.stock.Make = (document.getElementById("Make") as any).value;
    this.stock.Model = (document.getElementById("Model") as any).value;
    this.stock.ModelYear = (document.getElementById("ModelYear") as any).value;
    this.stock.KMS = (document.getElementById("KMS") as any).value;
    this.stock.Colour = (document.getElementById("Colour") as any).value;
    this.stock.VIN = (document.getElementById("VIN") as any).value;
    this.stock.RetailPrice = Math.floor((document.getElementById("RetailPrice") as any).value);
    this.stock.CostPrice = Math.floor((document.getElementById("CostPrice") as any).value);
    // get images
    this.stock.Images = [];
    let img1 = (document.getElementById('image-1') as any);
    let img2 = (document.getElementById('image-2') as any);
    let img3 = (document.getElementById('image-3') as any);
    let img1src = (document.getElementById('image-1src') as any);
    let img2src = (document.getElementById('image-2src') as any);
    let img3src = (document.getElementById('image-3src') as any);
    if(img1.files[0]){
      let filename1 = img1.files[0]['name'];
      let data1:any = await this.blobToData(img1.files[0]);
      this.stock.Images.push({
        Name: filename1,
        Data: data1,
        ID: 0,
        StockID: 0,
      });
    } else {
      this.stock.Images.push({
        Name: img1src.getAttribute('data-name'),
        Data: img1src.src,
        ID: 0,
        StockID: 0,
      });
    }
    if(img2.files[0]){
      let filename2 = img2.files[0]['name'];
      let data2:any = await this.blobToData(img2.files[0]);
      this.stock.Images.push({
        Name: filename2,
        Data: data2,
        ID: 0,
        StockID: 0,
      });
    } else {
      this.stock.Images.push({
        Name: img2src.getAttribute('data-name'),
        Data: img2src.src,
        ID: 0,
        StockID: 0,
      });
    }
    if(img3.files[0]){
      let filename3 = img3.files[0]['name'];
      let data3:any = await this.blobToData(img3.files[0]);
      this.stock.Images.push({
        Name: filename3,
        Data: data3,
        ID: 0,
        StockID: 0,
      });
    } else {
      this.stock.Images.push({
        Name: img3src.getAttribute('data-name'),
        Data: img3src.src,
        ID: 0,
        StockID: 0,
      });
    }

    let session_key = window.localStorage.getItem('sessionKey');
    let result = this.api.update_stock(this.stock, session_key);
    if(result == false || result['success'] == false){
      // TODO: display error
    } else {
      this.router.navigate(['/home']);
    }
  }

  blobToData = (blob: Blob) => {
    return new Promise((resolve) => {
      const reader = new FileReader()
      reader.onloadend = () => resolve(reader.result)
      reader.readAsDataURL(blob)
    })
  }
}
