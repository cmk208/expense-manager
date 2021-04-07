import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from './models/Category';
import { Account } from './models/Account';
import { Transaction } from './models/Transaction';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  readonly APIUrl = 'http://localhost:51386/';

  constructor(private http:HttpClient) { }

  getCategoryList(transactionTypeID?:string):Observable<any>{
    let params = new HttpParams()
    .set("transactionTypeID", transactionTypeID);

    return this.http.get<any>(this.APIUrl + 'api/Category/GetCategories', {observe: 'response', params: params});
  }

  getTransactionTypeList():Observable<any> {
    return this.http.get<any>(this.APIUrl + 'api/TransactionType/GetTransactionTypes', {observe: 'response'});
  }

  addCategory(category:Category):Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 
        'Content-Type': 'application/json; charset=UTF-8'
      })
    };

    return this.http.post(this.APIUrl + 'api/Category/Create', category, httpOptions);
  }

  editCategory(category:Category):Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 
        'Content-Type': 'application/json; charset=UTF-8'
      })
    };

    return this.http.put(this.APIUrl + 'api/Category/Edit', category, httpOptions);
  }

  deleteCategory(categoryID:string):Observable<any> {
    return this.http.delete(this.APIUrl + 'api/Category/Delete/' + categoryID);
  }

  getAccountList():Observable<any>{
    return this.http.get<any>(this.APIUrl + 'api/Account/GetAccounts', {observe: 'response'});
  }

  getAccountTypeList():Observable<any> {
    return this.http.get<any>(this.APIUrl + 'api/AccountType/GetAccountTypes', {observe: 'response'});
  }

  addAccount(account:Account):Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 
        'Content-Type': 'application/json; charset=UTF-8'
      })
    };

    return this.http.post(this.APIUrl + 'api/Account/Create', account, httpOptions);
  }

  editAccount(account:Account):Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 
        'Content-Type': 'application/json; charset=UTF-8'
      })
    };

    return this.http.put(this.APIUrl + 'api/Account/Edit', account, httpOptions);
  }

  deleteAccount(accountID:string):Observable<any> {
    return this.http.delete(this.APIUrl + 'api/Account/Delete/' + accountID);
  }

  getTransactionListByDate(startDate:string, endDate:string, transactionTypeID:string, accountID:string, categoryID:string, min:string, max:string):Observable<any>{
    let params = new HttpParams()
    .set("startDate", startDate)
    .set("endDate", endDate)
    .set("transactionTypeID", transactionTypeID)
    .set("accountID", accountID)
    .set("categoryID", categoryID)
    .set("min", min)
    .set("max", max);

    return this.http.get<any>(this.APIUrl + 'api/Transaction/GetTransactionsByDate', {observe: 'response', params: params});
  }

  getTransactionListByMonth(year:string, month:string, transactionTypeID:string, accountID:string, categoryID:string, min:string, max:string):Observable<any>{
    let params = new HttpParams()
    .set("year", year)
    .set("month", month)
    .set("transactionTypeID", transactionTypeID)
    .set("accountID", accountID)
    .set("categoryID", categoryID)
    .set("min", min)
    .set("max", max);

    return this.http.get<any>(this.APIUrl + 'api/Transaction/GetTransactionsByMonth', {observe: 'response', params: params});
  }

  getTransactionListByYear(year:string, transactionTypeID:string, accountID:string, categoryID:string, min:string, max:string):Observable<any>{
    let params = new HttpParams()
    .set("year", year)
    .set("transactionTypeID", transactionTypeID)
    .set("accountID", accountID)
    .set("categoryID", categoryID)
    .set("min", min)
    .set("max", max);

    return this.http.get<any>(this.APIUrl + 'api/Transaction/GetTransactionsByYear', {observe: 'response', params: params});
  }

  addTransaction(transaction:Transaction):Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 
        'Content-Type': 'application/json; charset=UTF-8'
      })
    };

    return this.http.post(this.APIUrl + 'api/Transaction/Create', transaction, httpOptions);
  }

  editTransaction(transaction:Transaction):Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 
        'Content-Type': 'application/json; charset=UTF-8'
      })
    };

    return this.http.put(this.APIUrl + 'api/Transaction/Edit', transaction, httpOptions);
  }

  deleteTransaction(transactionID:string):Observable<any> {
    return this.http.delete(this.APIUrl + 'api/Transaction/Delete/' + transactionID);
  }

  getReportListByDate(startDate:string, endDate:string, transactionTypeID:string):Observable<any>{
    let params = new HttpParams()
    .set("startDate", startDate)
    .set("endDate", endDate)
    .set("transactionTypeID", transactionTypeID);

    return this.http.get<any>(this.APIUrl + 'api/Report/GetReportsByDate', {observe: 'response', params: params});
  }

  getReportListByMonth(year:string, month:string, transactionTypeID:string):Observable<any>{
    let params = new HttpParams()
    .set("year", year)
    .set("month", month)
    .set("transactionTypeID", transactionTypeID);

    return this.http.get<any>(this.APIUrl + 'api/Report/GetReportsByMonth', {observe: 'response', params: params});
  }

  getReportListByYear(year:string, transactionTypeID:string):Observable<any>{
    let params = new HttpParams()
    .set("year", year)
    .set("transactionTypeID", transactionTypeID);

    return this.http.get<any>(this.APIUrl + 'api/Report/GetReportsByYear', {observe: 'response', params: params});
  }
}
