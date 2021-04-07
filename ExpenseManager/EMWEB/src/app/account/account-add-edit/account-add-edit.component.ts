import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { APIResponse } from 'src/app/models/ApiResponse';
import { Account } from 'src/app/models/Account'; 
import { AccountType } from 'src/app/models/AccountType'; 

declare var $: any;

@Component({
  selector: 'app-account-add-edit',
  templateUrl: './account-add-edit.component.html',
  styleUrls: ['./account-add-edit.component.css']
})
export class AccountAddEditComponent implements OnInit {

  constructor(
    private service:SharedService
  ) { }

  @Input() account:Account;
  AccountTypeList:AccountType[];
  AccountID:string;
  AccountTypeID:string;
  AccountName:string;
  Sequence:number;
  submit:boolean = false;

  ngOnInit() {
    this.submit = false;

    this.refreshAccountTypeList(this.account.AccountTypeID);

    this.AccountID = this.account.AccountID;
    this.AccountName = this.account.AccountName;
  }

  refreshAccountTypeList(transationTypeID?:string) {
    this.service.getAccountTypeList().subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<AccountType[]>(response.body);

        this.AccountTypeList = apiResponse.Content;

        if(transationTypeID) {
          this.AccountTypeID = transationTypeID;
        }
      } else {
        alert('Error! Cannot get account type list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  addAccount() {
    $('#DivOverlay').show();

    this.account.AccountTypeID = this.AccountTypeID;
    this.account.AccountName = this.AccountName;
    this.submit = true;

    if((this.AccountTypeID != null && this.AccountTypeID != '') &&
       (this.AccountName != null && this.AccountName != '')) {
      this.service.addAccount(this.account).subscribe(response => {
        var apiResponse = new APIResponse<Account>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.account = new Account();
  
          alert("Successful added new Account.");
  
          $('#accountModal').modal('hide');
          $('#DivOverlay').hide();

          this.submit = false;
        }
      }, error => {
        alert(error.message);

        $('#DivOverlay').hide();

        this.submit = false;
      });
    } else {
      $('#DivOverlay').hide();
    }
  }

  editAccount() {
    $('#DivOverlay').show();

    this.account.AccountID = this.AccountID;
    this.account.AccountTypeID = this.AccountTypeID;
    this.account.AccountName = this.AccountName;
    this.submit = true;

    if((this.AccountID != null && this.AccountID != '') &&
       (this.AccountTypeID != null && this.AccountTypeID != '') &&
       (this.AccountName != null && this.AccountName != '')) {
      this.service.editAccount(this.account).subscribe(response => {
        var apiResponse = new APIResponse<Account>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.account = new Account();
  
          alert("Successful edited new Account.");
  
          $('#accountModal').modal('hide');
          $('#DivOverlay').hide();

          this.submit = false;
        }
      }, error => {
        alert(error.message);

        $('#DivOverlay').hide();

        this.submit = false;
      });
    } else {
      $('#DivOverlay').hide();
    }
  }
}
