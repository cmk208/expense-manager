import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { APIResponse } from 'src/app/models/ApiResponse';
import { Transaction } from 'src/app/models/Transaction'; 
import { Account } from 'src/app/models/account'; 
import { Category } from 'src/app/models/category'; 
import { TransactionType } from 'src/app/models/TransactionType'; 
import { DatePipe, Time } from '@angular/common';

declare var $: any;

@Component({
  selector: 'app-transaction-add-edit',
  templateUrl: './transaction-add-edit.component.html',
  styleUrls: ['./transaction-add-edit.component.css']
})
export class TransactionAddEditComponent implements OnInit {

  constructor(
    private service:SharedService,
    private datePipe: DatePipe
  ) { }

  @Input() transaction:Transaction;
  TransactionTypeList:TransactionType[];
  FromAccountList:Account[];
  ToAccountList:Account[];
  CategoryList:Category[];
  TransactionID:string;
  TransactionTypeID:string;
  FromAccountID:string;
  ToAccountID:string;
  CategoryID:string;
  TransactionDate:any;
  TransactionTime:any;
  Amount:any;
  Note:string;
  TransactionTypeDisabled: boolean;
  submit:boolean;

  ngOnInit() {
    this.submit = false;
    
    if(this.transaction.TransactionIsEdit) {
      this.TransactionTypeDisabled = true;
    } else {
      this.TransactionTypeDisabled = false;
    }

    this.refreshTransactionTypeList(this.transaction.TransactionTypeID);
    this.refreshFromAccountList(null, this.transaction.FromAccountID);
    this.refreshToAccountList(this.transaction.FromAccountID, this.transaction.ToAccountID);

    this.TransactionID = this.transaction.TransactionID;
    this.TransactionDate = this.datePipe.transform(this.transaction.TransactionDate, 'yyyy-MM-dd');
    this.TransactionTime = this.transaction.TransactionTime;
    this.Amount = this.transaction.Amount;
    this.Note = this.transaction.Note;
  }

  refreshTransactionTypeList(transationTypeID?:string) {
    this.service.getTransactionTypeList().subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<TransactionType[]>(response.body);

        this.TransactionTypeList = apiResponse.Content;

        if(transationTypeID) {
          this.TransactionTypeID = transationTypeID;

          this.refreshCategoryList(this.transaction.TransactionTypeID, this.transaction.CategoryID);
        }
      } else {
        alert('Error! Cannot get transaction type list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  onTransactionTypeSelected(event:Event) {
    let selectedOptions = event.target['options'];
    let selectedIndex = selectedOptions.selectedIndex;
    let selectElementText = selectedOptions[selectedIndex].text;
    
    this.transaction.TransactionTypeID = this.TransactionTypeID;
    this.transaction.TransactionTypeName = selectElementText;

    this.refreshCategoryList(this.transaction.TransactionTypeID);
  }

  refreshFromAccountList(skipAccountID?:string, fromAccountID?:string) {
    this.service.getAccountList().subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<Account[]>(response.body);

        if(skipAccountID) {
          this.FromAccountList = apiResponse.Content.filter(x => x.AccountID != skipAccountID);
        } else {
          this.FromAccountList = apiResponse.Content;
        }

        if(fromAccountID) {
          this.FromAccountID = fromAccountID;
        }
      } else {
        alert('Error! Cannot get from account list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  onFromAccountSelected(fromAccountID:string) {
    this.refreshToAccountList(fromAccountID, this.ToAccountID);
  }

  refreshToAccountList(skipAccountID?:string, toAccountID?:string) {
    this.service.getAccountList().subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<Account[]>(response.body);

        if(skipAccountID) {
          this.ToAccountList = apiResponse.Content.filter(x => x.AccountID != skipAccountID);
        } else {
          this.ToAccountList = apiResponse.Content;
        }

        if(toAccountID) {
          this.ToAccountID = toAccountID;
        }
      } else {
        alert('Error! Cannot get to account list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  onToAccountSelected(toAccountID:string) {
    this.refreshFromAccountList(toAccountID, this.FromAccountID);
  }

  refreshCategoryList(transactionTypeID?:string, categoryID?:string) {
    this.service.getCategoryList(transactionTypeID).subscribe(response => {
      if(response.status == 200) {
        var apiResponse = new APIResponse<Category[]>(response.body);

        this.CategoryList = apiResponse.Content;

        if(categoryID) {
          this.CategoryID = categoryID;
        }
      } else {
        alert('Error! Cannot get category list.');
      }
    }, error => {
      alert(error.message);
    });
  }

  addTransaction() {
    $('#DivOverlay').show();

    this.transaction.TransactionID = this.TransactionID;
    this.transaction.TransactionTypeID = this.TransactionTypeID;
    this.transaction.FromAccountID = this.FromAccountID;

    if(this.transaction.TransactionTypeName == 'Transfer') {
      this.transaction.ToAccountID = this.ToAccountID;
    } else {
      this.transaction.CategoryID = this.CategoryID;
    }
    
    this.transaction.TransactionDate = this.TransactionDate;
    this.transaction.TransactionTime = this.TransactionTime;
    this.transaction.Amount = this.Amount;
    this.transaction.Note = this.Note;
    this.submit = true;

    if((this.TransactionTypeID != null && this.TransactionTypeID != '') &&
       (this.FromAccountID != null && this.FromAccountID != '') &&
       (
         (this.transaction.TransactionTypeName == 'Transfer' && this.ToAccountID != null && this.ToAccountID != '') ||
         (this.transaction.TransactionTypeName != 'Transfer' && this.CategoryID != null && this.CategoryID != '')
       ) &&
       this.TransactionDate != null &&
       (this.TransactionTime != null && this.TransactionTime != '') &&
       (this.Amount != null && this.Amount != '')
      ) {
      this.service.addTransaction(this.transaction).subscribe(response => {
        var apiResponse = new APIResponse<Transaction>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.transaction = new Transaction();
  
          alert("Successful added new Transaction.");
  
          $('#transactionModal').modal('hide');

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

  editTransaction() {
    $('#DivOverlay').show();

    this.transaction.TransactionID = this.TransactionID;
    this.transaction.TransactionTypeID = this.TransactionTypeID;
    this.transaction.FromAccountID = this.FromAccountID;

    if(this.transaction.TransactionTypeName == 'Transfer') {
      this.transaction.ToAccountID = this.ToAccountID;
    } else {
      this.transaction.CategoryID = this.CategoryID;
    }
    
    this.transaction.TransactionDate = this.TransactionDate;
    this.transaction.TransactionTime = this.TransactionTime;
    this.transaction.Amount = this.Amount;
    this.transaction.Note = this.Note;
    this.submit = true;

    if((this.TransactionID != null && this.TransactionID != '') &&
       (this.TransactionTypeID != null && this.TransactionTypeID != '') &&
       (this.FromAccountID != null && this.FromAccountID != '') &&
       (
         (this.transaction.TransactionTypeName == 'Transfer' && this.ToAccountID != null && this.ToAccountID != '') ||
         (this.transaction.TransactionTypeName != 'Transfer' && this.CategoryID != null && this.CategoryID != '')
       ) &&
       this.TransactionDate != null &&
       (this.TransactionTime != null && this.TransactionTime != '') &&
       (this.Amount != null && this.Amount != '')
      ) {
      this.service.editTransaction(this.transaction).subscribe(response => {
        var apiResponse = new APIResponse<Transaction>(response);
  
        if(apiResponse.StatusCode == 200) {
          this.transaction = new Transaction();
  
          alert("Successful edited new Transaction.");
  
          $('#transactionModal').modal('hide');

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
