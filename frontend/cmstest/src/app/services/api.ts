import { Injectable } from '@angular/core';
import { SyncRequestClient } from 'ts-sync-request';
import { HttpClient } from '@angular/common/http';

import Stock  from './../models/Stock';

@Injectable({
    providedIn: 'root'
  })
export class API {
    private base_api:string = "http://localhost:51019/";
    ////////////////////////////////////////////////////////////
    // AUTH APIs
    ////////////////////////////////////////////////////////////
    login(email:string, password:any) {
      let data = {
          email: email,
          password: password
      };    
      try {    
          let url =  this.base_api + "api/user/login";
          var response = new SyncRequestClient().post(url, data);  
          return response;
        } catch (error) {
          return false;
        }
  }
    register(email:string, password:any, confirmpassword:any) {
        let data = {
            email: email,
            password: password,
            confirmpassword: confirmpassword,
        };    
        try {    
            let url =  this.base_api + "api/user/register";
            var response = new SyncRequestClient().post(url, data);  
            return response;
          } catch (error) {
            return false;
          }
    }
    logout(session_key:string) {
        let data = {
            session_key: session_key
        };    
        try {    
            let url =  this.base_api + "api/user/logout";
            var response = new SyncRequestClient().post(url, data);  
            return response;
          } catch (error) {
            return false;
          }
    }
    ////////////////////////////////////////////////////////////
    // STOCK APIs
    ////////////////////////////////////////////////////////////
    total_stock() {
      try {    
          let url =  this.base_api + "/api/stock/total";
          var response = new SyncRequestClient().get(url);  
          return response;
        } catch (error) {
          return false;
        }
  }
  list_stock(page_number:number) {
    try {    
        let url =  this.base_api + "api/stock/list";
        let data = {
          page: page_number
        }
        var response = new SyncRequestClient().post(url, data);  
        return response;
      } catch (error) {
        return false;
      }
}
  get_stock(stock_id:number) {
    let data = {
        id: stock_id,
    };    
    try {    
        let url =  this.base_api + "api/stock/get";
        var response = new SyncRequestClient().post(url, data);  
        return response;
      } catch (error) {
        return false;
      }
  }
  add_stock(stock:Stock, session_key: string){
    let data = {
      stock: stock,
      session_key: session_key
    };    
    try {    
      let url =  this.base_api + "api/stock/add";
      var response = new SyncRequestClient().post(url, data);  
      return response;
    } catch (error) {
      return false;
    }
  } 
  update_stock(stock:Stock, session_key: string){
    let data = {
      stock: stock,
      session_key: session_key
    };    
    try {    
      let url =  this.base_api + "api/stock/update";
      var response = new SyncRequestClient().post(url, data);  
      return response;
    } catch (error) {
      return false;
    }
  } 
  
  delete_stock(stock_id:number, session_key:string) {
    let data = {
        id: stock_id,
        session_key: session_key
    };    
    try {    
        let url =  this.base_api + "api/stock/delete";
        var response = new SyncRequestClient().post(url, data);  
        return response;
      } catch (error) {
        return false;
      }
}
}