import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { APIResponse } from 'src/app/models/ApiResponse'; 
import { Account } from 'src/app/models/Account'; 

declare var $: any;

@Component({
  selector: 'app-account-show',
  templateUrl: './account-show.component.html',
  styleUrls: ['./account-show.component.css']
})
export class AccountShowComponent implements OnInit {

  constructor(
    private service:SharedService
  ) { }

  AccountList:Account[];
  account:Account;
  ModalTitle:string;
  activateAddEditAccount:boolean;

  ngOnInit() {
    const component = this;

    component.refreshAccountList();

    $('#accountModal').on('hidden.bs.modal', function () {
      component.activateAddEditAccount = false;
      component.account = new Account();
      component.refreshAccountList();
    });
  }

  refreshAccountList() {
    this.service.getAccountList().subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<Account[]>(response.body);

        this.AccountList = apiResponse.Content;
      } else {
        alert('Error! Cannot get account list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  addAccount() {
    $('#accountModal').find('input[type="text"]').val('');
    $('#accountModal').find('select').val('');

    this.account = new Account({
      AccountID: '0',
      AccountTypeID: '',
      AccountName: ''
    });
    this.ModalTitle = 'Add Account';
    this.activateAddEditAccount = true;
  }

  editAccount(account:Account) {
    this.account = account;
    this.ModalTitle = 'Edit Account';
    this.activateAddEditAccount = true;
  }

  deleteAccount(account:Account) {
    $('#DivOverlay').show();

    if(confirm('Are you sure want to delete?')) {
      this.service.deleteAccount(account.AccountID).subscribe(response => {
        var apiResponse = new APIResponse<Account>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.account = new Account();
  
          alert("Successful deleted new Account.");

          $('#DivOverlay').hide();

          this.refreshAccountList();
        }
      }, error => {
        var apiResponse = new APIResponse<boolean>(error.error);

        if(apiResponse) {
          alert(apiResponse.StatusRemark);
        } else {
          alert(error.message);
        }

        $('#DivOverlay').hide();
      })
    } else {
      $('#DivOverlay').hide();
    }
  }

  closeModal() {
    this.activateAddEditAccount = false;
    this.account = new Account();
  }
}
